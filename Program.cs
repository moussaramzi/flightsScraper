using static flightsScraper.Webscraper;

namespace flightsScraper
{
    class Program
    {
        static void Main()
        {
            try
            {
                using (var webScraper = new Webscraper())
                {
                    Console.WriteLine("Enter departure city:");
                    string departureCity = Console.ReadLine();

                    // Accept cookies
                    webScraper.AcceptCookies();
                    List<FlightInfo> flightInfos = webScraper.EnterDepartureAndSearch(departureCity);

                    if (flightInfos != null && flightInfos.Any())
                    {
                        string csvFilePath = ("C:\\Users\\mouss_66fvi74\\source\\repos\\flightsScraper\\csv\\data.csv");
                        string jsonFilePath = ("C:\\Users\\mouss_66fvi74\\source\\repos\\flightsScraper\\json\\data.json");
                        webScraper.WriteToCsvAndJson(flightInfos, csvFilePath, jsonFilePath);
                    }
                    else
                    {
                        Console.WriteLine("No flight information found.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
