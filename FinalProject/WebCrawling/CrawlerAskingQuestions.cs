using System.Numerics;
using WebCrawling.Dtos;

namespace WebCrawling
{
    public class CrawlerAskingQuestions
    {

        private static string ASKFORALLORNUMBER = "How many items would you like to crawl? Please enter a number if you do not want to crawl all items or enter letter A for all.";

        private static string ASKPRICETYPE = "Which items do you want to scrape? A) All B) Discounted C) Ones with Normal Prices";
        
        private static string VALIDITYREPEAT = "Please enter a valid value.All or an integer is valid.";

        CrawlingDto crawlingdto = new CrawlingDto();

        public CrawlingDto AskIfAllItemsOrNumberOfItemsToBeCrawled()
        {

            int number = 0;
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine(ASKFORALLORNUMBER);
            Console.WriteLine("------------------------------------------");
            var consoleInput = Console.ReadLine();

            if (!string.IsNullOrEmpty(consoleInput))
            {

                if (consoleInput.ToLower().Equals("all") || consoleInput.ToLower().Equals("a")) crawlingdto.AllProducts = true;

                if (Int32.TryParse(consoleInput, out number))
                {
                    crawlingdto.AllProducts = false;
                    crawlingdto.ScrapeCount = number;
                }

            }
            else
            {
                Console.WriteLine(VALIDITYREPEAT);
                AskIfAllItemsOrNumberOfItemsToBeCrawled();
            }

            Console.WriteLine($"Selected items are: AllProdcuts: {crawlingdto.AllProducts}, ScrapeCount: {crawlingdto.ScrapeCount}");
            return crawlingdto;
        }



        public CrawlingDto AskQuestionForWhichPriceTobeScraped()
        {
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("------------------------------------------");
            Console.WriteLine(ASKPRICETYPE);
             var selection = Console.ReadLine();
            if (!string.IsNullOrEmpty(selection) && selection.ToUpper().Equals("A")) crawlingdto.AllPrices = true;
            else if (!string.IsNullOrEmpty(selection) && selection.ToUpper().Equals("B")) crawlingdto.DiscountedPrices = true;
            else if (!string.IsNullOrEmpty(selection) && selection.ToUpper().Equals("C")) crawlingdto.NormalPrices = true;
            else AskQuestionForWhichPriceTobeScraped();

            Console.WriteLine($"Selected items are:  AllPrices: {crawlingdto.AllPrices}, DiscountedPrices: {crawlingdto.DiscountedPrices}, NormalPrices: {crawlingdto.NormalPrices}");
            return crawlingdto;
        }
    }
}
