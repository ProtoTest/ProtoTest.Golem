using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Xml;
//using Castle.Core.Logging;
using Gallio.Framework;
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
        /// Returns the App.config value for requested key, or default value if not defined, and tries to parse it for an byte.  
        /// </summary>
        /// <param name="key">Application configuration key</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns></returns>
        public static byte GetConfigValueAsByte(string key, string defaultValue)
        {
            string setting = "";
            try
            {
                setting = GetConfigValue(key, defaultValue);
                return byte.Parse(setting);
            }
            catch (Exception e)
            {
                TestLog.Warnings.WriteLine(
                    string.Format(
                        "Exception Reading App.Config. Using key='{0}', got a result of : '{1}'.   Using 1 as default. {2}",
                        key, setting, e.Message));
                return 1;
            }

        }


        /// <summary>
        /// Returns the App.config value for requested key, or default value if not defined, and tries to parse it for an int.  
        /// </summary>
        /// <param name="key">Application configuration key</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns></returns>
        public static int GetConfigValueAsInt(string key, string defaultValue)
        {
            string setting = "";
            try
            {
                setting = GetConfigValue(key, defaultValue);
                return int.Parse(setting);
            }
            catch (Exception e)
            {
                TestLog.Warnings.WriteLine(
                    string.Format(
                        "Exception Reading App.Config. Using key='{0}', got a result of : '{1}'.   Using 1 as default. {2}",
                        key, setting,e.Message));
                return 1;
            }
            
        }
        /// <summary>
        /// Returns the App.config value for requested key, or default value if not defined and returns a boolean.  Looks for True, true, False, false.  
        /// </summary>
        /// <param name="key">Application configuration key</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns></returns>
        public static bool GetConfigValueAsBool(string key, string defaultValue)
        {
            string setting = ConfigurationManager.AppSettings[key];
            if (setting == null)
            {
                setting = defaultValue;
            }
            return Common.IsTruthy(setting);
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
                launchApp = Config.GetConfigValueAsBool("LaunchApp", "False");
                appPath = Config.GetConfigValue("AppPath", "");
                package = Config.GetConfigValue("AppPackage", "");
                activity = Config.GetConfigValue("AppActivity", "");
                device = Config.GetConfigValue("Device", "");
                appiumPort = Config.GetConfigValue("AppiumPort", "4723");
                useIpa = Config.GetConfigValueAsBool("UseIpa", "False");
                appiumServerPath = Config.GetConfigValue("AppiumServerPath", "");
                resetApp = Config.GetConfigValueAsBool("ResetApp", "False");
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
                proxyServerPort = Config.GetConfigValueAsInt("ProxyServerPort", "18880");
                startProxy = Config.GetConfigValueAsBool("StartProxy", "False");
                useProxy = Config.GetConfigValueAsBool("UseProxy", "False");
                proxyPort = Config.GetConfigValueAsInt("ProxyPort", "18881");
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
                fuzziness = Config.GetConfigValueAsByte("Fuzziness", "50");
                accuracy = float.Parse(Config.GetConfigValue("Accuracy", "1"))/100;
                updateImages = Config.GetConfigValueAsBool("UpdateImages", "false");
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
                htmlOnError = Config.GetConfigValueAsBool("HtmlOnError", "True");
                screenshotOnError = Config.GetConfigValueAsBool("ScreenshotOnError", "True");
                videoRecordingOnError = Config.GetConfigValueAsBool("VideoRecordingOnError", "True");
                commandLogging = Config.GetConfigValueAsBool("CommandLogging", "True");
                actionLogging = Config.GetConfigValueAsBool("ActionLogging", "True");
                spellChecking = Config.GetConfigValueAsBool("SpellChecking", "False");
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
            public bool HighlightFoundElements;
            public List<string> Hosts;
            public bool LaunchBrowser;
            public int PageTimeoutSec;
            public bool RunOnRemoteHost;
            public int TestTimeoutMin;
            public bool AutoWaitForElements;

            public RuntimeSettings()
            {
                Browsers = GetBrowserList();
                Hosts = GetHostsList();
                LaunchBrowser = Config.GetConfigValueAsBool("LaunchBrowser", "True");
                TestTimeoutMin = Config.GetConfigValueAsInt("TestTimeoutMin", "5");
                ElementTimeoutSec = Config.GetConfigValueAsInt("ElementTimeoutSec", "5");
                OpenWindowTimeoutSec = Config.GetConfigValueAsInt("WindowOpenTimeoutSec", "10");
                PageTimeoutSec = Config.GetConfigValueAsInt("PageTimeoutSec", "30");
                EnvironmentUrl = Config.GetConfigValue("EnvironmentUrl", "");
                DegreeOfParallelism = Config.GetConfigValueAsInt("DegreeOfParallelism", "5");
                CommandDelayMs = Config.GetConfigValueAsInt("CommandDelayMs", "0");
                RunOnRemoteHost = Config.GetConfigValueAsBool("RunOnRemoteHost", "False");
                AutoWaitForElements = Config.GetConfigValueAsBool("AutoWaitForElements", "True");
                HighlightFoundElements = Config.GetConfigValueAsBool("HighlightFoundElements", "True");
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
            public string Machine1;
            public string Machine2;
            public string Machine3;
            public string Machine4;
            public string DataSetPath1;
            public string DataSetPath2;
            public string DataSetPath3;
            public string DataSetPath4;

            public PurpleSettings()
            {
                appPath = Config.GetConfigValue("AppPath", "NOT_SET");
                launchApp = Config.GetConfigValueAsBool("LaunchApp", "True");
                ProcessName = Config.GetConfigValue("ProcessName", "NOT SET");
                Purple_windowTitle = Config.GetConfigValue("Purple_WindowTitle", "EMPTY");
                Purple_blankValue = Config.GetConfigValue("Purple_BlankValue", "!BLANK!");
                Purple_Delimiter = Config.GetConfigValue("Purple_Delimiter", "/");
                Purple_ValueDelimiterStart = Config.GetConfigValue("Purple_ValueDelimiterStart", "[");
                Purple_ValueDelimiterEnd = Config.GetConfigValue("Purple_ValueDelimiterEnd", "]");
                Purple_ElementTimeoutWaitSeconds = Config.GetConfigValueAsInt("Purple_ElementWaitTimeOutSeconds", "0");
                Machine1 = Config.GetConfigValue("Machine1", "NOT_SET");
                Machine2 = Config.GetConfigValue("Machine2", "NOT_SET");
                Machine3 = Config.GetConfigValue("Machine3", "NOT_SET");
                Machine4 = Config.GetConfigValue("Machine4", "NOT_SET");
                DataSetPath1 = Config.GetConfigValue("DS1", "NOT_SET");
                DataSetPath2 = Config.GetConfigValue("DS2", "NOT_SET");
                DataSetPath3 = Config.GetConfigValue("DS3", "NOT_SET");
                DataSetPath4 = Config.GetConfigValue("DS4", "NOT_SET");
            }
        }
    }
}