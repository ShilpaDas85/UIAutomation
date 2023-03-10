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

namespace TestProject9
{
    public class Tests
    {
        IWebDriver driver;

        // Test data parsing
        //XmlDocument doc = new XmlDocument();
        //System.IO.StreamReader raw_data = new System.IO.StreamReader(@":\Users\shilp\source\repos\TestProject9\Testdata.xml");
        //XmlDocument doc = new XmlDocument();
        //Doc.Load(@"c:\\Users\shilp\source\repos\TestProject9\Testdata.xml");
       //testData.Load("@C:\Users\shilp\source\repos\TestProject9\Testdata.xml");
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
            // Navigate to the site
            //driver.Url = testData.SelectSingleNode("TestUrl").InnerText;
            driver.Url = "https://twitter.com/";

            // Click on login button
            IWebElement loginButton = driver.FindElement(By.XPath("//a[@data-testid='login']"));
            loginButton.Click();

            // Wait for the dialog appears
            WebDriverWait wait_dialog = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait_dialog.Until(ExpectedConditions.ElementIsVisible(By.XPath("//span[text()='Phone, email, or username']")));

            // Enter invalid id 
            IWebElement usernameBox = driver.FindElement(By.XPath("//span[text()='Phone, email, or username']"));
            usernameBox.Clear();
            usernameBox.SendKeys("000000000000");

            // Click on next Button 
            IWebElement nextButton = driver.FindElement(By.XPath("//span[text()='Next']"));
            nextButton.Click();

            // Wait for Error message
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//div[@data-testid='toast']")));

            // Capture the error message and verify
            string error_message = driver.FindElements(By.XPath("//div[@data-testid='toast']")).ToString();
         
        }

        [TearDown]
        public void Close_browser()
        {
            driver.Close();
        }
    }
}