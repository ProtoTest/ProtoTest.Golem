using System.Linq;
using Gallio.Framework;
using MbUnit.Framework;
using OpenQA.Selenium;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.Purple.Elements;
using ProtoTest.Golem.WebDriver;
using Image = System.Drawing.Image;

namespace ProtoTest.Golem.Purple
{
    /// <summary>
    /// Methods for performing non-terminating validations, and Wait commands.
    /// TODO I refactored the IPurpleElement class - only used for image comparison at the moment
    /// </summary>
    public class ElementVerification
    {
        private const string errorMessage = "{0}: {1}({2}): {3} after {4} seconds";
        private readonly IPurpleElement element;
        private readonly bool failTest;
        private readonly bool isTrue = true;
        private bool condition;
        private string message;
        private string notMessage;
        private int timeoutSec;

        public ElementVerification(IPurpleElement element, int timeoutSec = 0, bool failTest = false, bool isTrue = true)
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

            return string.Format(errorMessage, TestBase.GetCurrentClassAndMethodName(), element.ElementName, element.PurplePath,
                newMessage, timeoutSec);
        }

        private string GetSuccessMessage()
        {
            string newMessage;
            string correctMessage = "{0}: {1}({2}): {3}";
            newMessage = isTrue ? message : notMessage;

            return string.Format(correctMessage, TestBase.GetCurrentClassAndMethodName(), element.ElementName, element.PurplePath,
                newMessage);
        }


        //public IPurpleElement HasChildElement(string PurplePath)
        //{
        //    //message = "has child with PurplePath " + PurplePath;
        //    //notMessage = "no child with PurplePath " + PurplePath;

        //    //for (int i = 0; i <= timeoutSec; i++)
        //    //{
        //    //    condition = element.UIAElement.Current.IsEnabled && element.getItem().GetMultiple(PurplePath).Length > 0;
        //    //    if (condition == isTrue)
        //    //    {
        //    //        TestBase.LogEvent("!--Verification Passed " + GetSuccessMessage());
        //    //        return element;
        //    //    }
        //    //    Common.Delay(1000);
        //    //}
        //    //VerificationFailed();
        //    //return element;
        //}


        //public IPurpleElement Present()
        //{
        //    //message = "is present";
        //    //notMessage = "not present";

        //    //for (int i = 0; i <= timeoutSec; i++)
        //    //{
        //    //    condition = element.getItem().Present();
        //    //    if (condition == isTrue)
        //    //    {
        //    //        TestBase.LogEvent("!--Verification Passed " + GetSuccessMessage());
        //    //        return element;
        //    //    }
        //    //    Common.Delay(1000);
        //    //}
        //    //VerificationFailed();
        //    //return element;
        //}

        //public IPurpleElement Visible()
        //{
        //    //message = "is visible";
        //    //notMessage = "not visible";
        //    //if (timeoutSec == 0) timeoutSec = Config.Settings.runTimeSettings.ElementTimeoutSec;
        //    //for (int i = 0; i <= timeoutSec; i++)
        //    //{
        //    //    condition = element.getItem().Present() && element.getItem().Visible;
        //    //    if (condition == isTrue)
        //    //    {
        //    //        TestBase.LogEvent("!--Verification Passed " + GetSuccessMessage());
        //    //        return element;
        //    //    }
        //    //    Common.Delay(1000);
        //    //}
        //    //VerificationFailed();
        //    //return element;
        //}

        //public IPurpleElement Name(string value)
        //{
        //    //message = "contains text '" + value + "'";
        //    //notMessage = "doesn't contain text '" + value + "'";
        //    //for (int i = 0; i <= timeoutSec; i++)
        //    //{

        //    //    condition = element.getItem().Present() && (element.getItem().Name.Contains(value));
        //    //    if (condition == isTrue)
        //    //    {
        //    //        TestBase.LogEvent("!--Verification Passed " + GetSuccessMessage());
        //    //        return element;
        //    //    }
        //    //    Common.Delay(1000);
        //    //}
        //    //VerificationFailed();
        //    //return element;
        //}

        public IPurpleElement Image()
        {
            message = "image matches";
            notMessage = "image is {0} different";
            var comparer = new ElementImageComparer(element);
            condition = element.UIAElement.Current.IsEnabled && comparer.ImagesMatch();
            if (condition == isTrue)
            {
                TestContext.CurrentContext.IncrementAssertCount();
                TestBase.LogEvent(GetSuccessMessage());
            }

            else
            {
                notMessage = string.Format(notMessage, comparer.differenceString);
                VerificationFailed(
                    string.Format("{0}: {1}({2}): {3}", TestBase.GetCurrentClassAndMethodName(), element.ElementName,
                        element.PurplePath,
                        notMessage), comparer.GetMergedImage());
            }

            return element;
        }
    }
}