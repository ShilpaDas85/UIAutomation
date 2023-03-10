using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Xml;
using System.Text;
using DocumentFormat.OpenXml.Spreadsheet;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using WebSupergoo.ABCpdf;
using System.Xml.Linq;

namespace TestProject9
{
    public class Tests
    {
        IWebDriver driver;
        int timeoutInSeconds = 10;

        
        [SetUp]
        public void Launch_browser()
        {
            // Launch browser 
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void Test1()
        {
            // Reading test data from xml
            var testdataFile = "Testdata.xml";
            var presentDirectory = Directory.GetCurrentDirectory();

            // Determining the full path of file
            var testdataFilepath = Path.Combine(presentDirectory, testdataFile);
            XElement testData = XElement.Load(testdataFilepath);

            // Retriving data from XML
            IEnumerable<string> url = from item in testData.Descendants("Item")
                                          select (string)item.Attribute("TestUrl");
            IEnumerable<string> invalidUser = from item in testData.Descendants("Item")
                                     select (string)item.Attribute("InvalidUser");
            IEnumerable<string> errorMessageExpected = from item in testData.Descendants("Item")
                                              select (string)item.Attribute("ErrorMessage");


            // Navigate to the site
            driver.Url = url.ToString();

            // Click on login button
            IWebElement loginButton = driver.FindElement(By.XPath("//a[@data-testid='login']"));
            loginButton.Click();

            // Wait for the dialog appears
            WebDriverWait wait_dialog = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait_dialog.Until(ExpectedConditions.ElementIsVisible(By.XPath("//span[text()='Phone, email, or username']")));

            // Enter invalid id 
            IWebElement usernameBox = driver.FindElement(By.XPath("//span[text()='Phone, email, or username']"));
            usernameBox.Clear();
            usernameBox.SendKeys(invalidUser.ToString());

            // Click on next Button 
            IWebElement nextButton = driver.FindElement(By.XPath("//span[text()='Next']"));
            nextButton.Click();

            // Wait for Error message
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@data-testid='toast']")));

            // Capture the error message and verify
            string error_message = driver.FindElements(By.XPath("//div[@data-testid='toast']")).ToString();
            Assert.That(error_message, Is.EqualTo(errorMessageExpected.ToString()));

        }



        [TearDown]
        public void Close_browser()
        {
            driver.Close();
        }
    }
}
