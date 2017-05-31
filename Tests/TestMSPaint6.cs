using NUnit.Framework;
using Golem.Core;
using Golem.Purple;
using Golem.Tests.PageObjects.MSPaint;

namespace Golem.Tests
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