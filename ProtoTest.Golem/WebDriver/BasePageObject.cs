using System;
using System.Reflection;
using OpenQA.Selenium;
using ProtoTest.Golem.Core;

namespace ProtoTest.Golem.WebDriver
{
    /// <summary>
    ///     BasePageObject should be inherited by all page objects used in the framework.  It represents either a base
    ///     component or page in an application.  You must implement void WaitForElements(), which should contain checks for
    ///     ajax elements being present/not present.  It contains a static reference to the WebDriverTestBase.driver object
    /// </summary>
    public abstract class BasePageObject
    {
        public string className;
        public string currentMethod;
        public string url;

        public BasePageObject()
        {
            driver = WebDriverTestBase.driver;
            className = GetType().Name;
            if (Config.settings.runTimeSettings.AutoWaitForElements)
            {
                try
                {
                    WaitForElements();
                }
                catch (Exception e)
                {
                    throw new WebDriverException(string.Format("The {0} Page failed to load : " + e.Message, className));
                }
            }
            TestBase.testData.actions.addAction(TestBase.GetCurrentClassAndMethodName());
        }

        public BasePageObject(IWebDriver driver)
        {
            this.driver = driver;
            className = GetType().Name;
            if (Config.settings.runTimeSettings.AutoWaitForElements)
            {
                WaitForElements();
            }
            TestBase.testData.actions.addAction(TestBase.GetCurrentClassAndMethodName());
        }

        private void GetElementNAmes()
        {
//            var props = this.GetType().GetProperties();
//            foreach (var prop in props)
//            {
//                if(prop.PropertyType.Name=="Element")
//                {
//                    var value = prop.GetValue(this, null) as Element;
//                    value.Set_Name(prop.Name);
////                    prop.SetValue(this, value, null);
//                    Log.Message("Set name to " + value.name);
//                }
//
//            }
//            props = this.GetType().GetProperties();
//            foreach (var prop in props)
//            {
//                if (prop.PropertyType.Name == "Element")
//                {
//                    var value = prop.GetValue(this, null) as Element;
//                    Log.Message("Set name to " + value.name);
//                }

//            }
            //            // Get the type of FieldsClass.
            //            Type fieldsType = this.GetType();
            //
            //            // Get an array of FieldInfo objects.
            //            FieldInfo[] fields = fieldsType.GetFields(BindingFlags.Public
            //                | BindingFlags.Instance);
            //            // Display the values of the fields.
            //            Console.WriteLine("Displaying the values of the fields of {0}:",
            //                fieldsType);
            //            for (int i = 0; i < fields.Length; i++)
            //            {
            //                var text = string.Format("   {0}:\t'{1}'",
            //                    fields[i].Name, fields[i].GetValue(this));
            //
            //            }
        }

        public IWebDriver driver { get; set; }
        public abstract void WaitForElements();
    }
}