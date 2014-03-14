using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Castle.Components.DictionaryAdapter.Xml;
using Castle.Core.Logging;
using Gallio.Framework;
using Gallio.Model;
using MbUnit.Framework;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.Purple.PurpleElements;
using ProtoTest.Golem.WebDriver;
using ProtoTest.Golem.Purple.Elements;
using RestSharp.Extensions;


namespace ProtoTest.Golem.Purple
{
    public class PurpleTestBase : TestBase
    {
        
        [FixtureInitializer]
        public void WhiteSettings()
        {
            //CoreAppXmlConfiguration.Instance.MaxElementSearchDepth = 4;
            // CoreAppXmlConfiguration.Instance.LoggerFactory.Create("WhiteDefaultLogger",LoggerLevel.Info);
        }

        public static void WaitUntilReady()
        {
            Process[] processes = Process.GetProcessesByName(Config.Settings.whiteSettings.ProcessName);
            if (processes.Length == 0)
            {
                Common.Log(string.Format("Can not find process {0} ", Config.Settings.whiteSettings.ProcessName));
            }
            for (int x = 0; x < processes.Count(); x++)
            {
                if (processes[x].WaitForInputIdle(30000))
                {
                    Thread.Sleep(500);
                }
                
            }
        }


        [SetUp]
        public void SetUp()
        {
            var startInfo = new ProcessStartInfo(Config.Settings.whiteSettings.appPath);
            if (Config.Settings.whiteSettings.launchApp)
            {
                //app = Application.Launch(startInfo);
                Process.Start(startInfo);
                //Wait for the application to start -- make this configurable.  White doesn't seem to care
                LogEvent(string.Format("Waiting {0} seconds for {1} to start up...", Config.Settings.whiteSettings.appStartupTime, Config.Settings.whiteSettings.appPath));
                Thread.Sleep(Config.Settings.whiteSettings.appStartupTime * 1000);
                //WaitUntilReady();
            }
            else
            {            
                //app = Application.AttachOrLaunch(startInfo);
                //WaitUntilReady();
            }
            
            
        }

        [TearDown]
        public void TearDown()
        {
            
            //TestLog.WriteLine(CoreAppXmlConfiguration.Instance.WorkSessionLocation.ToString());
            LogScreenshotIfTestFailed();
            //app.Close();
            //app.ApplicationSession.Save();
        }

        public void LogScreenshotIfTestFailed()
        {
            if(TestContext.CurrentContext.Outcome!=TestOutcome.Passed)
                TestLog.EmbedImage(null, PurpleWindow.purpleWindow.GetImage());
        }
    }
}