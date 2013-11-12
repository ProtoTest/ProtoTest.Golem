using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using OpenQA.Selenium;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Core
{
    public class Config
    {
        private static ConfigSettings _settings;

        public static ConfigSettings Settings
        {
            get { return _settings ?? (_settings = new ConfigSettings()); }
            set { _settings = value; }
        }

        public static string GetConfigValue(string key, string defaultValue)
        {
            string setting = ConfigurationManager.AppSettings[key];
            if (setting == null)
                return defaultValue;
            return setting;
        }
    }

    public class ConfigSettings
    {
        public AppiumSettings appiumSettings;
        public HttpProxy httpProxy;
        public ReportSettings reportSettings;
        public RuntimeSettings runTimeSettings;
        public ImageCompareSettings imageCompareSettings;

        public ConfigSettings()
        {
            runTimeSettings = new RuntimeSettings();
            reportSettings = new ReportSettings();
            httpProxy = new HttpProxy();
            appiumSettings = new AppiumSettings();
            imageCompareSettings = new ImageCompareSettings();
        }

        public class AppiumSettings
        {
            public string activity;
            public string appOs;
            public string appPath;
            public bool launchApp = false;
            public string package;

            public AppiumSettings()
            {
                launchApp = Common.IsTruthy(Config.GetConfigValue("LaunchApp", "False"));
                appPath = Config.GetConfigValue("AppPath", "");
                package = Config.GetConfigValue("AppPackage", "");
                activity = Config.GetConfigValue("AppActivity", "");
                appOs = Config.GetConfigValue("AppOs", "Android");
            }
        }

        public class HttpProxy
        {
            public int proxyServerPort;
            public int proxyPort;
            public bool startProxy;
            public bool useProxy;

            public HttpProxy()
            {
                proxyServerPort = int.Parse(Config.GetConfigValue("ProxyServerPort", "8878"));
                startProxy = Common.IsTruthy(Config.GetConfigValue("StartProxy", "True"));
                useProxy = Common.IsTruthy(Config.GetConfigValue("UseProxy", "True"));
                proxyPort = int.Parse(Config.GetConfigValue("ProxyPort", "8876"));
            }
        }

        public class ReportSettings
        {
            public bool actionLogging;
            public bool commandLogging;
            public bool htmlOnError;
            public bool screenshotOnError;
            public bool spellChecking;
            public bool videoRecordingOnError;

            public ReportSettings()
            {
                htmlOnError = Common.IsTruthy(Config.GetConfigValue("HtmlOnError", "True"));
                screenshotOnError = Common.IsTruthy(Config.GetConfigValue("ScreenshotOnError", "True"));
                videoRecordingOnError = Common.IsTruthy(Config.GetConfigValue("VideoRecordingOnError", "True"));
                commandLogging = Common.IsTruthy(Config.GetConfigValue("CommandLogging", "True"));
                actionLogging = Common.IsTruthy(Config.GetConfigValue("ActionLogging", "True"));
                spellChecking = Common.IsTruthy(Config.GetConfigValue("SpellChecking", "False"));
            }
        }

        public class RuntimeSettings
        {
            public List<WebDriverBrowser.Browser> Browsers = new List<WebDriverBrowser.Browser>();
            public int CommandDelayMs;
            public int DegreeOfParallelism;
            public int ElementTimeoutSec;
            public string EnvironmentUrl;
            public bool HighlightOnVerify;
            public string HostIp;
            public bool LaunchBrowser;
            public int PageTimeoutSec;
            public bool RunOnRemoteHost;
            public int TestTimeoutMin;
            public string BrowserResolution;

            public RuntimeSettings()
            {
                Browsers = GetBrowserList();

                LaunchBrowser = Common.IsTruthy(Config.GetConfigValue("LaunchBrowser", "True"));
                TestTimeoutMin = int.Parse(Config.GetConfigValue("TestTimeoutMin", "5"));
                ElementTimeoutSec = int.Parse(Config.GetConfigValue("ElementTimeoutSec", "20"));
                PageTimeoutSec = int.Parse(Config.GetConfigValue("PageTimeoutSec", "30"));
                EnvironmentUrl = Config.GetConfigValue("EnvironmentUrl", "");
                DegreeOfParallelism = int.Parse(Config.GetConfigValue("DegreeOfParallelism", "5"));
                CommandDelayMs = int.Parse(Config.GetConfigValue("CommandDelayMs", "0"));
                RunOnRemoteHost = Common.IsTruthy(Config.GetConfigValue("RunOnRemoteHost", "False"));
                HostIp = Config.GetConfigValue("HostIp", "localhost");
                HighlightOnVerify = Common.IsTruthy(Config.GetConfigValue("HighlightOnVerify", "False"));
                BrowserResolution = Config.GetConfigValue("BrowserResolution", "Default");
            }


            private List<WebDriverBrowser.Browser> GetBrowserList()
            {
                var browsers = new List<WebDriverBrowser.Browser>();
                string browser = Config.GetConfigValue("Browser", "null");
                if (browser != "null")
                    browsers.Add(WebDriverBrowser.getBrowserFromString(browser));
                for (int i = 1; i < 5; i++)
                {
                    browser = Config.GetConfigValue("Browser" + i, "null");
                    if (browser != "null")
                        browsers.Add(WebDriverBrowser.getBrowserFromString(browser));
                }
                if (browsers.Count == 0)
                    browsers.Add(WebDriverBrowser.Browser.Firefox);
                return browsers;
            }

            public int GetTimeoutSettings()
            {
                return ElementTimeoutSec;
            }
        }

        public class ImageCompareSettings
        {
            public byte fuzziness;
            public float accuracy;
            public bool updateImages;

            public ImageCompareSettings()
            {
                fuzziness = Byte.Parse(Config.GetConfigValue("Fuzziness", "30"));
                accuracy = float.Parse(Config.GetConfigValue("Accuracy", ".01"));
                updateImages = Common.IsTruthy(Config.GetConfigValue("UpdateImages", "false"));
            }
        }
    }
}