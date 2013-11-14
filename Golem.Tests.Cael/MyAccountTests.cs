using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gallio.Runtime.Formatting;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver;
using Golem.PageObjects.Cael;
using MbUnit.Framework;

namespace Golem.Tests.Cael
{
    [TestFixture, DependsOn(typeof(DashboardTests))]
    public class MyAccountTests : WebDriverTestBase
    {
        /* Profile inputs */
        string education = "Less than HS Diploma";
        string areaOfStudy = "Engineering and Engineering Technology";
        string nameOfCollege = "American Sentinel University";
        string collegeCreditInvestigated = "";
        string collegeCreditsEarned = "0-10";
        string collegeCreditsNeeded = "91-120";
        string onlineLearning = "";
        string financialAid = "";
        string topicOfInterest = "Choosing a Degree or Program of Study";
        string gender = "Male";
        string race = "Asian/Pacific";
        string militaryExp = "No Military Experience";
        string employmentStatus = "Not Currently Employed";
        string employerName = "Prototest";
        string tuitionAssistance = "";
        string annualIncome = "$20-29,000";
        string laborUnion = "";
        string receivedTraining = "";
        string typeOfTraining = "All of it";
        string howHear = "Other";


        [Test]
        public void ChangePassword()
        {
            Config.Settings.runTimeSettings.HighlightOnVerify = false;
            string newPassword = @"Changeme1234!!";
            HomePage.OpenHomePage().
                GoToLoginPage().
                Login(UserTests.email1, UserTests.password)
                .StudentHeader.GoToMyAccountPage()
                .GoToPasswordPage().
                UpdateInfo(UserTests.email1, newPassword).StudentHeader.SignOut();

            HomePage.OpenHomePage().
                GoToLoginPage().
                Login(UserTests.email1, newPassword)
                .StudentHeader.GoToMyAccountPage()
                .GoToPasswordPage().
                UpdateInfo(UserTests.email1, UserTests.password);
        }

        [Test]
        public void VerifyContactInfo()
        {
            HomePage.OpenHomePage().
                GoToLoginPage().
                Login(UserTests.email1, UserTests.password)
                .StudentHeader.GoToMyAccountPage()
                .GoToContactInfoPage()
                .VerifyContactInfo(UserTests.firstName, UserTests.lastName,
                                   UserTests.DOB_Month, UserTests.DOB_Day, UserTests.DOB_Year,
                                   UserTests.address1, UserTests.address2, UserTests.city, UserTests.state, UserTests.zip,
                                   UserTests.phone);
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
                .StudentHeader.GoToMyAccountPage()
                .GoToProfilePage().EnterProfileInfo(defaultOptionStr,
                                                    defaultOptionStr,
                                                    defaultOptionStr,
                                                    null,
                                                    defaultOptionStr,
                                                    defaultOptionStr,
                                                    null,
                                                    null,
                                                    defaultOptionStr,
                                                    null,
                                                    defaultOptionStr,
                                                    defaultOptionStr,
                                                    defaultOptionStr,
                                                    "",
                                                    null,
                                                    defaultOptionStr,
                                                    null,
                                                    null,
                                                    "",
                                                    howHear,
                                                    "").VerifyProfileFormValidations(txtBoxValidationStr,
                                                                                          txtBoxValidationStr,
                                                                                          optionValidationStr,
                                                                                          txtBoxValidationStr,
                                                                                          txtBoxValidationStr,
                                                                                          txtBoxValidationStr,
                                                                                          txtBoxValidationStr);

        }

        [Test, DependsOn("VerifyProfileFormValidations")]
        public void EditProfile()
        {
            HomePage.OpenHomePage().
                GoToLoginPage()
                .Login(UserTests.email1, UserTests.password)
                .StudentHeader.GoToMyAccountPage()
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
                                                    howHear);

        }

        [Test, DependsOn("EditProfile")]
        public void VerifyProfile()
        {

            HomePage.OpenHomePage().
                GoToLoginPage().
                Login(UserTests.email1, UserTests.password)
                .StudentHeader.GoToMyAccountPage()
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
                                                    howHear);

        }

        [Test]
        public void VerifyContactInfoFormValidations()
        {
            string defaultOptionStr = "please select";
            string txtBoxValidationStr = "* This field is required";

            HomePage.OpenHomePage().
                GoToLoginPage().
               Login(UserTests.email1, UserTests.password)
               .StudentHeader.GoToMyAccountPage()
               .GoToContactInfoPage()
               .EnterContactInfo("", "", "", "", "", "", "", "", defaultOptionStr, "", "", "")
               .VerifyContactInfoFormValidations(txtBoxValidationStr, txtBoxValidationStr, txtBoxValidationStr,
                                                 txtBoxValidationStr, txtBoxValidationStr, txtBoxValidationStr,
                                                 txtBoxValidationStr, txtBoxValidationStr, txtBoxValidationStr,
                                                 txtBoxValidationStr);
        }

    }
}