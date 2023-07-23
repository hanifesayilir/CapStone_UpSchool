using Application.Common.Models.Crawler;
using Application.Features.Orders.Commands.Add;
using Application.Features.Orders.Commands.Update;
using Application.Features.OrdersStatus.Commands.Add;
using Application.Features.Products.Commands.AddList;
using Domain.Enums;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using WebCrawling.Dtos;
using WebCrawling.Enums;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;



namespace WebCrawling
{
    public class Crawling
    {
        private String _jwtToken = "";
        private string HomeUrl { get; set; }

        private Guid OrderId { get; set; }

        private int pageNumber { get; set; } = 0;

        private CrawlingDto CrawlingDto = new CrawlingDto();

        private HubConnection _hubConnection;

        private HttpClient _httpClient;

        private IWebDriver _driver;

        private List<CrawlerProduct> ProductCrawlerList = new List<CrawlerProduct>();

        private List<ProductDto> ProductListDto = new List<ProductDto>();

        private string CrawlerHubConnectionString = "https://localhost:7027/Hubs/CrawlerLogHub";




        private int vcount = 1;

        public Crawling(string url, CrawlingDto crawlingDto, string jwtToken)
        {
            HomeUrl = url;
            CrawlingDto = crawlingDto;
            _jwtToken = jwtToken;
            CreateOrderAndOrderEvent();
            Console.WriteLine($"Selected items are:  AllPrices: {CrawlingDto.AllPrices}, DiscountedPrices: {CrawlingDto.DiscountedPrices}, NormalPrices: {CrawlingDto.NormalPrices}, Allproducts: {CrawlingDto.AllProducts}, ScarpedNumber: {CrawlingDto.ScrapeCount} ");
            StartCommunication();
            Navigate(HomeUrl);
            Thread.Sleep(2000);
          
        }


        public async Task<bool> SetActualQuantity()
        {
         
          OrderUpdateCommand orderUpdateCommand = new OrderUpdateCommand();
            orderUpdateCommand.ActualQuantity = ProductCrawlerList.Count;
            orderUpdateCommand.Id = OrderId;
           
            await _hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"RequestedQuantity: {CrawlingDto.ScrapeCount}, actualQuantity: {ProductCrawlerList.Count}"));


            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
            var resp1 = await _httpClient.PostAsJsonAsync("api/Orders/UpdateActualQuantity", orderUpdateCommand);
            if (resp1 != null) return true;
            return false;
        }
        private async void CreateOrderAndOrderEvent()
        {
            var productType = Map();
            _httpClient = new HttpClient();
            const string url = "https://localhost:7027/";
            _httpClient.BaseAddress = new Uri(url);
            

            OrderAddCommand orderAddCommand = new OrderAddCommand()
            {
                Id = Guid.NewGuid(),
                RequestedAll = CrawlingDto.AllProducts,
                RequestedQuantity = CrawlingDto.ScrapeCount,
                ProductCrawlType = productType,
            };
           _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/Orders", orderAddCommand);
            response.EnsureSuccessStatusCode();

            OrderId = orderAddCommand.Id;


            await AddNewStatusToOrderEventStatus(OrderId, OrderStatus.BotStarted);
        }


        public async Task<bool> AddNewStatusToOrderEventStatus(Guid orderId, OrderStatus status)
        {

            OrderEventAddCommand orderEventAddCommand = new OrderEventAddCommand()
            {
                OrderId = orderId,
                Status = status,
            };


            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
            HttpResponseMessage responseEvent = await _httpClient.PostAsJsonAsync("api/OrderEvents/Add", orderEventAddCommand);
            responseEvent.EnsureSuccessStatusCode();
            return true;
        }




        private ProductCrawlType Map()
        {
            if (CrawlingDto is not null && CrawlingDto.AllPrices) return ProductCrawlType.All;
            else if (CrawlingDto is not null && CrawlingDto.DiscountedPrices) return ProductCrawlType.OnDisCount;
            else return ProductCrawlType.NonDisCount;
        }


        public async void Navigate(string url)
        {
            _driver.Navigate().GoToUrl(url);
            try
            {

                await _hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"Bot is started"));
                await _hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"Bot is on the {_driver.Url} page"));
                await AddNewStatusToOrderEventStatus(OrderId, OrderStatus.CrawlingStarted);

                pageNumber = _driver.FindElements(By.ClassName("page-item")).Count;
                CrawlingDto.TotalPages = pageNumber - 1;

                await _hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"There are {CrawlingDto.TotalPages} pages on spot"));



            }
            catch (Exception exception)
            {
                await AddNewStatusToOrderEventStatus(OrderId, OrderStatus.CrawlingFailed);
                _driver.Quit();
            }

        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {

            Stopwatch watch = Stopwatch.StartNew();

            while (watch.ElapsedMilliseconds < 5 * 1000)
            {
                var elements = _driver.FindElements(by);

                if (elements.Count() > 0) return elements;
                Thread.Sleep(10);

            }

            return new ReadOnlyCollection<IWebElement>(new List<IWebElement>());

        }

        public IWebElement FindElement(By by)
        {

            Stopwatch watch = Stopwatch.StartNew();

            while (watch.ElapsedMilliseconds < 5 * 1000)
            {
                var element = _driver.FindElement(by);

                if (element is not null) return element;
                Thread.Sleep(10);

            }

            return null;

        }



        public async void StartCommunication()
        {
            new DriverManager().SetUpDriver(new ChromeConfig());

            IWebDriver driver = new ChromeDriver();

            _driver = driver;
            _hubConnection = new HubConnectionBuilder()
                          .WithUrl(CrawlerHubConnectionString+"?access_token="+_jwtToken)
                          .WithAutomaticReconnect()
                          .Build();

            await _hubConnection.StartAsync();
        }


        public async Task<bool> MapCrawlerListToProductList()
        {

            await AddNewStatusToOrderEventStatus(OrderId, OrderStatus.CrawlingCompleted);
    

            foreach (CrawlerProduct productCrawler in ProductCrawlerList)
            {
                var productDto = productCrawler.CrawlerProductToMapProductDto(productCrawler, OrderId);
                ProductListDto.Add(productDto);
            }

   
            await AddNewStatusToOrderEventStatus(OrderId, OrderStatus.OrderCompleted);
            await _hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"Crawling is succesfully completed!!, {CrawlingDto.ScrapeCount} items are crawled"));
            await _hubConnection.InvokeAsync("SendLogNotificationAsync", CreateLog($"Order is completed!!!!"));

            var resp = await AddProductListToDatabase();

 
            if (resp) return true;
            return false;

        }



        public async Task<bool> AddProductListToDatabase()
        {

            var data = new StringContent(JsonConvert.SerializeObject(new
            {
                Products = ProductListDto
            }));
            data.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
            var response = await _httpClient.PostAsync("api/Products/AddList", data);
            response.EnsureSuccessStatusCode();
            return true;
        }


        CrawlerLogDto CreateLog(string message) => new CrawlerLogDto(message);


        private static void FindIfOnSaleBadgeExist(IWebElement card, out bool onSalePrice, out bool badgeTag)
        {
            onSalePrice = card.FindElements(By.ClassName("sale-price")).Count > 0 && card.FindElement(By.ClassName("sale-price")).Displayed;
            badgeTag = card.FindElements(By.ClassName("badge")).Count > 0 && card.FindElement(By.ClassName("badge")).Displayed;
        }

        private CrawlerProduct FindAllPrices(IWebElement card)
        {
            string name, price, picture;
            int view; bool onSale;
            string salePrice;
            Thread.Sleep(2000);
            GenerateNamePriceView(card, out name, out price, out view, out picture);

            bool onSalePrice, badgeTag;
            FindIfOnSaleBadgeExist(card, out onSalePrice, out badgeTag);

            if (onSalePrice) salePrice = card.FindElement(By.ClassName("sale-price")).Text;
            else salePrice = string.Empty;

            if (badgeTag) onSale = true;
            else onSale = false;
            ProductRatingEnum ratingEnum = GetRatingEnum(view);
            WriteTo(price, name, onSale, salePrice, picture, "AllPrices");

            return new CrawlerProduct(name, ratingEnum, price, salePrice, onSale, picture);
        }

        private static void GenerateNamePriceView(IWebElement card, out string name, out string price, out int view, out string picture)
        {

            name = card.FindElement(By.ClassName("product-name")).Text;
            price = card.FindElement(By.ClassName("price")).Text;
            view = card.FindElement(By.ClassName("d-flex")).FindElements(By.ClassName("bi-star-fill")).ToList().Count;
            picture = card.FindElement(By.ClassName("card-img-top")).GetAttribute("src");
        }

        private CrawlerProduct FindRestOfThePrices(IWebElement card)
        {
            string name, price, salePrice, picture = string.Empty;
            int view;
            bool onSalePrice = false, badgeTag, onSale = false;

            GenerateNamePriceView(card, out name, out price, out view, out picture);
            ProductRatingEnum ratingEnum = GetRatingEnum(view);

            FindIfOnSaleBadgeExist(card, out onSalePrice, out badgeTag);

            if (onSalePrice && badgeTag && CrawlingDto.DiscountedPrices)
            {
                salePrice = card.FindElement(By.ClassName("sale-price")).Text;
                onSale = true;
                WriteTo(price, name, onSale, salePrice, picture, "DisCountedPrces");
                return new CrawlerProduct(name, ratingEnum, price, salePrice, onSale, picture);
            }

            if (!onSalePrice && !badgeTag && CrawlingDto.NormalPrices)
            {
                salePrice = string.Empty;
                onSale = false;
                WriteTo(price, name, onSale, salePrice, picture, "DisCountedPrces");
                return new CrawlerProduct(name, ratingEnum, price, salePrice, onSale, picture);
            }

            return null;
        }

        private static ProductRatingEnum GetRatingEnum(int view)
        {
            ProductRatingEnum ratingEnum;
            switch (view)
            {
                case 5: ratingEnum = ProductRatingEnum.Five; break;
                case 4: ratingEnum = ProductRatingEnum.Four; break;
                case 3: ratingEnum = ProductRatingEnum.Three; break;
                case 2: ratingEnum = ProductRatingEnum.Two; break;
                case 1: ratingEnum = ProductRatingEnum.One; break;
                default: ratingEnum = ProductRatingEnum.None; break;

            }

            return ratingEnum;
        }

        public async void Scrape(int currentPageNumber)
        {
            IReadOnlyCollection<IWebElement> cards = FindElements(By.ClassName("card"));

            int cardIndex = 0;
            foreach (IWebElement card in cards)

            {
                cardIndex++;
                if (CrawlingDto.AllPrices)
                {
                    CrawlerProduct productDetail = FindAllPrices(card);
                    ProductCrawlerList.Add(productDetail);
                    vcount++;
                    if ((!CrawlingDto.AllProducts && vcount > CrawlingDto.ScrapeCount) ||
                        (currentPageNumber == CrawlingDto.TotalPages && ((vcount > CrawlingDto.ScrapeCount && CrawlingDto.ScrapeCount > 0)
                        || cardIndex == cards.Count)))
                    {
                        await MapCrawlerListToProductList();
                        break;
                    }


                }
                else
                {
                    CrawlerProduct product = FindRestOfThePrices(card);
                    if (product != null)
                    {
                        ProductCrawlerList.Add(product);
                        vcount++;
                    }

                    if ((!CrawlingDto.AllProducts && vcount > CrawlingDto.ScrapeCount) ||
                        (currentPageNumber == CrawlingDto.TotalPages && ((vcount > CrawlingDto.ScrapeCount && CrawlingDto.ScrapeCount > 0)
                         || cardIndex == cards.Count)))
                    {
                        await MapCrawlerListToProductList();
                        break;
                    }


                }
            }




        }
        public void NavigateToNextPages()
        {
            for (int i = 1; i < pageNumber; i++)
            {
                _driver.Navigate().GoToUrl($"{HomeUrl}?currentPage={i}");
           
                Thread.Sleep(2000);

                if (CrawlingDto.AllProducts)
                {
                    Scrape(i);
                    Thread.Sleep(2000);
                 
                }
                else
                {
                    if (vcount <= CrawlingDto.ScrapeCount)
                    {
                        Scrape(i);
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        Console.WriteLine("Crawling is completed");
                        break;
                    }
                }
            }
        }

        public void WriteToConsole(IReadOnlyCollection<IWebElement> elements)
        {
            if (elements.Count > 0)
            {
                foreach (IWebElement element in elements)
                {
                    Console.WriteLine($"{element} ==> {element.Text}");

                }
            }

        }


        private void WriteTo(string price, string name, bool onSale, string salePrice, string picture, string type)
        {
            Console.WriteLine("*******  " + type + vcount + " *******");
            Console.WriteLine("Price: --------" + price);
            Console.WriteLine("Name: --------" + name);
            Console.WriteLine("SalePrice-------" + salePrice);
            Console.WriteLine("OnSale-------" + onSale);
            Console.WriteLine("Picture-------" + picture);
        }


        public void Quit() => _driver.Quit();

        public List<CrawlerProduct> GetProductList()
        {
            return ProductCrawlerList;

        }

    }
}
