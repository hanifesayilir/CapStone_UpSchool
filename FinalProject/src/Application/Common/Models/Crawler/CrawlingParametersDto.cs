namespace Application.Common.Models.Crawler
{
    public class CrawlingParametersDto
    {

        public bool IsAllProducts { get; set; }

        public bool IsAnyNumberOfProducts { get; set; }

        public int NumberOfProducts { get; set; }

        public bool IsDiscounted { get; set; }
        public bool IsNormalPriced { get; set; }
        public bool IsAllPrice { get; set; }

        public String? JwtToken { get; set; }
        
      
 
    }
}
