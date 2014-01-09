<<<<<<< HEAD
﻿using Castle.Core.Logging;
=======
﻿using System.Threading;
>>>>>>> Updating white for fixes
using Gallio.Framework;
using MbUnit.Framework;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.White.Elements;
using TestStack.White;
using TestStack.White.Configuration;
using TestStack.White.Factory;
using TestStack.White.UIItems.WindowItems;

namespace ProtoTest.Golem.White
{
    public class WhiteTestBase : TestBase
    {
        public static Application app { get; set; }
        public static WhiteWindow _window;

        public static WhiteWindow window
        {
            get
            {
                return _window;
            }
            set
            {
                _window = value;
            }
        }

        [FixtureInitializer]
        public void WhiteSettings()
        {
<<<<<<< HEAD
            //CoreAppXmlConfiguration.Instance.RawElementBasedSearch = false;
            //CoreAppXmlConfiguration.Instance.MaxElementSearchDepth = 4;
            CoreAppXmlConfiguration.Instance.LoggerFactory = new WhiteDefaultLoggerFactory(LoggerLevel.Debug);
=======
          //  CoreAppXmlConfiguration.Instance.RawElementBasedSearch = false;
            //CoreAppXmlConfiguration.Instance.MaxElementSearchDepth = 4;
>>>>>>> Updating white for fixes
        }


        [SetUp]
        public void LaunchApp()
        {
            
            app = Application.Launch(Config.Settings.whiteSettings.appPath);
<<<<<<< HEAD
            app.ApplicationSession.WindowSession(InitializeOption.NoCache.AndIdentifiedBy("MainWindowX"));
=======
            while (app.GetWindows().Count == 0)
            {
                Thread.Sleep(200);
            }
        //    app.ApplicationSession.WindowSession(InitializeOption.NoCache.AndIdentifiedBy("MainWindowX"));
            

>>>>>>> Updating white for fixes
        }

        [TearDown]
        public void CloseApplication()
        {
            TestLog.EmbedImage(null, app.GetImage());
<<<<<<< HEAD
            TestLog.WriteLine(CoreAppXmlConfiguration.Instance.WorkSessionLocation.ToString());
=======
          //  TestLog.WriteLine(CoreAppXmlConfiguration.Instance.WorkSessionLocation.ToString());
>>>>>>> Updating white for fixes
            app.Close();
          //  app.ApplicationSession.Save();
        }
    }
}