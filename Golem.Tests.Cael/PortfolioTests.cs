using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using Golem.PageObjects.Cael;
using MbUnit.Framework;

namespace Golem.Tests.Cael
{
    [TestFixture,DependsOn(typeof(DashboardTests))]
    public class PortfolioTests : TestBaseClass
    {
        [Test, DependsOn("StartAnotherPortfolio")]
        [Row("Test Course", "English", "Literary Theory", 0)]
        [Row("Decline Course", "English", "Literature (Classics, World, English, etc.)", 1)]
        public void SetupPortfolio(string courseName, string courseCategory, string courseSubCategory, int portfolioRef )
        {
            // Pass this by reference, to return it from the calling function, since GetStarted() returns a page object
            int portfolioID = 0;

            HomePage.OpenHomePage().
                GoToLoginPage().
                Login(UserTests.email1, UserTests.password).
                Header.GoToPortfoliosPage().
                GetStarted(ref portfolioID).
                CreatePortfolio(courseName, "12340", "4", "Course Description", @"http://school.url/description",
                    "University of New England", @"http://school.com", "New England Association of Schools and Colleges",
                    courseCategory, courseSubCategory);

            // Save the portfolio ID for in the app config for user in other tests
            Common.UpdateConfigFile("PortfolioID_" + portfolioRef, portfolioID.ToString());
        }

        [Test]
        public void StartAnotherPortfolio()
        {
            HomePage.OpenHomePage().
                GoToLoginPage().
                Login(UserTests.email1, UserTests.password).
                Header.GoToPortfoliosPage().
                StartAnotherPortfolio().
                EnterPayment().
                ReturnToDashboardPage();
        }

        [Test, DependsOn("SetupPortfolio")]
        [Row("Test Course")]
        [Row("Decline Course")]
        public void EditPortfolio(string portfolioName)
        { 
            string tmpDir = Environment.GetEnvironmentVariable("temp");
            string narrativeFilePath = tmpDir + @"\sampledoc.docx";

            Common.CreateDummyFile(narrativeFilePath);

            HomePage.OpenHomePage().
                GoToLoginPage().
                Login(UserTests.email1, UserTests.password).
                Header.GoToPortfoliosPage().
                EditPortfolio(portfolioName)
                .EditOutcomesText("text of learning outcomes")
                .ChooseNarrativeFile(narrativeFilePath)
                .AddSupportDocument()
                .EnterSupportDocument("http://www.google.com/", "Caption Text", "Description")
                .SaveChanges();
        }

        [Test, DependsOn("EditPortfolio")]
        [Row("Test Course")]
        [Row("Decline Course")]
        public void SubmitPortfolio(string portfolioName)
        {
            HomePage.OpenHomePage().
                GoToLoginPage().
                Login(UserTests.email1, UserTests.password).
                Header.GoToPortfoliosPage().
                EditPortfolio(portfolioName).
                SubmitPortfolio().
                ConfirmAndSubmit().
                ReturnToDashboard();
        }

        [Test, DependsOn("SubmitPortfolio")]
        public void PreviewPortfolio()
        {
            HomePage.OpenHomePage().
                GoToLoginPage().
                Login(UserTests.email1, UserTests.password).
                Header.GoToPortfoliosPage().
                PreviewPortfolio("Test Course");
        }
    }
}
