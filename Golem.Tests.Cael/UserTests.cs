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
using HomePage = Golem.PageObjects.Cael.HomePage;



namespace Golem.Tests.Cael
{
    public class UserTests : TestBaseClass
    {
        public static string email1 = Config.GetConfigValue("UserEmail1", "prototestuser02141502@mailinator.com");
        public static string email2 = Config.GetConfigValue("UserEmail2", "prototestuser02141502@mailinator.com");
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
        public void CreateNewUsers()
        {
            string newEmail = "ProtoTestUser"+ Common.GetRandomString()+"@mailinator.com";
            HomePage.OpenHomePage().
                GoToCreateUserPage().
                CreateUser(newEmail,password,firstName,lastName,address1,address2,city,state,zip,phone,DOB_Month,DOB_Day,DOB_Year);
            Common.UpdateConfigFile("UserEmail1",newEmail);
            
            newEmail = "prototestassessor" + Common.GetRandomString() + "@mailinator.com";
            string department = "English";
            string[] subjects = {"Literature (Classics, World, English, etc.)", "Literary Theory"};
            
            Golem.PageObjects.Cael.Kentico.Login("bkitchener@prototest.com", "Qubit123!").CreateAssessor(newEmail, password, department, subjects);

            Common.UpdateConfigFile("UserEmail2", newEmail);

        }
        
        [Timeout(0)]
        [Test,DependsOn("CreateNewUsers")]
        public void ActivateUser()
        {
            OpenPage<PageObjects.Mailinator.HomePage>(@"http://mailinator.com/").
                Login(email1).
                WaitForEmail("LearningCounts.org",20).
                OpenEmailWithText("LearningCounts.org").
                ClickTextInBody("sign-in");

            LoginPage loginPage = new LoginPage();
            loginPage.Login(email1,password);

            OpenPage<PageObjects.Mailinator.HomePage>(@"http://mailinator.com/").
                Login(email2).
                WaitForEmail("LearningCounts.org", 20).
                OpenEmailWithText("LearningCounts.org").
                ClickTextInBody("sign-in");
            
            loginPage.Login(email2,password);
        }
    }
}
