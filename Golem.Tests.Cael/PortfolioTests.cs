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
        [Test]
        public void SetupPortfolio()
        {
            HomePage.OpenHomePage().
                GoToLoginPage().
                Login(UserTests.email1, UserTests.password).
                Header.GoToPortfoliosPage().
                GetStarted().
                CreatePortfolio("Test Course", "12340", "4", "Course Description", @"http://school.url/description",
                    "University of New England", "http://school.com", "New England Association of Schools and Colleges",
                    "English", "Literary Theory");
        }

        [Test, DependsOn("SetupPortfolio")]
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
        public void EditPortfolio()
        { 
            string tmpDir = Environment.GetEnvironmentVariable("temp");
            string narrativeFilePath = tmpDir + @"\sampledoc.docx";

            Common.CreateDummyFile(narrativeFilePath);

            HomePage.OpenHomePage().
                GoToLoginPage().
                Login(UserTests.email1, UserTests.password).
                Header.GoToPortfoliosPage().
                EditPortfolio("Test Course")
                .EditOutcomesText("text of learning outcomes")
                .ChooseNarrativeFile(narrativeFilePath)
                .AddSupportDocument()
                .EnterSupportDocument("http://www.google.com/", "Caption Text", "Description")
                .SaveChanges();
        }

        [Test, DependsOn("EditPortfolio")]
        public void SubmitPortfolio()
        {
            HomePage.OpenHomePage().
                GoToLoginPage().
                Login(UserTests.email1, UserTests.password).
                Header.GoToPortfoliosPage().
                EditPortfolio("Test Course").
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
