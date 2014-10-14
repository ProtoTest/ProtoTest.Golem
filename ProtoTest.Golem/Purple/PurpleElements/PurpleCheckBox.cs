using System;
using System.Windows.Automation;

namespace ProtoTest.Golem.Purple.PurpleElements
{
    public class PurpleCheckBox : PurpleElementBase
    {
        //TODO: Finish Implimenting PurpleCheckBox.
        public bool Checked
        {
            get { return IsElementToggledOn(); } 
            set { Check(value);}
        }

        public PurpleCheckBox(string name, string locatorPath) : base(name, locatorPath)
        {
        }

        public bool IsElementToggledOn()
        {
            Object objPattern;
            TogglePattern togPattern;
            if (true == this.PurpleElement.TryGetCurrentPattern(TogglePattern.Pattern, out objPattern))
            {
                togPattern = objPattern as TogglePattern;
                return togPattern.Current.ToggleState == ToggleState.On;
            }
            return false;
        }

        public void Check(bool value)
        {
            if (IsElementToggledOn())
            {
                if (!value)
                {
                    Click();
                }
            }
            else
            {
                if (value)
                {
                    Click();
                }
            }
        }
    }
}
