using Gallio.Framework;
using OpenQA.Selenium;
using ProtoTest.Golem.Core;

namespace ProtoTest.Golem.WebDriver.Elements.Validation
{
    /// <summary>
    ///     Validation Element class. Provides the functionality of an Element with the
    ///     added benefit of being able to verify form validations
    /// </summary>
    public class ValidationElement : Element
    {
        protected By locatorValidation;

        public ValidationElement()
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="name">Name of the element</param>
        /// <param name="locatorElement">Locator for the element</param>
        /// <param name="locatorValidation">Locator for the form validation</param>
        public ValidationElement(string name, By locatorElement, By locatorValidation) : base(name, locatorElement)
        {
            this.locatorValidation = locatorValidation;
        }

        /// <summary>
        ///     Verifies the text exists for the validation element.
        ///     The form validation error must be on screen before you call this API.
        /// </summary>
        /// <param name="text">The text to verify</param>
        /// <param name="seconds">Number iterations in seconds to retry finding the element</param>
        /// <returns>this</returns>
        public ValidationElement VerifyTextValidation(string text, int seconds = 0)
        {
            for (int i = 0; i <= seconds; i++)
            {
                if (driver.FindElements(locatorValidation).Count != 0)
                {
                    if (driver.FindElement(locatorValidation).Text.Contains(text))
                    {
                        TestContext.CurrentContext.IncrementAssertCount();
                        return this;
                    }
                }
                else
                {
                    Common.Delay(1000);
                }
            }

            TestBase.AddVerificationError(TestBase.GetCurrentClassAndMethodName() + ": Element : " + name + " (" +
                                          locatorValidation + ") not present after " + seconds + " seconds");
            return this;
        }
    }
}