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
        public BrowserStackSettings browserStackSettings;
        public SauceLabsSettings sauceLabsSettings;

        public ConfigSettings()
        {
            runTimeSettings = new RuntimeSettings();
            reportSettings = new ReportSettings();
            httpProxy = new HttpProxy();
            appiumSettings = new AppiumSettings();
            imageCompareSettings = new ImageCompareSettings();
            purpleSettings = new PurpleSettings();
            browserStackSettings = new BrowserStackSettings();
            sauceLabsSettings = new SauceLabsSettings();
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
            public string appiumVersion;
            public string platformVersion;

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
                appiumVersion = Config.GetConfigValue("AppiumVersion", "1.2");
                platformVersion = Config.GetConfigValue("PlatformVersion", "4.3");
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
            public bool validateTraffic;

            public HttpProxy()
            {
                proxyServerPort = Config.GetConfigValueAsInt("ProxyServerPort", "18880");
                startProxy = Config.GetConfigValueAsBool("StartProxy", "False");
                useProxy = Config.GetConfigValueAsBool("UseProxy", "False");
                proxyPort = Config.GetConfigValueAsInt("ProxyPort", "18881");
                validateTraffic = Config.GetConfigValueAsBool("ValidateTraffic", "False");
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
            public bool diagnosticLog;
            public bool testLog;
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
                diagnosticLog = Config.GetConfigValueAsBool("DiagnosticLog", "True");
                testLog = Config.GetConfigValueAsBool("TestLog", "True");
            }        
        }

        /// <summary>
        /// Specify execution settings
        /// </summary>
        public class RuntimeSettings
        {
            public string BrowserResolution;
            public List<BrowserInfo> Browsers = new List<BrowserInfo>();
            public int CommandDelayMs;
            public int DegreeOfParallelism;
            public int ElementTimeoutSec;
            public int OpenWindowTimeoutSec;
            public string EnvironmentUrl;
            public bool HighlightFoundElements;
            public bool LaunchBrowser;
            public int PageTimeoutSec;
            public bool RunOnRemoteHost;
            public string HostIp;
            public string RemoteHostPort;
            public int TestTimeoutMin;
            public bool AutoWaitForElements;
            public bool FindHiddenElements;

            public string Version
            {
                get { return WebDriverTestBase.browserInfo.version; }
                set { WebDriverTestBase.browserInfo.version = value; }
            }

            public string Platform
            {
                get { return WebDriverTestBase.browserInfo.platform; }
                set { WebDriverTestBase.browserInfo.platform = value; }
            }

            public WebDriverBrowser.Browser Browser
            {
                get { return WebDriverTestBase.browserInfo.browser; }
                set { WebDriverTestBase.browserInfo.browser = value; }
            }

            public RuntimeSettings()
            {
                Browsers = GetBrowserInfo();
                LaunchBrowser = Config.GetConfigValueAsBool("LaunchBrowser", "True");
                TestTimeoutMin = Config.GetConfigValueAsInt("TestTimeoutMin", "5");
                ElementTimeoutSec = Config.GetConfigValueAsInt("ElementTimeoutSec", "5");
                OpenWindowTimeoutSec = Config.GetConfigValueAsInt("WindowOpenTimeoutSec", "10");
                PageTimeoutSec = Config.GetConfigValueAsInt("PageTimeoutSec", "30");
                EnvironmentUrl = Config.GetConfigValue("EnvironmentUrl", "");
                DegreeOfParallelism = Config.GetConfigValueAsInt("DegreeOfParallelism", "5");
                CommandDelayMs = Config.GetConfigValueAsInt("CommandDelayMs", "0");
                RunOnRemoteHost = Config.GetConfigValueAsBool("RunOnRemoteHost", "False");
                RemoteHostPort = Config.GetConfigValue("RemoteHostPort", "8080");
                HostIp = Config.GetConfigValue("HostIp", "localhost");
                AutoWaitForElements = Config.GetConfigValueAsBool("AutoWaitForElements", "True");
                HighlightFoundElements = Config.GetConfigValueAsBool("HighlightFoundElements", "True");
                BrowserResolution = Config.GetConfigValue("BrowserResolution", "Default");
                FindHiddenElements = Config.GetConfigValueAsBool("FindHiddenElements", "True");

            }

            private List<BrowserInfo> GetBrowserInfo()
            {
                var hosts = new List<BrowserInfo>();
                string browser = Config.GetConfigValue("Browser", "null");
                string version = Config.GetConfigValue("Version", "");
                string platform = Config.GetConfigValue("Platform", "");
                if (browser != "null")
                {
                    hosts.Add(new BrowserInfo(WebDriverBrowser.getBrowserFromString(browser), version, platform));
                }
                for (int i = 1; i < 10; i++)
                {
                    browser = Config.GetConfigValue("Browser" + i, "null");
                    version = Config.GetConfigValue("Version" + i, "");
                    platform = Config.GetConfigValue("Platform" + i, "");
                    if (browser != "null")
                    {
                        hosts.Add(new BrowserInfo(WebDriverBrowser.getBrowserFromString(browser), version, platform));
                    }
                }
                return hosts;
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
            public string ProjectName1;
            public string ProjectName2;
            public string ProjectName3;
            public string ProjectName4;

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
                ProjectName1 = Config.GetConfigValue("PrjName1", "NOT SET");
                ProjectName2 = Config.GetConfigValue("PrjName2", "NOT SET");
                ProjectName3 = Config.GetConfigValue("PrjName3", "NOT SET");
                ProjectName4 = Config.GetConfigValue("PrjName4", "NOT SET");
            }
        }

        public class BrowserStackSettings
        {
            public string BrowserStack_User;
            public string BrowserStack_Key;
            public string BrowserStack_OS;
            public string BrowserStack_OS_Version;
            public string BrowserStackRemoteURL;

            public BrowserStackSettings()
            {
                BrowserStack_User = Config.GetConfigValue("BrowserStack_User", null);
                BrowserStack_Key = Config.GetConfigValue("BrowserStack_Key", null);
                BrowserStack_OS = Config.GetConfigValue("BrowserStack_OS", null);
                BrowserStack_OS_Version = Config.GetConfigValue("BrowserStack_OS_Version", null);
                BrowserStackRemoteURL = Config.GetConfigValue("BrowserStackRemoteURL", "hub.browserstack.com");
            }
        }

        public class SauceLabsSettings
        {
            public bool UseSauceLabs;
            public string SauceLabsUsername;
            public string SauceLabsAPIKey;
            public string SauceLabsUrl;

            public SauceLabsSettings()
            {

                UseSauceLabs = Config.GetConfigValueAsBool("UseSauceLabs", "False");
                SauceLabsUrl = Config.GetConfigValue("SauceLabsUrl", "http://ondemand.saucelabs.com/wd/hub");
                SauceLabsUsername = Config.GetConfigValue("SauceLabsUsername", "NOT_SET");
                SauceLabsAPIKey = Config.GetConfigValue("SauceLabsAPIKey", "NOT_SET");
            }
        }
    }
}