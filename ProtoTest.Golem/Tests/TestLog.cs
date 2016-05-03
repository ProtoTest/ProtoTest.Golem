using System.Drawing.Imaging;
using System.IO;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Tests
{
    internal class TestLog : WebDriverTestBase
    {
        [Test]
        public void TestLogFile()
        {
            Log.Message("This is a test");
        }

        [Test]
        public void TestLogImage()
        {
            driver.Navigate().GoToUrl("http://www.google.com");
            
            Log.Image(driver.GetScreenshot());
        }


        [Test]
        public void TestLogVideo()
        {
            driver.Navigate().GoToUrl("http://www.google.com");

            Log.Video(testData.recorder.Video);
        }
    }
}