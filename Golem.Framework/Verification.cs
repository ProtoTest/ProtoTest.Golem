using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Gallio.Framework;
using MbUnit.Framework;
using OpenQA.Selenium;

namespace Golem.Framework
{
    public class Verification
    {
        private Element element;
        private int timeoutSec;
        private bool failTest;
        private bool isNot = false;
        private string message;
        private string notMessage;
        private bool condition;
        private const string errorMessage = "{0}: {1}({2}): {3} after {4} seconds";

        public Verification Not
        {
            get
            {
                return new Verification(this.element,this.timeoutSec,this.failTest,true);
            }
        }

        public Verification(Element element, int timeoutSec = 0, bool failTest = false, bool isNot = false)
        {
            this.element = element;
            this.timeoutSec = timeoutSec;
            this.failTest = failTest;
            this.isNot = isNot;
        }

        private void VerificationFailed(string message = "")
        {
            if (message == "") message = GetErrorMessage();
            if (failTest)
            {
                Assert.Fail(message);
            }
            else
            {
                TestBaseClass.AddVerificationError(message);
            }

        }

        private string GetErrorMessage()
        {
            string newMessage;
            newMessage = isNot ? this.message : this.notMessage;

            return string.Format(errorMessage, Common.GetCurrentClassAndMethodName(), element.name, element.by,
                newMessage, this.timeoutSec.ToString());

        }


        public Element ChildElement(By bylocator)
        {
            this.message = "still found";
            this.notMessage = "not found";
            
            for (int i = 0; i <= this.timeoutSec; i++)
            {
                this.condition = element.FindElements(bylocator).Count > 0;
                if (condition != isNot)
                {
                    TestContext.CurrentContext.IncrementAssertCount();
                    if (Config.Settings.runTimeSettings.HighlightOnVerify)
                        element.Highlight();
                    return this.element;
                }
                Common.Delay(1000);
            }
            VerificationFailed();
            return this.element;
        }


        public Element Present()
        {
            this.message = "still present";
            this.notMessage = "not present";
            this.condition = element != null;
            for (int i = 0; i <= this.timeoutSec; i++)
            {
                if (condition!=isNot)
                {
                    TestContext.CurrentContext.IncrementAssertCount();
                    if (Config.Settings.runTimeSettings.HighlightOnVerify)
                        element.Highlight();
                    return this.element;
                }
                else
                {
                    Common.Delay(1000);
                    this.condition = (element != null);
                }
                    
            }
            VerificationFailed();
            return this.element;
        }

        public Element Visible()
        {
            this.message = "still visible";
            this.notMessage = "not visible";
            if (timeoutSec == 0) timeoutSec = Config.Settings.runTimeSettings.ElementTimeoutSec;
            for (int i = 0; i <= timeoutSec; i++)
            {
                this.condition = element.Displayed;
                if (condition!=isNot)
                {
                    TestContext.CurrentContext.IncrementAssertCount();
                    if (Config.Settings.runTimeSettings.HighlightOnVerify)
                        element.Highlight();
                    return this.element;
                }
                Common.Delay(1000);
            }
            VerificationFailed();
            return this.element;
        }

  
        public Element Text(string text)
        {
            this.message = "contains text '" + text + "'";
            this.notMessage = "doesn't contain text '" + text + "'";
            for (int i = 0; i <= timeoutSec; i++)
            {
                this.condition = (element != null) && (element.Text.Contains(text));
                if (this.condition != isNot)
                {
                    TestContext.CurrentContext.IncrementAssertCount();
                    if (Config.Settings.runTimeSettings.HighlightOnVerify)
                        element.Highlight();
                    return this.element;
                }
                else
                Common.Delay(1000);    
            }
            VerificationFailed();
            return this.element;
        }

        public Element Value(string text)
        {
            this.message = "has value " + text;
            this.notMessage = "doesn't have value " + text;
            for (int i = 0; i <= timeoutSec; i++)
            {
                this.condition = (element != null) && (element.GetAttribute("value").Contains(text));
                if (this.condition != isNot)
                {
                    TestContext.CurrentContext.IncrementAssertCount();
                    if (Config.Settings.runTimeSettings.HighlightOnVerify)
                        element.Highlight();
                    return this.element;
                }
                Common.Delay(1000);    
            }
            VerificationFailed();
            return this.element;
        }

        public Element Attribute(string attribute, string value)
        {
            this.message = "has attribute " + attribute + " with value " + value;
            this.notMessage = "doesn't have attribute " + attribute + " with value " + value;
            for (int i = 0; i <= timeoutSec; i++)
            {
                this.condition = (element != null) && (element.GetAttribute(attribute).Contains(value));
                if (condition != isNot)
                {
                    TestContext.CurrentContext.IncrementAssertCount();
                    if (Config.Settings.runTimeSettings.HighlightOnVerify)
                        element.Highlight();
                    return this.element;
                }
                Common.Delay(1000);                   
            }
            VerificationFailed();
            return this.element;
        }

        public Element Selected()
        {
            this.message = "is selected";
            this.notMessage = "isn't selected";
            for (int i = 0; i <= timeoutSec; i++)
            {
                this.condition = (element != null) && (element.Selected);
                if (condition != isNot)
                {
                    TestContext.CurrentContext.IncrementAssertCount();
                    if (Config.Settings.runTimeSettings.HighlightOnVerify)
                        element.Highlight();
                    return this.element;
                }
                Common.Delay(1000);
            }
            VerificationFailed();
            return this.element;
        }

        
    }
}
