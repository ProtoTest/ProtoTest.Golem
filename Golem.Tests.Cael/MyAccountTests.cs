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
    public class MyAccountTests : TestBaseClass
    {
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
        public void ChangePassword()
        {
            Config.Settings.runTimeSettings.HighlightOnFind = false;
            string newPassword = @"Changeme1234!!";
            HomePage.OpenHomePage().
                GoToLoginPage().
                Login(UserTests.email1, UserTests.password)
                .Header.GoToMyAccountPage()
                .GoToPasswordPage().
                UpdateInfo(UserTests.email1, newPassword).LoggedInHeader.SignOut();

            HomePage.OpenHomePage().
                GoToLoginPage().
                Login(UserTests.email1, newPassword)
                .Header.GoToMyAccountPage()
                .GoToPasswordPage().
                UpdateInfo(UserTests.email1, UserTests.password).
                LoggedInHeader.SignOut();
        }

        [Test]
        public void VerifyContactInfo()
        {
            HomePage.OpenHomePage().
               GoToLoginPage().
               Login(UserTests.email1, UserTests.password)
               .Header.GoToMyAccountPage()
               .GoToContactInfoPage()
               .VerifyContactInfo(UserTests.firstName, UserTests.lastName, 
                                  UserTests.DOB_Month, UserTests.DOB_Day, UserTests.DOB_Year,
                                  UserTests.address1, UserTests.address2, UserTests.city, UserTests.state, UserTests.zip,
                                  UserTests.phone)
               .LoggedInHeader.SignOut();
        }

        [Test]
        public void EditPassword()
        {
            string newPassword = @"Changeme1234!!";
            HomePage.OpenHomePage().
                GoToLoginPage().
                Login(UserTests.email1, UserTests.password)
                .Header.GoToMyAccountPage()
                .GoToPasswordPage().
                UpdateInfo(UserTests.email1, newPassword)
               .LoggedInHeader.SignOut();
        }

        [Test]
        public void EditProfile()
        {
            HomePage.OpenHomePage().
                GoToLoginPage().
                Login(UserTests.email1, UserTests.password)
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
                                                    howHear).LoggedInHeader.SignOut();

        }

        [Test, DependsOn("EditProfile")]
        public void VerifyProfile()
        {

            HomePage.OpenHomePage().
                GoToLoginPage().
                Login(UserTests.email1, UserTests.password)
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
                                                    howHear).LoggedInHeader.SignOut();

        }

        [Test]
        public void VerifyProfileFormValidations()
        {
            string defaultOptionStr = "please select";
            string txtBoxValidationStr = "* This field is required";
            string optionValidationStr = "* Please select an option";

            HomePage.OpenHomePage().
                GoToLoginPage().
                Login(UserTests.email1, UserTests.password)
                .Header.GoToMyAccountPage()
                .GoToProfilePage().EnterProfileInfo(defaultOptionStr,
                                                    defaultOptionStr,
                                                    defaultOptionStr,
                                                    collegeCreditInvestigated,
                                                    defaultOptionStr,
                                                    defaultOptionStr,
                                                    onlineLearning,
                                                    financialAid,
                                                    defaultOptionStr,
                                                    gender,
                                                    defaultOptionStr,
                                                    defaultOptionStr,
                                                    defaultOptionStr,
                                                    "",
                                                    tuitionAssistance,
                                                    defaultOptionStr,
                                                    laborUnion,
                                                    receivedTraining,
                                                    "",
                                                    howHear).VerifyProfileFormValidations(txtBoxValidationStr,
                                                                                          txtBoxValidationStr,
                                                                                          optionValidationStr,
                                                                                          txtBoxValidationStr,
                                                                                          txtBoxValidationStr,
                                                                                          txtBoxValidationStr,
                                                                                          txtBoxValidationStr).LoggedInHeader.SignOut();

        }

        [Test]
        public void VerifyContactInfoFormValidations()
        {
            string defaultOptionStr = "please select";
            string txtBoxValidationStr = "* This field is required";

            HomePage.OpenHomePage().
               GoToLoginPage().
               Login(UserTests.email1, UserTests.password)
               .Header.GoToMyAccountPage()
               .GoToContactInfoPage()
               .EnterContactInfo("", "", "", "", "", "", "", "", defaultOptionStr, "", "")
               .VerifyContactInfoFormValidations(txtBoxValidationStr, txtBoxValidationStr, txtBoxValidationStr,
                                                 txtBoxValidationStr, txtBoxValidationStr, txtBoxValidationStr,
                                                 txtBoxValidationStr, txtBoxValidationStr, txtBoxValidationStr,
                                                 txtBoxValidationStr)
               .LoggedInHeader.SignOut();
        }
    }
}
