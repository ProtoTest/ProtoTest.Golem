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
    [TestFixture, DependsOn(typeof(PortfolioTests))]
    public class Kentico : WebDriverTestBase
    {
        [Test]
        public void AssignPortfoliosToAssessor()
        {
            // NOTE: Assessors can only be assigned to one area of expertise (i.e. English)
            // So make sure the portfolio's created are of category 'English'


            // Grab the previously saved portfolio ids to test and assign them to the assessor
            string portfolio_decline_id = Config.GetConfigValue("PortfolioID_0", null);
            string portfolio_assess_id = Config.GetConfigValue("PortfolioID_1", null);
            string assessor_login = Config.GetConfigValue("AssessorEmail", null);

            Assert.IsNotNull(portfolio_assess_id);
            Assert.IsNotNull(portfolio_decline_id);
            Assert.IsNotNull(assessor_login);

            String admin = Config.GetConfigValue("GlobalAdmin", "msiwiec@prototest.com");

            Golem.PageObjects.Cael.Kentico.Login(admin, UserTests.password)
                .AssignPortfolioToAssessor(portfolio_decline_id, assessor_login);

            Golem.PageObjects.Cael.Kentico.Login(admin, UserTests.password)
                .AssignPortfolioToAssessor(portfolio_assess_id, assessor_login);
            
        }
    }
}
