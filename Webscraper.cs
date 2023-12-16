using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

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

        public void ScrapeFlightPrices(string departure, string destination)
        {
            try
            {
                // Navigate to Google Flights
                driver.Navigate().GoToUrl("https://www.google.com/flights");

                // Wait for the page to load
                Thread.Sleep(5000);

                // Find and interact with the elements to enter departure and destination
                IWebElement departureInput = driver.FindElement(By.CssSelector("input[jsname='yrriRe'][aria-label='Flying from']"));
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

        public void Dispose()
        {
            // Dispose of the WebDriver when done
            driver.Dispose();
        }
    }
}
