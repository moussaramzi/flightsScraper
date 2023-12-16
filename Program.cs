
namespace flightsScraper
{
    class Program
    {
        static void Main()
        {
            using (var webScraper = new Webscraper())
            {
                // Provide departure and destination airports or locations
                string departure = "JFK";
                string destination = "LAX";

                // Call the scraping method
                webScraper.ScrapeFlightPrices(departure, destination);
            }
        }
    }
}
