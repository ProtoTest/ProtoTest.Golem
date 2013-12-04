using System.Drawing;
using Gallio.Framework;
using MbUnit.Framework;
using OpenQA.Selenium;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver.Elements;

namespace ProtoTest.Golem.WebDriver
{
    public class ElementVerification
    {
        private const string errorMessage = "{0}: {1}({2}): {3} after {4} seconds";
        private readonly Element element;
        private readonly bool failTest;
        private readonly bool isTrue = true;
        private bool condition;
        private string message;
        private string notMessage;
        private int timeoutSec;

        public ElementVerification(Element element, int timeoutSec = 0, bool failTest = false, bool isTrue = true)
        {
            this.element = element;
            this.timeoutSec = timeoutSec;
            this.failTest = failTest;
            this.isTrue = isTrue;
        }

        public ElementVerification Not()
        {
            return new ElementVerification(element, timeoutSec, failTest, false); 
        }

        private void VerificationFailed(string message = "", Image image = null)
        {
            if (message == "") message = GetErrorMessage();
            if (failTest)
            {
                Assert.Fail(message);
            }
            else
            {
                if (image == null)
                    TestBase.AddVerificationError(message);
                else
                    TestBase.AddVerificationError(message, image);
            }
        }


        private string GetErrorMessage()
        {
            string newMessage;
            newMessage = isTrue ? notMessage : message;

            return string.Format(errorMessage, TestBase.GetCurrentClassAndMethodName(), element.name, element.by,
                newMessage, timeoutSec);
        }

        private string GetSuccessMessage()
        {
            string newMessage;
            string correctMessage = "{0}: {1}({2}): {3}";
            newMessage = isTrue ? message : notMessage;

            return string.Format(correctMessage, TestBase.GetCurrentClassAndMethodName(), element.name, element.by,newMessage);
        }


        public Element ChildElement(By bylocator)
        {
            message = "is found";
            notMessage = "not found";

            for (int i = 0; i <= timeoutSec; i++)
            {
                condition = element.FindElements(bylocator).Count > 0;
                if (condition == isTrue)
                {
                    TestBase.LogEvent("!--Verification Passed " + GetSuccessMessage());
                    TestContext.CurrentContext.IncrementAssertCount();
                    if (Config.Settings.runTimeSettings.HighlightOnVerify)
                        element.Highlight();
                    return element;
                }
                Common.Delay(1000);
            }
            VerificationFailed();
            return element;
        }


        public Element Present()
        {
            message = "is present";
            notMessage = "not present";
            condition = element.Present;
            for (int i = 0; i <= timeoutSec; i++)
            {
                if (condition == isTrue)
                {
                    TestBase.LogEvent("!--Verification Passed " + GetSuccessMessage());
                    TestContext.CurrentContext.IncrementAssertCount();
                    if (Config.Settings.runTimeSettings.HighlightOnVerify)
                        element.Highlight();
                    return element;
                }
                Common.Delay(1000);
                condition = (element.Present);
            }
            VerificationFailed();
            return element;
        }

        public Element Visible()
        {
            message = "is visible";
            notMessage = "not visible";
            if (timeoutSec == 0) timeoutSec = Config.Settings.runTimeSettings.ElementTimeoutSec;
            for (int i = 0; i <= timeoutSec; i++)
            {
                condition = element.Displayed;
                if (condition == isTrue)
                {
                    TestBase.LogEvent("!--Verification Passed " + GetSuccessMessage());
                    TestContext.CurrentContext.IncrementAssertCount();
                    if (Config.Settings.runTimeSettings.HighlightOnVerify)
                        element.Highlight();
                    return element;
                }
                Common.Delay(1000);
            }
            VerificationFailed();
            return element;
        }


        public Element Text(string text)
        {
            message = "contains text '" + text + "'";
            notMessage = "doesn't contain text '" + text + "'";
            for (int i = 0; i <= timeoutSec; i++)
            {
                condition = (element.Present) && (element.Text.Contains(text));
                if (condition == isTrue)
                {
                    TestBase.LogEvent("!--Verification Passed " + GetSuccessMessage());
                    TestContext.CurrentContext.IncrementAssertCount();
                    if (Config.Settings.runTimeSettings.HighlightOnVerify)
                        element.Highlight();
                    return element;
                }
                Common.Delay(1000);
            }
            VerificationFailed();
            return element;
        }

        public Element Value(string text)
        {
            message = "has value " + text;
            notMessage = "doesn't have value " + text;
            for (int i = 0; i <= timeoutSec; i++)
            {
                condition = (element.Present) && (element.GetAttribute("value").Contains(text));
                if (condition == isTrue)
                {
                    TestBase.LogEvent("!--Verification Passed " + GetSuccessMessage());
                    TestContext.CurrentContext.IncrementAssertCount();
                    if (Config.Settings.runTimeSettings.HighlightOnVerify)
                        element.Highlight();
                    return element;
                }
                Common.Delay(1000);
            }
            VerificationFailed();
            return element;
        }

        public Element Attribute(string attribute, string value)
        {
            message = "has attribute " + attribute + " with value " + value;
            notMessage = "doesn't have attribute " + attribute + " with value " + value;
            for (int i = 0; i <= timeoutSec; i++)
            {
                condition = (element.Present) && (element.GetAttribute(attribute).Contains(value));
                if (condition == isTrue)
                {
                    TestBase.LogEvent("!--Verification Passed " + GetSuccessMessage());
                    TestContext.CurrentContext.IncrementAssertCount();
                    if (Config.Settings.runTimeSettings.HighlightOnVerify)
                        element.Highlight();
                    return element;
                }
                Common.Delay(1000);
            }
            VerificationFailed();
            return element;
        }

        public Element Selected()
        {
            message = "is selected";
            notMessage = "isn't selected";
            for (int i = 0; i <= timeoutSec; i++)
            {
                condition = (element.Present) && (element.Selected);
                if (condition == isTrue)
                {
                    TestBase.LogEvent("!--Verification Passed " + GetSuccessMessage());
                    TestContext.CurrentContext.IncrementAssertCount();
                    if (Config.Settings.runTimeSettings.HighlightOnVerify)
                        element.Highlight();
                    return element;
                }
                Common.Delay(1000);
            }
            VerificationFailed();
            return element;
        }

        public Element Image()
        {
            message = "image matches";
            notMessage = "image is {0} different";
            condition = (element.Present) && (element.Images.ImagesMatch());
            if (condition == isTrue)
            {
                TestContext.CurrentContext.IncrementAssertCount();
                TestBase.LogEvent(GetSuccessMessage());
            }
                
            else
            {
                notMessage = string.Format(notMessage, element.Images.differenceString);
                VerificationFailed(string.Format("{0}: {1}({2}): {3}", TestBase.GetCurrentClassAndMethodName(), element.name, element.by,
                notMessage), element.Images.GetMergedImage());
            }
                
            return element;
        }

        
    }
}