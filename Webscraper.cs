using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using OpenQA.Selenium.Support.UI;
using System.Formats.Asn1;
using System.Globalization;
using CsvHelper;
using static flightsScraper.Webscraper;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.DevTools.V118.IndexedDB;
using Newtonsoft.Json;


namespace flightsScraper
{
    public class Webscraper : IDisposable
    {
        private IWebDriver driver;

        public Webscraper()
        {
            // Initialize WebDriver
            driver = new ChromeDriver();
        }
       

        public void AcceptCookies()
        {
            try
            {
                // Navigate to Google Flights
                driver.Navigate().GoToUrl("https://www.google.com/flights");

                // Wait for the cookie pop-up to appear (adjust the time accordingly)
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                IWebElement cookiePopup = wait.Until(d => d.FindElement(By.CssSelector("button[jsname='b3VHJd']")));

                // Click on the "Accept" button
                cookiePopup.Click();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while accepting cookies: {ex.Message}");
            }
        }

        public List<FlightInfo> EnterDepartureAndSearch(string departure)
        {
            Actions actions = new Actions(driver);
            List<FlightInfo> flightInfos = new List<FlightInfo>();
            try
            {
                // Navigate to Google Flights

                // Wait for the page to load

                // Find and interact with the element to enter departure
                Console.WriteLine("Before finding departure input");

                IWebElement departureInput = driver.FindElement(By.CssSelector("input[jsname='yrriRe']"));
                departureInput.Clear(); // Clear existing text
                departureInput.SendKeys(departure);
                Thread.Sleep(2000);

                Console.WriteLine("After entering departure");
                actions.SendKeys(Keys.Enter).Perform();

                // Wait and press enter
                Thread.Sleep(2000);
                actions.SendKeys(Keys.Enter).Perform();

            
                Thread.Sleep(6000);

                // Find the ol element containing the flight results
                new WebDriverWait(driver, TimeSpan.FromSeconds(10)).Until(driver => driver.FindElement(By.XPath("//ol[@class='SD4Ugf']")));

                IWebElement flightResultsOl = driver.FindElement(By.XPath("//ol[@class='SD4Ugf']"));

                // Find all child li elements within the ol
                IReadOnlyCollection<IWebElement> flightResultItems = flightResultsOl.FindElements(By.XPath("//li[contains(@class, 'lPyEac P0ukfb')]"));

                Console.WriteLine("Before loop");
                // Iterate through the results
                foreach (IWebElement resultItem in flightResultItems)
                {
                    // Extract name and date
                    IWebElement nameElement = resultItem.FindElement(By.XPath(".//h3[@class='W6bZuc YMlIz']"));
                    string name = nameElement.Text;

                    IWebElement dateElement = resultItem.FindElement(By.XPath(".//div[@class='CQYfx']"));
                    string date = dateElement.Text;

                    // Extract price
                   

                    IWebElement priceElement = resultItem.FindElement(By.XPath(".//span[@class='QB2Jof xLPuCe']/span[@aria-label]"));
                    string price = priceElement.GetAttribute("aria-label").Trim();

                    // Print the extracted information to the console
                    Console.WriteLine($"Name: {name}, Date: {date}, Price: {price}");

                    // Create FlightInfo object and add to the list
                    flightInfos.Add(new FlightInfo { Name = name, Date = date, Price = price });
                }
                Console.WriteLine("After loop");
                Console.WriteLine($"Number of flightInfos: {flightInfos.Count}");

                Console.WriteLine("Scraping completed");
                return flightInfos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public void WriteToCsvAndJson(List<FlightInfo> flightInfos, string filePath, string jsonFilePath)
        {
            try
            {
                // Use 'true' to append to an existing file
                using (var writer = new StreamWriter(filePath, true))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                using (var jsonWriter = new StreamWriter(jsonFilePath, true))

                {
                    // Check if the file exists
                    if (!File.Exists(filePath))
                    {
                        // If the file doesn't exist, write the header
                        csv.WriteRecords(new List<FlightInfo> { new FlightInfo { Name = "Name", Date = "Date", Price = "Price" } });
                    }

                    // Append the records
                    csv.WriteRecords(flightInfos);
                    string json = JsonConvert.SerializeObject(flightInfos, Formatting.Indented);
                    jsonWriter.WriteLine(json);
                }

                Console.WriteLine($"Data written to CSV file: {filePath}");
                Console.WriteLine($"Data written to json file: {jsonFilePath}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while writing to CSV: {ex.Message}");
            }
        }


        public void Dispose()
        {
            // Dispose of the WebDriver when done
            driver.Dispose();
        }
        public class FlightInfo
        {
            public string Name { get; set; }
            public string Date { get; set; }
            public string Price { get; set; }
        }
    }
        
}
