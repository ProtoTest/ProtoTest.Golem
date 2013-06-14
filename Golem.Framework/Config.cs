using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;

namespace Golem.Framework
{
    public class Config
    {
        public static ConfigSettings _settings;
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
            else
                return setting;
        }


    }

    public class ConfigSettings
    {
        public RuntimeSettings runTimeSettings;
        public ReportSettings reportSettings;
        public HttpProxy httpProxy;

        public ConfigSettings()
        {
            runTimeSettings = new RuntimeSettings();
            reportSettings = new ReportSettings();
            httpProxy = new HttpProxy();
        }

        public class RuntimeSettings
        {
            public WebDriverBrowser.Browser browser;
            public bool launchBrowser;
            public int testTimeoutMin;
            public int elementTimeoutSec;
            public int pageTimeoutSec;
            public string environmentUrl;
            public int degreeOfParallelism;
            public int commandDelayMs;

            public RuntimeSettings()
            {
            browser = WebDriverBrowser.getBrowserFromString(Config.GetConfigValue("Browser","Firefox"));
            launchBrowser = Common.IsTruthy(Config.GetConfigValue("LaunchBrowser", "True"));
            testTimeoutMin = int.Parse(Config.GetConfigValue("TestTimeoutMin","5"));
            elementTimeoutSec =int.Parse(Config.GetConfigValue("ElementTimeoutSec","20"));
            pageTimeoutSec = int.Parse(Config.GetConfigValue("PageTimeoutSec","30"));
            environmentUrl = Config.GetConfigValue("EnvironmentUrl","http://www.google.com/");
            degreeOfParallelism = int.Parse(Config.GetConfigValue("NumberOfParallelTests","20"));
            commandDelayMs = int.Parse(Config.GetConfigValue("CommandDelayMs", "0"));
            }
        }
        public class ReportSettings
        {

            public bool htmlOnError;
            public bool screenshotOnError;
            public bool videoRecordingOnError;
            public bool commandLogging;
            public bool actionLogging;

            public ReportSettings()
            {
                htmlOnError = Common.IsTruthy(Config.GetConfigValue("HtmlOnError", "True"));
                screenshotOnError = Common.IsTruthy(Config.GetConfigValue("ScreenshotOnError","True"));
                videoRecordingOnError = Common.IsTruthy(Config.GetConfigValue("VideoRecordingOnError", "True"));
                commandLogging = Common.IsTruthy(Config.GetConfigValue("CommandLogging","True"));
                actionLogging= Common.IsTruthy(Config.GetConfigValue("ActionLogging","True"));

            }
            
        }
        public class HttpProxy
        {
            public HttpProxy()
            {
                startProxy = Common.IsTruthy(Config.GetConfigValue("StartFiddlerProxy","True"));
            
            }
            public bool startProxy;
        }
      
    }
}
