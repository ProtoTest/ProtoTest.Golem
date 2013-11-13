using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoTest.Golem.WebDriver;
using Golem.PageObjects.Cael;
using MbUnit.Framework;
using OpenQA.Selenium;
using ProtoTest.Golem.Core;

namespace Golem.Tests.Cael
{
    [TestFixture]
    public class Kentico : WebDriverTestBase
    {
        [Test]
        public void AssignPortfoliosToAssessor()
        {
            // NOTE: Assessors can only be assigned to one area of expertise (i.e. English)
            // So make sure the portfolio's created are of category 'English'
            string default_id = "invalid";

            // Grab the previously saved portfolio ids to test and assign them to the assessor
            string portfolio_decline_id = Config.GetConfigValue("PortfolioID_1", default_id);
            string portfolio_assess_id = Config.GetConfigValue("PortfolioID_2", default_id);

            if ((default_id != portfolio_assess_id) && (default_id != portfolio_decline_id))
            {
                Golem.PageObjects.Cael.Kentico.Login("bkitchener@prototest.com", "Qubit123!")
                    .AssignPortfolioToAssessor(portfolio_decline_id, "prototestassessor2@mailinator.com");

                Golem.PageObjects.Cael.Kentico.Login("bkitchener@prototest.com", "Qubit123!")
                    .AssignPortfolioToAssessor(portfolio_assess_id, "prototestassessor2@mailinator.com");
            }
        }
    }
}
