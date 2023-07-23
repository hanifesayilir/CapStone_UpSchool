
using Application.Features.Products.Commands.AddList;
using WebCrawling.Enums;

namespace WebCrawling
{
    public class CrawlerProduct
    {
        private Guid Id { get; set; }
        private string Name { get; set; }

        private string Picture { get; set; }

        private ProductRatingEnum Rating { get; set; }

        private string Price { get; set; }

        private string SalePrice { get; set; }

        private bool OnSale { get; set; }

        public CrawlerProduct(string name, ProductRatingEnum rating, string price, string salePrice, bool onSale, string picture)
        {
            Id = Guid.NewGuid();
            Name = name;
            Rating = rating;
            Price = price;
            SalePrice = salePrice;
            OnSale = onSale;
            Picture = picture;
        }


        public ProductDto CrawlerProductToMapProductDto(CrawlerProduct crawlerProduct, Guid orderId)
        {
            decimal tempValue = 0;

            if (!crawlerProduct.OnSale) tempValue = 0;
            else tempValue =Parse(crawlerProduct.SalePrice);

          


            return new ProductDto()
            {
                Id = Guid.NewGuid(),
                OrderId= orderId,
                Name = crawlerProduct.Name,
                IsOnSale = crawlerProduct.OnSale,
                Price = Parse(crawlerProduct.Price),
                SalePrice = tempValue,
                Picture = crawlerProduct.Picture,
            };
        }

        private decimal Parse(string s)
        {
            s = s.Replace(",", ".").Replace("$", "");
            return decimal.Parse(s);
        }

    



    }
}
