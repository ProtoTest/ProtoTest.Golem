using NUnit.Framework;
using ProtoTest.Golem.Core;

namespace ProtoTest.Golem.Tests
{
    public class ConfigTests : TestBase
    {
        [SetUp]
        public void init()
        {
            Config.settings.runTimeSettings.ElementTimeoutSec = 27;
        }
        [Test]
        public void TestCustomConfigSettings()
        {
            var value = Config.GetConfigValue("CustomSetting", "default");
            Assert.AreEqual(value, "WasFound");
        }

        [Test]
        public void TestConfigSettingsAreModifiyable()
        {
            var timeout = Config.settings.runTimeSettings.ElementTimeoutSec;
            Config.settings.runTimeSettings.ElementTimeoutSec = 1234;
            Assert.AreEqual(Config.settings.runTimeSettings.ElementTimeoutSec, 1234);
            Config.settings.runTimeSettings.ElementTimeoutSec = timeout;
        }

        [Test]
        public void TestConfigFileDefaults()
        {
            var value = Config.GetConfigValue("Testsdflksjdflsdkj", "-1");
            Assert.AreEqual(value, "-1");
        }

        [Test]
        public void TestInit()
        {
            Assert.AreEqual(Config.settings.runTimeSettings.ElementTimeoutSec, 27);
        }
    }
}