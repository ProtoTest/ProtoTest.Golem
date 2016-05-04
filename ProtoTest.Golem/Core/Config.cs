using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Core
{
    /// <summary>
    ///     The Config class holds instantiates the ConfigSettings class, and any Config-related functions
    /// </summary>
    public class Config
    {
        public static ConfigSettings Settings
        {
            get { return TestBase.testData.configSettings; }
            set { TestBase.testData.configSettings = value; }
        }

        /// <summary>
        ///     Returns the App.config value for requested key, or default value if not defined.
        /// </summary>
        /// <param name="key">Application configuration key</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns></returns>
        public static string GetConfigValue(string key, string defaultValue)
        {
            var setting = ConfigurationManager.AppSettings[key];
            if (setting == null)
            {
                return defaultValue;
            }

            return setting;
        }

        public static ConfigSettings GetDefaultConfig()
        {
            if (TestBase.testDataCollection.Count == 0)
            {
                return new ConfigSettings();
            }
            else
            {
                return TestBase.testDataCollection.FirstOrDefault(x=>x.Value.configSettings!=null).Value.configSettings;
            }
        }

        /// <summary>
        ///     Returns the App.config value for requested key, or default value if not defined, and tries to parse it for an byte.
        /// </summary>
        /// <param name="key">Application configuration key</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns></returns>
        public static byte GetConfigValueAsByte(string key, string defaultValue)
        {
            var setting = "";
            try
            {
                setting = GetConfigValue(key, defaultValue);
                return byte.Parse(setting);
            }
            catch (Exception e)
            {
                Log.Warning(string.Format(
                    "Exception Reading App.Config. Using key='{0}', got a result of : '{1}'.   Using 1 as default. {2}",
                    key, setting, e.Message));
                return 1;
            }
        }

        /// <summary>
        ///     Returns the App.config value for requested key, or default value if not defined, and tries to parse it for an int.
        /// </summary>
        /// <param name="key">Application configuration key</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns></returns>
        public static int GetConfigValueAsInt(string key, string defaultValue)
        {
            var setting = "";
            try
            {
                setting = GetConfigValue(key, defaultValue);
                return int.Parse(setting);
            }
            catch (Exception e)
            {
                Log.Warning(string.Format(
                    "Exception Reading App.Config. Using key='{0}', got a result of : '{1}'.   Using 1 as default. {2}",
                    key, setting, e.Message));
                return 1;
            }
        }

        /// <summary>
        ///     Returns the App.config value for requested key, or default value if not defined and returns a boolean.  Looks for
        ///     True, true, False, false.
        /// </summary>
        /// <param name="key">Application configuration key</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns></returns>
        public static bool GetConfigValueAsBool(string key, string defaultValue)
        {
            var setting = ConfigurationManager.AppSettings[key];
            if (setting == null)
            {
                setting = defaultValue;
            }
            return Common.IsTruthy(setting);
        }

        /// <summary>
        ///     Updates the App.config setting key with value
        /// </summary>
        /// <param name="key">Application configuration key</param>
        /// <param name="value">Application configuration key value to set</param>
        public static void UpdateConfigFile(string key, string value)
        {
            var doc = new XmlDocument();
            var path = Assembly.GetCallingAssembly().Location + ".config";
            doc.Load(path);
            doc.SelectSingleNode("//add[@key='" + key + "']").Attributes["value"].Value = value;
            doc.Save(path);

            path = AppDomain.CurrentDomain.BaseDirectory.Replace(@"\bin\Debug", "").Replace(@"\bin\Release", "") +
                   "\\App.config";
            doc.Load(path);
            doc.SelectSingleNode("//add[@key='" + key + "']").Attributes["value"].Value = value;
            doc.Save(path);
        }
    }

    /// <summary>
    ///     ConfigSettings holds each config section, and reads them in from the App.Config upon instantiation.  To override
    ///     these settings put the commands in a [FixtureInitializer]
    /// </summary>
    public class ConfigSettings
    {
        public BrowserStackSettings browserStackSettings;
        public HttpProxy httpProxy;
        public ImageCompareSettings imageCompareSettings;
        public PurpleSettings purpleSettings;
        public ReportSettings reportSettings;
        public RuntimeSettings runTimeSettings;
        public SauceLabsSettings sauceLabsSettings;

        public ConfigSettings()
        {
            runTimeSettings = new RuntimeSettings();
            reportSettings = new ReportSettings();
            httpProxy = new HttpProxy();
            imageCompareSettings = new ImageCompareSettings();
            purpleSettings = new PurpleSettings();
            browserStackSettings = new BrowserStackSettings();
            sauceLabsSettings = new SauceLabsSettings();
        }


        /// <summary>
        ///     Contains all settings related to the BrowserMobProxy
        /// </summary>
        public class HttpProxy
        {
            public bool killOldProxy;
            public int proxyPort;
            public int proxyServerPort;
            public string proxyUrl;
            public bool startProxy;
            public bool useProxy;
            public bool validateTraffic;

            public HttpProxy()
            {
                proxyServerPort = Config.GetConfigValueAsInt("ProxyServerPort", "18880");
                startProxy = Config.GetConfigValueAsBool("StartProxy", "False");
                useProxy = Config.GetConfigValueAsBool("UseProxy", "False");
                proxyPort = Config.GetConfigValueAsInt("ProxyPort", "18881");
                proxyUrl = Config.GetConfigValue("ProxyUrl", "localhost");
                validateTraffic = Config.GetConfigValueAsBool("ValidateTraffic", "False");
                killOldProxy = Config.GetConfigValueAsBool("KillOldProxy", "True");
            }
        }

        /// <summary>
        ///     Contains settings for image comparison.
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
        ///     Specify what should show up in the report
        /// </summary>
        public class ReportSettings
        {
            public string reportPath;
            public bool actionLogging;
            public bool commandLogging;
            public bool diagnosticLog;
            public bool htmlOnError;
            public bool screenshotOnError;
            public bool spellChecking;
            public bool testLog;
            public bool videoRecordingOnError;

            public ReportSettings()
            {
                htmlOnError = Config.GetConfigValueAsBool("HtmlOnError", "False");
                screenshotOnError = Config.GetConfigValueAsBool("ScreenshotOnError", "True");
                videoRecordingOnError = Config.GetConfigValueAsBool("VideoRecordingOnError", "True");
                commandLogging = Config.GetConfigValueAsBool("CommandLogging", "True");
                actionLogging = Config.GetConfigValueAsBool("ActionLogging", "True");
                spellChecking = Config.GetConfigValueAsBool("SpellChecking", "False");
                diagnosticLog = Config.GetConfigValueAsBool("DiagnosticLog", "True");
                testLog = Config.GetConfigValueAsBool("TestLog", "True");
                reportPath = Config.GetConfigValue("ReportPath", Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "reports"));
            }
        }

        /// <summary>
        ///     Specify execution settings
        /// </summary>
        public class RuntimeSettings
        {
            public bool AutoWaitForElements;
            public string BrowserResolution;
            public IEnumerable<BrowserInfo> Browsers = new List<BrowserInfo>();
            public int CommandDelayMs;
            public int DegreeOfParallelism;
            public int ElementTimeoutSec;
            public string EnvironmentUrl;
            public bool FindHiddenElements;
            public bool HighlightFoundElements;
            public string HostIp;
            public bool LaunchBrowser;
            public int OpenWindowTimeoutSec;
            public int PageTimeoutSec;
            public string RemoteHostPort;
            public bool RunOnRemoteHost;
            public int TestTimeoutMin;

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

            public WebDriverBrowser.Browser Browser
            {
                get { return TestBase.testData.browserInfo.browser; }
                set { TestBase.testData.browserInfo.browser = value; }
            }

            private List<BrowserInfo> GetBrowserInfo()
            {
                var hosts = new List<BrowserInfo>();
                var browser = Config.GetConfigValue("Browser", "null");
                var caps = Config.GetConfigValue("BrowserCapabilities", "null");
                if (browser != "null")
                {
                    hosts.Add(new BrowserInfo(WebDriverBrowser.getBrowserFromString(browser), caps));
                }
                for (var i = 1; i < 10; i++)
                {
                    browser = Config.GetConfigValue("Browser" + i, "null");
                    if (browser != "null")
                    {
                        caps = Config.GetConfigValue("Browser" + i + "Capabilities", "null");
                        hosts.Add(new BrowserInfo(WebDriverBrowser.getBrowserFromString(browser), caps));
                    }
                }
                if (hosts.Count == 0)
                {
                    hosts.Add(new BrowserInfo(WebDriverBrowser.Browser.Chrome));
                }
                return hosts;
            }


        }

        /// <summary>
        ///     Settings for TestStack.White module
        /// </summary>
        public class PurpleSettings
        {
            public string appPath;
            public string DataSetPath1;
            public string DataSetPath2;
            public string DataSetPath3;
            public string DataSetPath4;
            public bool launchApp;
            public string Machine1;
            public string Machine2;
            public string Machine3;
            public string Machine4;
            public string ProcessName;
            public string ProjectName1;
            public string ProjectName2;
            public string ProjectName3;
            public string ProjectName4;
            public string Purple_blankValue;
            public string Purple_Delimiter;
            public int Purple_ElementTimeoutWaitSeconds;
            public string Purple_ValueDelimiterEnd;
            public string Purple_ValueDelimiterStart;
            public string Purple_windowTitle;

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
            public string BrowserStack_Key;
            public string BrowserStack_OS;
            public string BrowserStack_OS_Version;
            public string BrowserStack_User;
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
            public string SauceLabsAPIKey;
            public string SauceLabsUrl;
            public string SauceLabsUsername;
            public string ScreenResolution;
            public bool UseSauceLabs;

            public SauceLabsSettings()
            {
                UseSauceLabs = Config.GetConfigValueAsBool("UseSauceLabs", "False");
                SauceLabsUrl = Config.GetConfigValue("SauceLabsUrl", "http://ondemand.saucelabs.com/wd/hub");
                SauceLabsUsername = Config.GetConfigValue("SauceLabsUsername", "NOT_SET");
                SauceLabsAPIKey = Config.GetConfigValue("SauceLabsAPIKey", "NOT_SET");
                ScreenResolution = Config.GetConfigValue("ScreenResolution", "1280x1024");
            }
        }
    }
}