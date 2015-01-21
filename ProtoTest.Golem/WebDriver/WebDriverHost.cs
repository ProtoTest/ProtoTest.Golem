using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoTest.Golem.WebDriver
{
    public class BrowserInfo
    {
        public WebDriverBrowser.Browser browser;
        public string version;
        public string platform;

        public BrowserInfo(WebDriverBrowser.Browser browser, string version = "", string platform = "")
        {
            this.browser = browser;
            this.version = version;
            this.platform = platform;
        }

    }
}
