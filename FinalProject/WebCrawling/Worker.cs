
using Application.Common.Interfaces;
using Application.Common.Models.Crawler;
using Domain.Identity;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebCrawling.Dtos;

namespace WebCrawling
{
    public class Worker : BackgroundService
    {
        const string homeUrl = "https://4teker.net";
        private readonly ILogger<Worker> _logger;
        private HubConnection hubConnection;
        private readonly IJwtService  _jwtService;
        public Worker(ILogger<Worker> logger, IJwtService jwtService )
        {
            _logger = logger; 
            _jwtService= jwtService;
        }



        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
             
           
            await StartHubConnection();

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}  --" +
                    hubConnection.ConnectionId, DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }

            await hubConnection.DisposeAsync();

        }

        async Task StartHubConnection()
        {
           
            hubConnection = new HubConnectionBuilder()
              .WithUrl("https://localhost:7027/Hubs/CrawlerSendingParameters")
              .WithAutomaticReconnect()
              .Build();

            hubConnection.On<CrawlingParametersDto>("SendingParameters", HandleMessage);

            hubConnection.Closed += async (error) =>
            {
                Console.WriteLine("Connection closed. Restarting...");
                await Task.Delay(TimeSpan.FromSeconds(5));
                await hubConnection.StartAsync();
            };

            await hubConnection.StartAsync();
            Console.WriteLine($"Connection ID: {hubConnection.ConnectionId}");
        }

        void HandleMessage(CrawlingParametersDto crawlingParametersDto)
        {
            
            Console.WriteLine("Received message:");
            Console.WriteLine("NumberOfProducts " + crawlingParametersDto.NumberOfProducts);
            Console.WriteLine("IsAnyNumberOfProducts " + crawlingParametersDto.IsAnyNumberOfProducts);
            Console.WriteLine("isAllProdcuts " + crawlingParametersDto.IsAllProducts);
            Console.WriteLine("allPrice " + crawlingParametersDto.IsAllPrice);
            Console.WriteLine("normalPrice " + crawlingParametersDto.IsNormalPriced);
            Console.WriteLine("dsicounted: " + crawlingParametersDto.IsDiscounted);
            Console.WriteLine("jwtToken: " + crawlingParametersDto.JwtToken);


            CrawlingDto crawlingDto = new CrawlingDto()
            {
                NormalPrices = crawlingParametersDto.IsNormalPriced,
                AllPrices = crawlingParametersDto.IsAllPrice,
                DiscountedPrices = crawlingParametersDto.IsDiscounted,
                ScrapeCount = crawlingParametersDto.NumberOfProducts,
                SomeProducts = crawlingParametersDto.IsAnyNumberOfProducts,
                AllProducts = crawlingParametersDto.IsAllProducts,
                
            };

            Console.WriteLine("crawlingDto message:");
            Console.WriteLine("NormalPrices: " + crawlingDto.NormalPrices);
            Console.WriteLine("AllPrices " + crawlingDto.AllPrices);
            Console.WriteLine("DiscountedPrices " + crawlingDto.DiscountedPrices);
            Console.WriteLine("ScrapeCount " + crawlingDto.ScrapeCount);
            Console.WriteLine("someProducts " + crawlingDto.SomeProducts);
            Console.WriteLine("AllProcuts: " + crawlingDto.AllProducts);

            Crawler(crawlingDto, crawlingParametersDto.JwtToken);

        }


        async Task Crawler(CrawlingDto val, String jwtToken)
        {
            Crawling crawling = new Crawling(homeUrl, val, jwtToken);

            crawling.NavigateToNextPages();

            await crawling.SetActualQuantity();

            crawling.Quit();

            await hubConnection.InvokeAsync("GetMessages", "Crawler completed");


            Console.WriteLine("Crawler completed" + val.AllPrices);


        }
    }
}
