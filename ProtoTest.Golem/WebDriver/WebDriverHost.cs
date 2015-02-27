namespace ProtoTest.Golem.WebDriver
{
    public class BrowserInfo
    {
        public WebDriverBrowser.Browser browser;
        public string platform;
        public string version;

        public BrowserInfo(WebDriverBrowser.Browser browser, string version = "", string platform = "")
        {
            this.browser = browser;
            this.version = version;
            this.platform = platform;
        }

        public BrowserInfo()
        {
        }
    }
}