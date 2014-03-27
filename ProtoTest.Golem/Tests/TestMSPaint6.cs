using MbUnit.Framework;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.Purple;
using ProtoTest.Golem.Tests.PageObjects.MSPaint;

namespace ProtoTest.Golem.Tests
{
    public class TestMSPaint6 : PurpleTestBase
    {
        [FixtureInitializer]
        public void setup()
        {
            Config.Settings.purpleSettings.Purple_windowTitle = "Untitled - Paint";
            Config.Settings.purpleSettings.ProcessName = "Paint";
            Config.Settings.purpleSettings.Purple_blankValue = "!BLANK!";
            Config.Settings.purpleSettings.Purple_Delimiter = "/";
            Config.Settings.purpleSettings.appPath = "%windir%\\system32\\mspaint.exe";
            Config.Settings.purpleSettings.Purple_ElementTimeoutWaitSeconds = 30;
        }

        [Test]
        public void PaintExample()
        {
            MSPaint_6.PaintWindow().PaintText("ProtoTest Purple");
        }
    }
}
