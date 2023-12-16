
using static flightsScraper.Webscraper;

namespace flightsScraper
{
    class Program
    {
         static void Main()
        {
            using (var webScraper = new Webscraper())
            {
                Console.WriteLine("Enter departure city:");
                string departureCity = Console.ReadLine();

                // Accept cookies
                webScraper.AcceptCookies();
                List<FlightInfo> flightInfos = webScraper.EnterDepartureAndSearch(departureCity);
                // Define the CSV file path
                string csvFilePath = Path.Combine("csv/data.csv");
                if (flightInfos != null && flightInfos.Any())
                {
                    webScraper.WriteToCsv(flightInfos, csvFilePath);
                }
                // Provide departure and destination airports or locations


              
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
