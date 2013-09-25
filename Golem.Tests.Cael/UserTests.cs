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


        /* Profile inputs */
        string education = "Less than HS Diploma";
        string areaOfStudy = "Engineering and Engineering Technology";
        string nameOfCollege = "American Sentinel University";
        bool collegeCreditInvestigated = true;
        string collegeCreditsEarned = "0-10";
        string collegeCreditsNeeded = "91-120";
        bool onlineLearning = true;
        bool financialAid = true;
        string topicOfInterest = "Choosing a Degree or Program of Study";
        string gender = "Male";
        string race = "Asian/Pacific";
        string militaryExp = "No Military Experience";
        string employmentStatus = "Not Currently Employed";
        string employerName = "Prototest";
        bool tuitionAssistance = true;
        string annualIncome = "$20-29,000";
        bool laborUnion = false;
        bool receivedTraining = true;
        string typeOfTraining = "All of it";
        string howHear = "Other";


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
                .Header.GoToMyAccountPage()
                .GoToPasswordPage().
                UpdateInfo(email, newPassword).header.SignOut();

            OpenPage<PageObjects.Cael.HomePage>(@"http://lcdev.bluemodus.com/").
                GoToLoginPage().
                Login(email, newPassword)
                .Header.GoToMyAccountPage()
                .GoToPasswordPage().
                UpdateInfo(email, password).
                header.SignOut();
        }

        [Test, DependsOn("ActivateUser")]
        public void VerifyContactInfo()
        {
            OpenPage<PageObjects.Cael.HomePage>(@"http://lcdev.bluemodus.com/").
               GoToLoginPage().
               Login(email, password)
               .Header.GoToMyAccountPage()
               .GoToContactInfoPage()
               .VerifyContactInfo(firstName, lastName, DOB_Month, DOB_Day, DOB_Year, address1, address2, city, state, zip, phone)
               .header.SignOut();
        }

        [Test, DependsOn("ActivateUser")]
        public void EditPassword()
        {
            string newPassword = @"Changeme1234!!";
            OpenPage<PageObjects.Cael.HomePage>(@"http://lcdev.bluemodus.com/").
                GoToLoginPage().
                Login(email, password)
                .Header.GoToMyAccountPage()
                .GoToPasswordPage().
                UpdateInfo(email, newPassword).header.SignOut();
        }

        [Test, DependsOn("ActivateUser")]
        public void EditProfile()
        {
            string education = "Less than HS Diploma";
            string areaOfStudy = "Engineering and Engineering Technology";
            string nameOfCollege = "American Sentinel University";
            bool collegeCreditInvestigated = true;
            string collegeCreditsEarned = "0-10";
            string collegeCreditsNeeded = "91-120";
            bool onlineLearning = true;
            bool financialAid = true;
            string topicOfInterest = "Choosing a Degree or Program of Study";
            string gender = "Male";
            string race = "Asian/Pacific";
            string militaryExp = "No Military Experience";
            string employmentStatus = "Not Currently Employed";
            string employerName = "Prototest";
            bool tuitionAssistance = true;
            string annualIncome = "$20-29,000";
            bool laborUnion = false;
            bool receivedTraining = true;
            string typeOfTraining = "All of it";
            string howHear = "Other";


            OpenPage<PageObjects.Cael.HomePage>(@"http://lcdev.bluemodus.com/").
                GoToLoginPage().
                Login(email, password)
                .Header.GoToMyAccountPage()
                .GoToProfilePage().EnterProfileInfo(education,
                                                    areaOfStudy,
                                                    nameOfCollege,
                                                    collegeCreditInvestigated,
                                                    collegeCreditsEarned,
                                                    collegeCreditsNeeded,
                                                    onlineLearning,
                                                    financialAid,
                                                    topicOfInterest,
                                                    gender,
                                                    race,
                                                    militaryExp,
                                                    employmentStatus,
                                                    employerName,
                                                    tuitionAssistance,
                                                    annualIncome,
                                                    laborUnion,
                                                    receivedTraining,
                                                    typeOfTraining,
                                                    howHear).header.SignOut();

        }

        [Test, DependsOn("EditProfile")]
        public void VerifyProfile()
        {
           
            OpenPage<PageObjects.Cael.HomePage>(@"http://lcdev.bluemodus.com/").
                GoToLoginPage().
                Login(email, password)
                .Header.GoToMyAccountPage()
                .GoToProfilePage().VerifyProfileInfo(education,
                                                    areaOfStudy,
                                                    nameOfCollege,
                                                    collegeCreditInvestigated,
                                                    collegeCreditsEarned,
                                                    collegeCreditsNeeded,
                                                    onlineLearning,
                                                    financialAid,
                                                    topicOfInterest,
                                                    gender,
                                                    race,
                                                    militaryExp,
                                                    employmentStatus,
                                                    employerName,
                                                    tuitionAssistance,
                                                    annualIncome,
                                                    laborUnion,
                                                    receivedTraining,
                                                    typeOfTraining,
                                                    howHear).header.SignOut();

        }
    }
}
