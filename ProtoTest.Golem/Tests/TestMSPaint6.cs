using NUnit.Framework;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.Purple;
using ProtoTest.Golem.Tests.PageObjects.MSPaint;

namespace ProtoTest.Golem.Tests
{
    public class TestMSPaint6 : PurpleTestBase
    {
        [OneTimeSetUp]
        public void setup()
        {
            Config.settings.purpleSettings.Purple_windowTitle = "Untitled - Paint";
            Config.settings.purpleSettings.ProcessName = "Paint";
            Config.settings.purpleSettings.Purple_blankValue = "!BLANK!";
            Config.settings.purpleSettings.Purple_Delimiter = "/";
            Config.settings.purpleSettings.appPath = "%windir%\\system32\\mspaint.exe";
            Config.settings.purpleSettings.Purple_ElementTimeoutWaitSeconds = 30;
        }

        [Test]
        public void PaintExample()
        {
            MSPaint_6.PaintWindow().PaintText("ProtoTest Purple");
        }
    }
}