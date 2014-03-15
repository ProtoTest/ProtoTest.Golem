using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Xml;
using Castle.Core.Logging;
using ProtoTest.Golem.Appium;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Core
{
    /// <summary>
    /// The Config class holds instantiates the ConfigSettings class, and any Config-related functions
    /// </summary>
    public class Config
    {
        private static ConfigSettings _settings;

        public static ConfigSettings Settings
        {
            get { return _settings ?? (_settings = new ConfigSettings()); }
            set { _settings = value; }
        }

        /// <summary>
        /// Returns the App.config value for requested key, or default value if not defined.
        /// </summary>
        /// <param name="key">Application configuration key</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns></returns>
        public static string GetConfigValue(string key, string defaultValue)
        {
            string setting = ConfigurationManager.AppSettings[key];
            if (setting == null)
            {
                return defaultValue;
            }

            return setting;
        }

        /// <summary>
        /// Updates the App.config setting key with value
        /// </summary>
        /// <param name="key">Application configuration key</param>
        /// <param name="value">Application configuration key value to set</param>
        public static void UpdateConfigFile(string key, string value)
        {
            var doc = new XmlDocument();
            string path = Assembly.GetCallingAssembly().Location + ".config";
            doc.Load(path);
            doc.SelectSingleNode("//add[@key='" + key + "']").Attributes["value"].Value = value;
            doc.Save(path);

            path = Directory.GetCurrentDirectory().Replace(@"\bin\Debug", "").Replace(@"\bin\Release", "") +
                   "\\App.config";
            doc.Load(path);
            doc.SelectSingleNode("//add[@key='" + key + "']").Attributes["value"].Value = value;
            doc.Save(path);
        }
    }
    /// <summary>
    /// ConfigSettings holds each config section, and reads them in from the App.Config upon instantiation.  To override these settings put the commands in a [FixtureInitializer]
    /// </summary>
    public class ConfigSettings
    {
        public AppiumSettings appiumSettings;
        public HttpProxy httpProxy;
        public ImageCompareSettings imageCompareSettings;
        public ReportSettings reportSettings;
        public RuntimeSettings runTimeSettings;
        public PurpleSettings purpleSettings;

        public ConfigSettings()
        {
            runTimeSettings = new RuntimeSettings();
            reportSettings = new ReportSettings();
            httpProxy = new HttpProxy();
            appiumSettings = new AppiumSettings();
            imageCompareSettings = new ImageCompareSettings();
            purpleSettings = new PurpleSettings();
        }

        /// <summary>
        /// Contains all settings related to appium support.
        /// </summary>
        public class AppiumSettings
        {
            public string activity;
            public string device;
            public string appPath;
            public bool launchApp = false;
            public string package;
            public string appiumPort;
            public bool useIpa;
            public string appiumServerPath;
            public bool resetApp = false;
            public string bundleId;

            public AppiumSettings()
            {
                launchApp = Common.IsTruthy(Config.GetConfigValue("LaunchApp", "False"));
                appPath = Config.GetConfigValue("AppPath", "");
                package = Config.GetConfigValue("AppPackage", "");
                activity = Config.GetConfigValue("AppActivity", "");
                device = Config.GetConfigValue("Device", "");
                appiumPort = Config.GetConfigValue("AppiumPort", "4723");
                useIpa = Common.IsTruthy(Config.GetConfigValue("UseIpa", "False"));
                appiumServerPath = Config.GetConfigValue("AppiumServerPath", "");
                resetApp = Common.IsTruthy(Config.GetConfigValue("ResetApp", "False"));
                bundleId = Config.GetConfigValue("BundleId", "");
            }
        }

        /// <summary>
        /// Contains all settings related to the BrowserMobProxy
        /// </summary>
        public class HttpProxy
        {
            public int proxyPort;
            public int proxyServerPort;
            public bool startProxy;
            public bool useProxy;

            public HttpProxy()
            {
                proxyServerPort = int.Parse(Config.GetConfigValue("ProxyServerPort", "18880"));
                startProxy = Common.IsTruthy(Config.GetConfigValue("StartProxy", "False"));
                useProxy = Common.IsTruthy(Config.GetConfigValue("UseProxy", "False"));
                proxyPort = int.Parse(Config.GetConfigValue("ProxyPort", "18881"));
            }
        }

        /// <summary>
        /// Contains settings for image comparison.
        /// </summary>
        public class ImageCompareSettings
        {
            public float accuracy;
            public byte fuzziness;
            public bool updateImages;

            public ImageCompareSettings()
            {
                fuzziness = Byte.Parse(Config.GetConfigValue("Fuzziness", "50"));
                accuracy = float.Parse(Config.GetConfigValue("Accuracy", ".01"));
                updateImages = Common.IsTruthy(Config.GetConfigValue("UpdateImages", "false"));
            }
        }

        /// <summary>
        /// Specify what should show up in the report
        /// </summary>
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

        /// <summary>
        /// Specify execution settings
        /// </summary>
        public class RuntimeSettings
        {
            public string BrowserResolution;
            public List<WebDriverBrowser.Browser> Browsers = new List<WebDriverBrowser.Browser>();
            public int CommandDelayMs;
            public int DegreeOfParallelism;
            public int ElementTimeoutSec;
            public int OpenWindowTimeoutSec;
            public string EnvironmentUrl;
            public bool HighlightOnVerify;
            public List<string> Hosts;
            public bool LaunchBrowser;
            public int PageTimeoutSec;
            public bool RunOnRemoteHost;
            public int TestTimeoutMin;

            public RuntimeSettings()
            {
                Browsers = GetBrowserList();
                Hosts = GetHostsList();
                LaunchBrowser = Common.IsTruthy(Config.GetConfigValue("LaunchBrowser", "True"));
                TestTimeoutMin = int.Parse(Config.GetConfigValue("TestTimeoutMin", "5"));
                ElementTimeoutSec = int.Parse(Config.GetConfigValue("ElementTimeoutSec", "5"));
                OpenWindowTimeoutSec = int.Parse(Config.GetConfigValue("WindowOpenTimeoutSec", "10"));
                PageTimeoutSec = int.Parse(Config.GetConfigValue("PageTimeoutSec", "30"));
                EnvironmentUrl = Config.GetConfigValue("EnvironmentUrl", "");
                DegreeOfParallelism = int.Parse(Config.GetConfigValue("DegreeOfParallelism", "5"));
                CommandDelayMs = int.Parse(Config.GetConfigValue("CommandDelayMs", "0"));
                RunOnRemoteHost = Common.IsTruthy(Config.GetConfigValue("RunOnRemoteHost", "False"));

                HighlightOnVerify = Common.IsTruthy(Config.GetConfigValue("HighlightOnVerify", "False"));
                BrowserResolution = Config.GetConfigValue("BrowserResolution", "Default");
            }

            private List<string> GetHostsList()
            {
                var hosts = new List<string>();
                string host = Config.GetConfigValue("HostIp", "null");
                if (host != "null")
                {
                    hosts.Add(host);
                }
                for (int i = 1; i < 10; i++)
                {
                    host = Config.GetConfigValue("HostIp" + i, "null");
                    if (host != "null")
                        hosts.Add(host);
                }
                if (hosts.Count == 0)
                {
                    hosts.Add("localhost");
                }

                return hosts;
            }

            private List<WebDriverBrowser.Browser> GetBrowserList()
            {
                var browsers = new List<WebDriverBrowser.Browser>();
                string browser = Config.GetConfigValue("Browser", "null");
                if (browser != "null")
                {
                    browsers.Add(WebDriverBrowser.getBrowserFromString(browser));
                }
                for (int i = 1; i < 5; i++)
                {
                    browser = Config.GetConfigValue("Browser" + i, "null");
                    if (browser != "null")
                    {
                        browsers.Add(WebDriverBrowser.getBrowserFromString(browser));
                    }
                }
                if (browsers.Count == 0)
                {
                    browsers.Add(WebDriverBrowser.Browser.Chrome);
                }

                return browsers;
            }
        }

        /// <summary>
        /// Settings for TestStack.White module
        /// </summary>
        public class PurpleSettings
        {
            public string appPath;
            
            public bool launchApp;
            public string ProcessName;
            public string Purple_windowTitle;
            public string Purple_blankValue;
            public string Purple_Delimiter;
            public string Purple_ValueDelimiterStart;
            public string Purple_ValueDelimiterEnd;
            public int Purple_ElementTimeoutWaitSeconds;

            public PurpleSettings()
            {
                appPath = Config.GetConfigValue("AppPath", "NOT_SET");
                launchApp = Common.IsTruthy(Config.GetConfigValue("LaunchApp", "True"));
                ProcessName = Config.GetConfigValue("ProcessName", "NOT SET");
                Purple_windowTitle = Config.GetConfigValue("Purple_WindowTitle", "EMPTY");
                Purple_blankValue = Config.GetConfigValue("Purple_BlankValue", "!BLANK!");
                Purple_Delimiter = Config.GetConfigValue("Purple_Delimiter", "/");
                Purple_ValueDelimiterStart = Config.GetConfigValue("Purple_ValueDelimiterStart", "[");
                Purple_ValueDelimiterEnd = Config.GetConfigValue("Purple_ValueDelimiterEnd", "]");
                Purple_ElementTimeoutWaitSeconds = int.Parse(Config.GetConfigValue("Purple_ElementWaitTimeOutSeconds", "0"));

            }
        }
    }
}