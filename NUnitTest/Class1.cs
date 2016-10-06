using Gallio.Common.Media;
using Gallio.Framework;

namespace SeleniumNUnitTest
{
    using NUnit.Framework;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using System;

    [TestFixture]
    public class GmailTests
    {
        private IWebDriver driver;

        public GmailTests() { }
        private ScreenRecorder recorder;

        [SetUp]
        public void LoadDriver()
        {
            recorder = Capture.StartRecording(new CaptureParameters { Zoom = .25 }, 5);
            Console.WriteLine("SetUp");
            driver = new ChromeDriver();
        }

        [Test]
        public void Login()
        {
            
//            recorder.OverlayManager.AddOverlay(overlay);

            driver.Navigate().GoToUrl("http://gmail.com");
            driver.FindElement(By.Id("Email")).SendKeys("test");
            driver.FindElement(By.Id("Passwd")).SendKeys("test");
            driver.FindElement(By.Id("Passwd")).Submit();

            Assert.True(driver.Title.Contains("Inbox"));
        }

        [TearDown]
        public void UnloadDriver()
        {
            Console.WriteLine(recorder.Video);
            Console.WriteLine("TearDown");
            driver.Quit();
        }
    }

    [TestFixture, Description("Tests Google Search with String data")]
    public class GoogleTests
    {
        private IWebDriver driver;

        public GoogleTests() { }

        [SetUp]
        public void LoadDriver() { driver = new ChromeDriver(); }

        [TestCase("Google")]   // searchString = Google
        [TestCase("Bing")]     // searchString = Bing
        public void Search(string searchString)
        {
            // execute Search twice with testdata: Google, Bing

            driver.Navigate().GoToUrl("http://google.com");
            driver.FindElement(By.Name("q")).SendKeys(searchString);
            driver.FindElement(By.Name("q")).Submit();

            Assert.True(driver.Title.Contains("Google"));
        }

        [TearDown]
        public void UnloadDriver() { driver.Quit(); }
    }
}