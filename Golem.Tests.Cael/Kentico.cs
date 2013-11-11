using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoTest.Golem.WebDriver;
using Golem.PageObjects.Cael;
using MbUnit.Framework;
using OpenQA.Selenium;

namespace Golem.Tests.Cael
{
    [TestFixture]
    public class Kentico : WebDriverTestBase
    {
        [Test]
        public void AssignPortfolioToAssessor()
        {
            Golem.PageObjects.Cael.Kentico.Login("bkitchener@prototest.com", "Qubit123!")
                .AssignPortfolioToAssessor("360", "prototestassessor2@mailinator.com");
        }
    }
}
