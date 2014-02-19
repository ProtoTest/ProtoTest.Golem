using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;
using MbUnit.Framework;

namespace GoogleTests
{
    class AppiumTest : WebDriverTestBase
    {
        [FixtureInitializer]
        public void setup()
        {
            Config.Settings.runTimeSettings.LaunchBrowser = false;
            Config.Settings.appiumSettings.launchApp = true;
            Config.Settings.appiumSettings.package = "com.wegmans.wegmansapp";
            Config.Settings.appiumSettings.activity = "com.wegmans.android.wegmans.common.activities.StartupActivity";
            Config.Settings.appiumSettings.device = "Android";
            Config.Settings.appiumSettings.appPath = @"C:\Users\Brian\Documents\Wegmans\Wegmans.apk";
        }
        [Test]
        public void Test()
        {
            

        }
    }
}