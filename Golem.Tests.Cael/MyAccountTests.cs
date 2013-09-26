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
            string newPassword = @"Changeme1234!!";
            OpenPage<PageObjects.Cael.HomePage>(@"http://lcdev.bluemodus.com/").
                GoToLoginPage().
                Login(UserTests.email, UserTests.password)
                .Header.GoToMyAccountPage()
                .GoToPasswordPage().
                UpdateInfo(UserTests.email, newPassword).header.SignOut();

            OpenPage<PageObjects.Cael.HomePage>(@"http://lcdev.bluemodus.com/").
                GoToLoginPage().
                Login(UserTests.email, newPassword)
                .Header.GoToMyAccountPage()
                .GoToPasswordPage().
                UpdateInfo(UserTests.email, UserTests.password).
                header.SignOut();
        }

        [Test]
        public void VerifyContactInfo()
        {
            OpenPage<PageObjects.Cael.HomePage>(@"http://lcdev.bluemodus.com/").
               GoToLoginPage().
               Login(UserTests.email, UserTests.password)
               .Header.GoToMyAccountPage()
               .GoToContactInfoPage()
               .VerifyContactInfo(UserTests.firstName, UserTests.lastName, 
                                  UserTests.DOB_Month, UserTests.DOB_Day, UserTests.DOB_Year,
                                  UserTests.address1, UserTests.address2, UserTests.city, UserTests.state, UserTests.zip,
                                  UserTests.phone)
               .header.SignOut();
        }

        [Test]
        public void EditPassword()
        {
            string newPassword = @"Changeme1234!!";
            OpenPage<PageObjects.Cael.HomePage>(@"http://lcdev.bluemodus.com/").
                GoToLoginPage().
                Login(UserTests.email, UserTests.password)
                .Header.GoToMyAccountPage()
                .GoToPasswordPage().
                UpdateInfo(UserTests.email, newPassword).header.SignOut();
        }

        [Test]
        public void VerifyProfileFormValidations()
        {
            string defaultOptionStr = "please select";
            string txtBoxValidationStr = "* This field is required";
            string optionValidationStr = "* Please select an option";

            OpenPage<PageObjects.Cael.HomePage>(@"http://lcdev.bluemodus.com/").
                GoToLoginPage().
                Login(UserTests.email, UserTests.password)
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
                                                                                          txtBoxValidationStr).header.SignOut();

        }

        [Test]
        public void EditProfile()
        {
            OpenPage<PageObjects.Cael.HomePage>(@"http://lcdev.bluemodus.com/").
                GoToLoginPage().
                Login(UserTests.email, UserTests.password)
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
                Login(UserTests.email, UserTests.password)
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
