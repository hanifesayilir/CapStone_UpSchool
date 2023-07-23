namespace Application.Features.Products.Queries.GetAllByOrderId
{
    public class ProductGetAllDto
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }
       
        public string Name { get; set; }

        public string Picture { get; set; }

        public bool IsOnSale { get; set; }

        public decimal Price { get; set; }

        public decimal SalePrice { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

    

        public ProductGetAllDto(Guid id, Guid orderId, string name, string picture, bool isOnSale, decimal price, decimal salePrice, DateTimeOffset createdOn)
        {
            Id = id;
            OrderId = orderId;
            Name = name;
            Picture = picture;
            IsOnSale = isOnSale;
            Price = price;
            SalePrice = salePrice;
            CreatedOn   = createdOn;
           
        }

        public ProductGetAllDto()
        {
        }
    }
}
