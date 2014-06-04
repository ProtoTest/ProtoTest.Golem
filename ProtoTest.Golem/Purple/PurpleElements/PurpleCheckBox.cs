using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Automation;

namespace ProtoTest.Golem.Purple.PurpleElements
{
    public class PurpleCheckBox : PurpleElementBase
    {
        //TODO: Finish Implimenting PurpleCheckBox.
        public bool Checked { get; set; }

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

        public void Check()
        {
            if (IsElementToggledOn())
            {
                
            }
            else
            {
                
            }
        }
    }
}
