using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Appium
{
    public class AppiumTouchCommands
    {
        private readonly IWebDriver driver;
        public Dictionary<String, Double> coords;

        public AppiumTouchCommands(IWebDriver driver)
        {
            this.driver = driver;
        }

        public void ResetCoordinates()
        {
            coords = new Dictionary<string, double>();
        }

        public void AddCoordinate(string key, double value)
        {
            coords.Add(key, value);
        }

        public void Execute(string command)
        {
            driver.ExecuteJavaScript("mobile: " + command, coords);
        }

        public void Tap(double X, double Y, int count = 1, double duration = .1)
        {
            coords = new Dictionary<string, double>();
            coords.Add("x", X);
            coords.Add("y", Y);
            coords.Add("tapCount", count);
            coords.Add("duration", duration);
            driver.ExecuteJavaScript("mobile: swipe", coords);
        }

        public void ScrollTo(string id)
        {
            var elementObject = new Dictionary<string, string>();
            elementObject.Add("element", id);
            driver.ExecuteJavaScript("mobile: scrollTo", elementObject);
        }

        public void Swipe(double startX, double startY, double endX, double endY)
        {
            coords = new Dictionary<string, double>();
            coords.Add("startX", startX);
            coords.Add("startY", startY);
            coords.Add("endX", endX);
            coords.Add("endY", endY);
            driver.ExecuteJavaScript("mobile: swipe", coords);
        }

        public void SwipeDown()
        {
            coords = new Dictionary<string, double>();
            coords.Add("startX", 0.5);
            coords.Add("startY", 0.95);
            coords.Add("endX", 0.5);
            coords.Add("endY", 0.05);
            driver.ExecuteJavaScript("mobile: swipe", coords);
        }

        public void SwipeUp()
        {
            coords = new Dictionary<string, double>();
            coords.Add("startX", 0.5);
            coords.Add("startY", 0.05);
            coords.Add("endX", 0.5);
            coords.Add("endY", 0.95);
            driver.ExecuteJavaScript("mobile: swipe", coords);
        }

        public void SwipeRight()
        {
            coords = new Dictionary<string, double>();
            coords.Add("startX", 0.05);
            coords.Add("startY", 0.5);
            coords.Add("endX", 0.95);
            coords.Add("endY", 0.5);
            driver.ExecuteJavaScript("mobile: swipe", coords);
        }

        public void SwipeLeft()
        {
            coords = new Dictionary<string, double>();
            coords.Add("startX", 0.95);
            coords.Add("startY", 0.5);
            coords.Add("endX", 0.05);
            coords.Add("endY", 0.5);
            driver.ExecuteJavaScript("mobile: swipe", coords);
        }
    }
}