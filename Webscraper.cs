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
        /*

        public void ScrapeFlightPrices(string departure, string destination)
        {
            try
            {
                // Navigate to Google Flights
                driver.Navigate().GoToUrl("https://www.google.com/flights");

                // Wait for the page to load
                Thread.Sleep(5000);

                // Find and interact with the elements to enter departure and destination
                IWebElement departureInput = driver.FindElement(By.CssSelector("input[jsname='yrriRe'][role='combobox'][aria-haspopup='true'][aria-expanded='false'][jslog='8582;ved:2ahUKEwjs_9r9lZSDAxXixxEIHQBCCfkQhkN6BAgDEAw;track:click']"));
                departureInput.Clear(); // Clear existing text
                departureInput.SendKeys(departure);

                IWebElement destinationInput = driver.FindElement(By.CssSelector("input[jsname='yrriRe'][aria-label='Waarheen nog meer?']"));
                destinationInput.Clear(); // Clear existing text
                destinationInput.SendKeys(destination);


                // Wait for the dropdown suggestions to load
                Thread.Sleep(2000);

                // Select the first suggestion for both departure and destination
                IWebElement firstSuggestion = driver.FindElement(By.XPath("li[role='option']"));
                firstSuggestion.Click();

                // Click on the search button
                IWebElement searchButton = driver.FindElement(By.XPath("//button[@aria-label='Search flights']"));
                searchButton.Click();

                // Wait for the search results to load (you might need to adjust the wait time)
                Thread.Sleep(5000);

                // Implement your scraping logic here
                // Use driver.FindElement and other methods to find and extract data

                // For example, extracting prices
                IWebElement priceElement = driver.FindElement(By.XPath("//div[@class='flt-subhead1 gws-flights-results__price']"));
                string price = priceElement.Text;

                // Print the scraped price
                Console.WriteLine($"Flight price: {price}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        */

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
                driver.Navigate().GoToUrl("https://www.google.com/flights");

                // Wait for the page to load
                Thread.Sleep(4000);

                // Find and interact with the element to enter departure
                Console.WriteLine("Before finding departure input");

                IWebElement departureInput = driver.FindElement(By.CssSelector("input[jsname='yrriRe']"));
                departureInput.Clear(); // Clear existing text
                departureInput.SendKeys(departure);
                Thread.Sleep(2000);

                Console.WriteLine("After entering departure");
                actions.SendKeys(Keys.Enter).Perform();

                // Wait for the dropdown suggestions to load
                Thread.Sleep(2000);
                actions.SendKeys(Keys.Enter).Perform();

                // Simulate pressing the "Enter" key to select the first suggestion
                // Find the button based on text content
                /*
                Console.WriteLine("Before finding search button");
                IWebElement searchButton = driver.FindElement(By.CssSelector("button[jsname='vLv7Lb']"));
                Console.WriteLine("After finding search button");

                // Click on the search button
                Console.WriteLine("Before clicking search button");
                searchButton.Click();
                Console.WriteLine("After clicking search button");
                */
                // Wait for the search results to load (you might need to adjust the wait time)
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

                Console.WriteLine("Scraping completed");
                return flightInfos;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public void WriteToCsv(List<FlightInfo> flightInfos, string filePath)
        {
            try
            {
                using (var writer = new StreamWriter(filePath))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(flightInfos);
                }

                Console.WriteLine($"Data written to CSV file: {filePath}");
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
