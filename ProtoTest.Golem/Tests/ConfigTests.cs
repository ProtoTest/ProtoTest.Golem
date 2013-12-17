using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MbUnit.Framework;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Tests
{
    public class ConfigTests : TestBase
    {
        [Test]
        public void TestCustomConfigSettings()
        {
            string value = Config.GetConfigValue("CustomSetting", "default");
            Assert.AreEqual(value,"WasFound");
        }

        [Test]
        public void TestConfigSettingsAreModifiyable()
        {
            var timeout = Config.Settings.runTimeSettings.ElementTimeoutSec;
            Config.Settings.runTimeSettings.ElementTimeoutSec = 1234;
            Assert.AreEqual(Config.Settings.runTimeSettings.ElementTimeoutSec,1234);
            Config.Settings.runTimeSettings.ElementTimeoutSec = timeout;
        }
        [Test]
        public void TestConfigFileDefaults()
        {
            var value = Config.GetConfigValue("Testsdflksjdflsdkj", "-1");
            Assert.AreEqual(value,"-1");
        }

    }
}
