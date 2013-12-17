using System.Collections.Generic;
using Gallio.Common.Media;
using OpenQA.Selenium;
using ProtoTest.Golem.WebDriver;

namespace ProtoTest.Golem.Core
{
    public class TestDataContainer
    {
        public List<VerificationError> VerificationErrors;
        public ActionList actions;
        public Element lastElement = new Element();
        public ScreenRecorder recorder;
        public string testName;

        public TestDataContainer(string name)
        {
            testName = name;
            actions = new ActionList();
            VerificationErrors = new List<VerificationError>();
        }

        public IWebDriver driver { get; set; }
    }
}