using System.Runtime.CompilerServices;
using System.Windows.Input;
using OpenQA.Selenium.Remote;
using ProtoTest.Golem.Core;

namespace ProtoTest.Golem.WebDriver
{
    public class BrowserInfo
    {
        public WebDriverBrowser.Browser browser;
        public DesiredCapabilities capabilities = new DesiredCapabilities();

        public BrowserInfo(WebDriverBrowser.Browser browser, string capabilities)
        {
            this.browser = browser;
            this.capabilities = GetCapsFromString(capabilities);
        }

        public BrowserInfo(WebDriverBrowser.Browser browser)
        {
            this.browser = browser;
            this.capabilities = new WebDriverBrowser().GetCapabilitiesForBrowser(browser);
        }

        public DesiredCapabilities GetCapsFromString(string caps) {
            DesiredCapabilities endCaps = new WebDriverBrowser().GetCapabilitiesForBrowser(browser);
            if(caps=="null") return endCaps;
            string[] capabilities = caps.Split(',');
            foreach (var cap in capabilities)
            {
                string[] toks = cap.Split('=');
                string key = toks[0];
                string value = toks[1];
                endCaps.SetCapability(key,value);
            }
            return endCaps;

        }
    }
}