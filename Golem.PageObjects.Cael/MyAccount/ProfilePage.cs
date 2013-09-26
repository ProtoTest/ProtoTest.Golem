using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using OpenQA.Selenium;

namespace Golem.PageObjects.Cael.MyAccount
{
    public class ProfilePage : MyAccountPage
    {
        public ValidationElement EducationAchieved_Drp = new ValidationElement("Education Achieved Dropdowqn", 
                                                                                By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_educationAchievedDDList"),
                                                                                By.XPath("//*[@class='p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_educationAchievedDDListformError parentFormform formError']/div"));
        public Element AreaOfStudy_Drp = new Element("Area of Study", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_areaOfStudyDDList"));
        public ValidationElement NameOfCollege_Drp = new ValidationElement("Name of College",
                                                                           By.Id("schoolDDList"),
                                                                           By.XPath("//*[@class='schoolDDListformError parentFormform formError']/div"));
        public Element CollegeCredit_Radio = new Element("College Credit Interest Radio", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_investigationRButton_0"));
        public Element CollegeCreditsEarned_Drp = new Element("College Credits Earned Dropdown", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_creditsEarnedDDList"));
        public Element CollegeCreditsNeed_Drp = new Element("College Credits Needed Dropdown", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_creditsRemainingDDList"));
        public Element InterestedOnline_Radio = new Element("InterestedOnline_Radio", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_onlineRButton_0"));
        public Element FinancialAid_Radio = new Element("FinancialAid_Radio", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_aidRButton_0"));
        public Element TopicOfInterest_Drp = new Element("Topic of Interest", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_topicOfInterestDDList"));
        public ValidationElement Gender_Radio = new ValidationElement("Gender_Radio",
                                                                      By.Id("genderRButton_0"),
                                                                      By.XPath("//*[@class='genderRButton_0formError parentFormform formError']/div"));
        public ValidationElement Race_Drp = new ValidationElement("Race_Drp",
                                                                  By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_EthnicityDDList"),
                                                                  By.XPath("//*[@class='p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_EthnicityDDListformError parentFormform formError']/div"));
        public ValidationElement MilitaryExperience_Drp = new ValidationElement("MilitaryExperience_Drp",
                                                                                By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_militaryDDList"),
                                                                                By.XPath("//*[@class='p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_militaryDDListformError parentFormform formError']/div"));
        public ValidationElement EmploymentStatus_Drp = new ValidationElement("EmploymentStatus_Drp",
                                                                              By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_employmentStatusDDList"),
                                                                              By.XPath("//*[@class='p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_employmentStatusDDListformError parentFormform formError']/div"));
        public Element Employer_Field = new Element("Employer_Field", By.Id("employerTextBox"));
        public Element TuitionAssistance_Radio = new Element("TuitionAssistance_Radio", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_tuitionRButton_0"));

        public ValidationElement AnnualIncome_Drp = new ValidationElement("AnnualIncome_Drp",
                                                                          ByE.PartialAttribute("select", "@id", "annualPersonalIncomeDDList"),
                                                                          By.XPath("//*[@class='p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_annualPersonalIncomeDDListformError parentFormform formError']/div"));
        public Element LaborUnion_Radio = new Element("LaborUnion_Radio", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_laborRButton_0"));
        public Element ReceivedTraining_Radio = new Element("ReceivedTraining_Radio", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_trainingRButton_0"));
        public Element NameOrTypeOfTrainnig_Field = new Element("NameOrTypeOfTrainnig_Field", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_trainingTypeTextBox"));
        public Element HowDidYouHear_Drp = new Element("HowDidYouHear_Drp", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_howDidYouHearDDList"));
        public Element SaveChanges_Btn = new Element("SaveChanges_Btn", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_saveProfileButton"));


        public ProfilePage EnterProfileInfo(string education, string areaOfStudy, string nameOfCollege, bool collegeCreditInvestigated,
            string collegeCreditsEarned, string collegeCreditsNeeded, bool onlineLearning, bool financialAid,
            string topicOfInterest, string gender, string race, string militaryExperience, string employmentStatus,
            string nameOfEmployer, bool tuitionAssitance, string annualIncome, bool laborUnion, bool receviedTraining,
            string typeOfTraining, string howHear)
        {
            EducationAchieved_Drp.SelectOption(education);
            AreaOfStudy_Drp.SelectOption(areaOfStudy);
            NameOfCollege_Drp.SelectOption(nameOfCollege);
            CollegeCredit_Radio.Click();
            CollegeCreditsEarned_Drp.SelectOption(collegeCreditsEarned);
            CollegeCreditsNeed_Drp.SelectOption(collegeCreditsNeeded);
            InterestedOnline_Radio.Click();
            FinancialAid_Radio.Click();
            TopicOfInterest_Drp.SelectOption(topicOfInterest);
            Gender_Radio.Click();
            Race_Drp.SelectOption(race);
            MilitaryExperience_Drp.SelectOption(militaryExperience);
            EmploymentStatus_Drp.SelectOption(employmentStatus);
            Employer_Field.Text = nameOfEmployer;
            TuitionAssistance_Radio.Click();
            AnnualIncome_Drp.SelectOption(annualIncome);
            LaborUnion_Radio.Click();
            ReceivedTraining_Radio.Click();
            NameOrTypeOfTrainnig_Field.Text = typeOfTraining;
            HowDidYouHear_Drp.SelectOption(howHear);
            SaveChanges_Btn.Click();

            return new ProfilePage();

        }

        public ProfilePage VerifyProfileInfo(string education, string areaOfStudy, string nameOfCollege, bool collegeCreditInvestigated,
            string collegeCreditsEarned, string collegeCreditsNeeded, bool onlineLearning, bool financialAid,
            string topicOfInterest, string gender, string race, string militaryExperience, string employmentStatus,
            string nameOfEmployer, bool tuitionAssitance, string annualIncome, bool laborUnion, bool receviedTraining,
            string typeOfTraining, string howHear)
        {
            EducationAchieved_Drp.VerifyText(education);
            AreaOfStudy_Drp.VerifyText(areaOfStudy);
            NameOfCollege_Drp.VerifyText(nameOfCollege);
            CollegeCredit_Radio.VerifySelected();
            CollegeCreditsEarned_Drp.VerifyText(collegeCreditsEarned);
            CollegeCreditsNeed_Drp.VerifyText(collegeCreditsNeeded);
            InterestedOnline_Radio.VerifySelected();
            FinancialAid_Radio.VerifySelected();
            TopicOfInterest_Drp.VerifyText(topicOfInterest);
            Gender_Radio.VerifySelected();
            Race_Drp.VerifyText(race);
            MilitaryExperience_Drp.VerifyText(militaryExperience);
            EmploymentStatus_Drp.VerifyText(employmentStatus);
            Employer_Field.VerifyValue(nameOfEmployer);
            TuitionAssistance_Radio.VerifySelected();
            AnnualIncome_Drp.VerifyText(annualIncome);
            LaborUnion_Radio.VerifySelected();
            ReceivedTraining_Radio.VerifySelected();
            NameOrTypeOfTrainnig_Field.VerifyText(typeOfTraining);
            HowDidYouHear_Drp.VerifyText(howHear);

            return this;

        }

        public ProfilePage VerifyProfileFormValidations(string education, string nameOfCollege, string gender,
            string race, string militaryExperience, string employmentStatus, string annualIncome)
        {
            EducationAchieved_Drp.VerifyTextValidation(education);
            NameOfCollege_Drp.VerifyTextValidation(nameOfCollege);
            Golem.Framework.Common.Log("ATTEMPTING TO CLEAR GENDER RADIO!!!!!");
            Gender_Radio.VerifyTextValidation(gender); // NOT FINDING THIS ELEMENT? Also how can we uncheck the radio?
            Race_Drp.VerifyTextValidation(race);
            MilitaryExperience_Drp.VerifyTextValidation(militaryExperience);
            EmploymentStatus_Drp.VerifyTextValidation(employmentStatus);
            AnnualIncome_Drp.VerifyTextValidation(annualIncome);

            return this;
        }

        public override void WaitForElements()
        {
            SaveChanges_Btn.VerifyVisible(30);
            AnnualIncome_Drp.VerifyPresent(30);
        }
    }
}
