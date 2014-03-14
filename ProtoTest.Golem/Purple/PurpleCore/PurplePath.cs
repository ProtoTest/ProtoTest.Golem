using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gallio.Runner.Reports.Schema;
using ProtoTest.Golem.Core;

namespace ProtoTest.Golem.Purple.PurpleCore
{
    public static class PurplePath
    {
        private static PurpleLib.PurplePath _purplePath = new PurpleLib.PurplePath();

        public static PurpleLib.PurplePath Locator {get { return _purplePath; }}

        static PurplePath()
        {
            _purplePath.Delimiter = Config.Settings.whiteSettings.Purple_Delimiter;
            _purplePath.BlankValue = Config.Settings.whiteSettings.Purple_blankValue;
            _purplePath.DefaultWindowName = Config.Settings.whiteSettings.Purple_windowTitle;
            _purplePath.ValueDelimiterStart = Config.Settings.whiteSettings.Purple_ValueDelimiterStart;
            _purplePath.ValueDelimiterEnd = Config.Settings.whiteSettings.Purple_ValueDelimiterEnd;
        }


    }
}
