using System.Windows.Automation;
using System.Windows.Forms;
using Golem.Core;

namespace Golem.Purple.PurpleElements
{
    public class PurpleTextBox : PurpleElementBase
    {
        private string _textToEnter;

        public PurpleTextBox(string name, string locatorPath) : base(name, locatorPath)
        {
        }

        public PurpleTextBox(string name, AutomationElement element) : base(name, element)
        {
        }

        public string Text
        {
            get
            {
                var enteredText = "ELEMENT NOT FOUND";
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

        private string GetText()
        {
            var textValue = "THERE IS NO TEXT";
            if (_UIAElement.Current.IsPassword)
            {
                Log.Message(string.Format("Field is {0} is a Password field, cannot get value", ElementName));
                textValue = "PASSWORD FIELD CANNOT BE READ";
            }
            object basePattern;
            if (_UIAElement.TryGetCurrentPattern(ValuePattern.Pattern, out basePattern))
            {
                var valuePattern = (BasePattern) basePattern as ValuePattern;
                if (valuePattern != null)
                {
                    textValue = valuePattern.Current.Value;
                }
                var textPattern = (BasePattern) basePattern as TextPattern;
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