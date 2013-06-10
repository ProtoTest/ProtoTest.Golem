using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Golem.Framework
{
    public static class Config
    {
        public class RuntimeSettings
        {
            public static WebDriverBrowser.Browser browser = WebDriverBrowser.Browser.Firefox;
            public static int elementTimeout = 10;
            public static int pageTimeout = 30;
            public static int implicitTimeout = 0;
            public static string environmentUrl = "http://www.google.com/";
        }
        public class ReportSettings
        {
            public bool screenshotOnError = true;
            public bool videoRecordingOnError = true;
            public bool commandLogging = true;
            public bool pageObjectActionLogging = true;
        }
        public class HttpProxy
        {
            public bool startFiddlerProxy = true;
        }

    }
}
