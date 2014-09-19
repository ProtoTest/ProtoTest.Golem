using System.Windows.Automation;
using System.Windows.Forms;

namespace ProtoTest.Golem.Purple.PurpleElements
{
    public class PurpleTextBox : PurpleElementBase
    {
        private string _textToEnter;

        public string Text
        {
            get
            {
                string enteredText = "ELEMENT NOT FOUND";
                if (PurpleElement != null)
                {
                    enteredText = GetText();
                }
                return enteredText;
            }
            set
            {
                if (PurpleElement.Current.IsEnabled)
                {
                    EnterText(value);
                }
            }
        }

        public PurpleTextBox(string name, string locatorPath) : base(name, locatorPath)
        {
        }

        private string GetText()
        {
            string textValue = "THERE IS NO TEXT";
            if (_UIAElement.Current.IsPassword)
            {
                PurpleTestBase.LogEvent(string.Format("Field is {0} is a Password field, cannot get value", ElementName));
                textValue = "PASSWORD FIELD CANNOT BE READ";
            }
            object basePattern;
            if (_UIAElement.TryGetCurrentPattern(ValuePattern.Pattern, out basePattern))
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
            if (!_UIAElement.Current.IsOffscreen)
            {
                _UIAElement.SetFocus();
                SendKeys.SendWait(val);
            }
            
        }

    }
}
