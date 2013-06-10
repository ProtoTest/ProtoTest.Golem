using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenQA.Selenium;
using System.Drawing;

namespace Golem.Framework
{
    public class VerificationError
    {
        public string errorText;
        public Image screenshot;

        public VerificationError(string errorText) 
        {
            this.errorText = errorText;
            this.screenshot = Golem.Framework.TestBaseClass.driver.GetScreenshot();
        }
        public VerificationError(string errorText, Image screenshot)
        {
            this.errorText = errorText;
            this.screenshot = screenshot;
        }
    }
}
