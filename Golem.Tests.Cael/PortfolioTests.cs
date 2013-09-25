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
    public class PortfolioTests : TestBaseClass
    {
        [Test]
        public void CreateNewPortfolio()
        {
            OpenPage<HomePage>(@"http://lcdev.bluemodus.com/").
                GoToLoginPage().
                Login(UserTests.email, UserTests.password).
                Header.GoToPortfoliosPage().
                StartAnotherPortfolio().
                EnterPayment().
                ReturnToDashboardPage();
        }

        [Test,DependsOn("CreateNewPortfolio")]
        public void SetupPortfolio()
        {
            OpenPage<HomePage>(@"http://lcdev.bluemodus.com/").
                GoToLoginPage().
                Login(UserTests.email, UserTests.password).
                Header.GoToPortfoliosPage().
                GetStarted().
                CreatePortfolio("Test Course", "12340", "4", "Course Description", @"http://school.url/description",
                    "University of New England", "http://school.com", "New England Association of Schools and Colleges",
                    "English", "Literary Theory");
        }


        [Test, DependsOn("SetupPortfolio")]
        public void EditPortfolio()
        {
            OpenPage<HomePage>(@"http://lcdev.bluemodus.com/").
                GoToLoginPage().
                Login(UserTests.email, UserTests.password).
                Header.GoToPortfoliosPage().
                EditPortfolio("Test Course")
                .EditOutcomesText("text of learning outcomes")
                .ChooseNarrativeFile(@"C:\Users\Brian\Documents\sampledoc.docx")
                .AddSupportDocument()
                .EnterSupportDocument("http://www.google.com/", "Caption Text", "Description")
                .SaveChanges();
        }

        [Test, DependsOn("EditPortfolio")]
        public void SubmitPortfolio()
        {
            OpenPage<HomePage>(@"http://lcdev.bluemodus.com/").
                GoToLoginPage().
                Login(UserTests.email, UserTests.password).
                Header.GoToPortfoliosPage().
                EditPortfolio("Test Course").
                SubmitPortfolio().
                ConfirmAndSubmit().
                ReturnToDashboard();
        }

        [Test, DependsOn("SubmitPortfolio")]
        public void PreviewPortfolio()
        {
            OpenPage<HomePage>(@"http://lcdev.bluemodus.com/").
                GoToLoginPage().
                Login(UserTests.email, UserTests.password).
                Header.GoToPortfoliosPage().
                PreviewPortfolio("Test Course");
        }

    }
}
