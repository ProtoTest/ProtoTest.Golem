using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Gallio.Framework;
using MbUnit.Framework;

namespace Golem.Framework
{
    public class Verification
    {
        private Element element;
        private int timeoutSec;
        private bool failTest;
        public bool Not;
        public Verification(Element element, int timeoutSec = 0, bool failTest=false,bool not=false)
        {
            this.element = element;
            this.timeoutSec = timeoutSec;
            this.failTest = failTest;
            this.Not = not;
        }
        
        public Element Present()
        {
            for (int i = 0; i <= this.timeoutSec; i++)
            {
                if (element != null)
                {
                    TestContext.CurrentContext.IncrementAssertCount();
                    if (Config.Settings.runTimeSettings.HighlightOnVerify)
                        element.Highlight();
                    return this.element;
                }
                else
                {
                    Common.Delay(1000);
                }
                    
            }
            VerificationFailed(Common.GetCurrentClassAndMethodName() + ": Element : " + this.element.name + " (" + this.element.by + ") not present after " + timeoutSec + " seconds");
            return this.element;
        }

        public Element Visible()
        {
            for (int i = 0; i <= timeoutSec; i++)
            {
                if (element != null)
                {
                    if (element.Displayed)
                    {
                        TestContext.CurrentContext.IncrementAssertCount();
                        if (Config.Settings.runTimeSettings.HighlightOnVerify)
                            element.Highlight();
                        return this.element;
                    }

                }
                Common.Delay(1000);
            }
            VerificationFailed(Common.GetCurrentClassAndMethodName() + ": Element : " + this.element.name + " (" + this.element.by + ") not visible after " + timeoutSec + " seconds");
            return this.element;
        }

        public Element NotPresent()
        {
            for (int i = 0; i <= timeoutSec; i++)
            {
                if (element == null)
                {
                    TestContext.CurrentContext.IncrementAssertCount();
                    return this.element;
                }

                else
                    Common.Delay(1000);
            }
            VerificationFailed(Common.GetCurrentClassAndMethodName() + ": Element : " + this.element.name + " (" + this.element.by + ") still present after " + timeoutSec + " seconds");
            return this.element;
        }
        public Element NotVisible()
        {
            for (int i = 0; i <= timeoutSec; i++)
            {
                if (!element.Displayed)
                {
                    TestContext.CurrentContext.IncrementAssertCount();
                    return this.element;
                }

                else
                    Common.Delay(1000);
            }
            VerificationFailed(Common.GetCurrentClassAndMethodName() + ": Element : " + this.element.name + " (" + this.element.by + ") still visible after " + timeoutSec + " seconds");
            return this.element;
        }

        public Element Text(string text)
        {
            for (int i = 0; i <= timeoutSec; i++)
            {
                if (element != null)
                {
                    if (element.Text.Contains(text))
                    {
                        TestContext.CurrentContext.IncrementAssertCount();
                        if (Config.Settings.runTimeSettings.HighlightOnVerify)
                            element.Highlight();
                        return this.element;
                    }
                }
                else
                    Common.Delay(1000);
            }
            VerificationFailed(Common.GetCurrentClassAndMethodName() + ": Element : " + this.element.name + " (" + this.element.by + ") did not have text : " + text + " after " + timeoutSec + " seconds");
            return this.element;
        }

        public Element Value(string text)
        {
            for (int i = 0; i <= timeoutSec; i++)
            {
                if (element != null)
                {
                    string value;

                    if ((value = element.GetAttribute("value")) != null)
                    {
                        if (value.Equals(text))
                        {
                            TestContext.CurrentContext.IncrementAssertCount();
                            if (Config.Settings.runTimeSettings.HighlightOnVerify)
                                element.Highlight();
                            return this.element;
                        }
                    }
                }
                else
                    Common.Delay(1000);
            }
            VerificationFailed(Common.GetCurrentClassAndMethodName() + ": Element : " + this.element.name + " (" + this.element.by + ") did not have attribute value : " + text + " after " + timeoutSec + " seconds");
            return this.element;
        }

        public Element Selected()
        {
            for (int i = 0; i <= timeoutSec; i++)
            {
                if (element != null)
                {
                    if (element.Selected)
                    {
                        TestContext.CurrentContext.IncrementAssertCount();
                        if (Config.Settings.runTimeSettings.HighlightOnVerify)
                           element.Highlight();
                        return this.element;
                    }
                }
                else
                    Common.Delay(1000);
            }
            VerificationFailed(Common.GetCurrentClassAndMethodName() + ": Element : " + this.element.name + " (" + this.element.by + ") was not selected after " + timeoutSec + " seconds");
            return this.element;
        }

        private void VerificationFailed(string message)
        {
            if (failTest)
            {
                Assert.Fail(message);
            }
            else
            {
                Golem.Framework.TestBaseClass.AddVerificationError(message);    
            }
            
        }
    }
}
