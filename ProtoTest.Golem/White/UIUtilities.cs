using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestStack.White.UIItems;

namespace ProtoTest.Golem.White
{
    public class UIUtilities
    {
        //This class will be used for utilities funtions that all White elements and components can use
        public UIUtilities()
        {
            
        }

        public void checkboxer(bool state, CheckBox checkBox)
        {
            //Checks the state of the checkbox, and makes it match the bool value that was passed in
            if (state)
            {
                if (!checkBox.Checked)
                {
                    checkBox.Click();
                }
            }
            else
            {
                if (checkBox.Checked)
                {
                    checkBox.Click();
                }
            }
        }

    }
}
