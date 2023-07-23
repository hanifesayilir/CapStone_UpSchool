using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebCrawling.Dtos
{
    public class CrawlingDto
    {
        public bool AllProducts { get; set; }

        public bool SomeProducts { get; set; }

        public int ScrapeCount { get; set; }

        public bool DiscountedPrices { get; set; }

        public bool NormalPrices { get; set; }

        public bool AllPrices { get; set; }

        public int TotalPages { get; set; }

        

        public CrawlingDto()
        {
            AllProducts = false;
            SomeProducts = false;
            ScrapeCount = 0;
            DiscountedPrices = false;
            NormalPrices = false;
            AllPrices = false;
            TotalPages = 0;
        }
    }
}
