using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoTest.Golem.WebDriver
{
    public class WebDriverHost
    {
        public WebDriverBrowser.Browser browser;
        public string version;
        public string platform;
        public string hostIp;

        public WebDriverHost(WebDriverBrowser.Browser browser, string hostIp, string version="", string platform="")
        {
            this.browser = browser;
            this.version = version;
            this.platform = platform;
            this.hostIp = hostIp;
        }
    }
}
