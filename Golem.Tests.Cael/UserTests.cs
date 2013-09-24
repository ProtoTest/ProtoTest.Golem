using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gallio.Runtime.Formatting;
using Golem.Framework;
using Golem.PageObjects.Cael;
using Golem.PageObjects.Mailinator;
using MbUnit.Framework;

namespace Golem.Tests.Cael
{
    public class UserTests : TestBaseClass
    {
        //string email = "ProtoTestUser"+ Common.GetRandomString()+"@mailinator.com";
        public static string email = "prototestuser23103539@mailinator.com";
        public static string password = "prototest123!!";
        public static string firstName = "ProtoTest";
        public static string lastName = "Tester";
        public static string address1 = "1999 Broadway";
        public static string address2 = "#1410";
        public static string city = "Denver";
        public static string state = "Colorado";
        public static string zip = "80202";
        public static string phone = "3035551234";
        public static string DOB_Month = "January";
        public static string DOB_Day = "1";
        public static string DOB_Year = "1960";

        [Test]
        public void CreateNewUser()
        {
            
            OpenPage<PageObjects.Cael.HomePage>("http://lcdev.bluemodus.com/").
                GoToCreateUserPage().
                CreateUser(email,password,firstName,lastName,address1,address2,city,state,zip,phone,DOB_Month,DOB_Day,DOB_Year);


        }

        [Test,DependsOn("CreateNewUser")]
        public void ActivateUser()
        {
            OpenPage<PageObjects.Mailinator.HomePage>(@"http://mailinator.com/").
                Login(email).
                OpenEmailWithText("LearningCounts.org").
                ClickTextInBody("sign-in");

            LoginPage loginPage = new LoginPage();
            loginPage.Login(email,password);
        }

        [Test, DependsOn("ActivateUser")]
        public void ChangePassword()
        {
            string newPassword = @"Changeme1234!!";
            OpenPage<PageObjects.Cael.HomePage>(@"http://lcdev.bluemodus.com/").
                GoToLoginPage().
                Login(email, password)
                .header.GoToMyAccountPage()
                .GoToPasswordPage().
                UpdateInfo(email, newPassword).header.SignOut();

            OpenPage<PageObjects.Cael.HomePage>(@"http://lcdev.bluemodus.com/").
                GoToLoginPage().
                Login(email, newPassword)
                .header.GoToMyAccountPage()
                .GoToPasswordPage().
                UpdateInfo(email, password).header.SignOut();
        }

        [Test, DependsOn("ActivateUser")]
        public void EditPassword()
        {
            string newPassword = @"Changeme1234!!";
            OpenPage<PageObjects.Cael.HomePage>(@"http://lcdev.bluemodus.com/").
                GoToLoginPage().
                Login(email, password)
                .header.GoToMyAccountPage()
                .GoToPasswordPage().
                UpdateInfo(email, newPassword).header.SignOut();

        }
    }
}
