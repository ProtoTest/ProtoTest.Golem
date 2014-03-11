using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;
using System.Windows.Forms;
using Gallio.Runner.Reports.Schema;

namespace ProtoTest.Golem.White.PurpleElements
{
    public class PurpleTextBox : PurpleElementBase
    {
        private string _textToEnter;

        public string Text
        {
            get { return GetText(); }
            set { EnterText(value); }
        }

        public PurpleTextBox(string name, string locatorPath) : base(name, locatorPath)
        {
        }

        private string GetText()
        {
            string textValue = "THERE IS NO TEXT";
            if (PurpleElement.Current.IsPassword)
            {
                WhiteTestBase.LogEvent(string.Format("Field is {0} is a Password field, cannot get value", ElementName));
                textValue = "PASSWORD FIELD CANNOT BE READ";
            }
            object basePattern;
            if (PurpleElement.TryGetCurrentPattern(ValuePattern.Pattern, out basePattern))
            {
                ValuePattern valuePattern = (BasePattern)basePattern as ValuePattern;
                if (valuePattern != null)
                {
                    textValue = valuePattern.Current.Value;
                }
                TextPattern textPattern = (BasePattern)basePattern as TextPattern;
                if (textPattern != null)
                {
                    textValue = textPattern.DocumentRange.GetText(int.MaxValue);
                }
            }
            return textValue;
        }

        private void EnterText(string val)
        {
            if (PurpleElement.Current.IsEnabled)
            {
                if (!PurpleElement.Current.IsOffscreen)
                {
                    PurpleElement.SetFocus();
                    SendKeys.SendWait(val);
                }
            }
        }

    }
}
