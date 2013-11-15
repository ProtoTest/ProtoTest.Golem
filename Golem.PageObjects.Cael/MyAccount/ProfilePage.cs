using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoTest.Golem.WebDriver;
using OpenQA.Selenium;
using ProtoTest.Golem.WebDriver.Elements.Validation;


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
        public Element CollegeCredit_Radio = new Element("College Credit Interest Radio", ByE.PartialAttribute("input", "@id", "investigationRButton_0"));
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
        public Element SaveChangesComplete = new Element("Saved!", By.Id("lblSave2"));


        public ProfilePage EnterProfileInfo(string education, string areaOfStudy, string nameOfCollege, string collegeCreditInvestigated,
            string collegeCreditsEarned, string collegeCreditsNeeded, string onlineLearning, string financialAid,
            string topicOfInterest, string gender, string race, string militaryExperience, string employmentStatus,
            string nameOfEmployer, string tuitionAssitance, string annualIncome, string laborUnion, string receviedTraining,
            string typeOfTraining, string howHear, string savedCompletedText="Saved!")
        {
            if(education != null) EducationAchieved_Drp.SelectOption(education);
            if (areaOfStudy != null) AreaOfStudy_Drp.SelectOption(areaOfStudy);
            if (nameOfCollege != null) NameOfCollege_Drp.SelectOption(nameOfCollege);
            if (collegeCreditInvestigated != null) CollegeCredit_Radio.Click();
            if (collegeCreditsEarned != null) CollegeCreditsEarned_Drp.SelectOption(collegeCreditsEarned);
            if (collegeCreditsNeeded != null) CollegeCreditsNeed_Drp.SelectOption(collegeCreditsNeeded);
            if (onlineLearning != null) InterestedOnline_Radio.Click();
            if (financialAid != null) FinancialAid_Radio.Click();
            if (topicOfInterest != null) TopicOfInterest_Drp.SelectOption(topicOfInterest);
            if (gender != null) Gender_Radio.Click();
            if (race != null) Race_Drp.SelectOption(race);
            if (militaryExperience != null) MilitaryExperience_Drp.SelectOption(militaryExperience);
            if (employmentStatus != null) EmploymentStatus_Drp.SelectOption(employmentStatus);
            if (nameOfEmployer != null) Employer_Field.Text = nameOfEmployer;
            if (tuitionAssitance != null) TuitionAssistance_Radio.Click();
            if (annualIncome != null) AnnualIncome_Drp.SelectOption(annualIncome);
            if (laborUnion != null) LaborUnion_Radio.Click();
            if (receviedTraining != null) ReceivedTraining_Radio.Click();
            if (typeOfTraining != null) NameOrTypeOfTrainnig_Field.Text = typeOfTraining;
            if (howHear != null) HowDidYouHear_Drp.SelectOption(howHear);
            SaveChanges_Btn.Click();
            SaveChangesComplete.Verify().Visible().Verify().Text(savedCompletedText);

            return new ProfilePage();

        }

        public ProfilePage VerifyProfileInfo(string education, string areaOfStudy, string nameOfCollege, string collegeCreditInvestigated,
            string collegeCreditsEarned, string collegeCreditsNeeded, string onlineLearning, string financialAid,
            string topicOfInterest, string gender, string race, string militaryExperience, string employmentStatus,
            string nameOfEmployer, string tuitionAssitance, string annualIncome, string laborUnion, string receviedTraining,
            string typeOfTraining, string howHear)
        {
            EducationAchieved_Drp.Verify().Text(education);
            AreaOfStudy_Drp.Verify().Text(areaOfStudy);
            NameOfCollege_Drp.Verify().Text(nameOfCollege);
            CollegeCredit_Radio.Verify().Selected();
            CollegeCreditsEarned_Drp.Verify().Text(collegeCreditsEarned);
            CollegeCreditsNeed_Drp.Verify().Text(collegeCreditsNeeded);
            InterestedOnline_Radio.Verify().Selected();
            FinancialAid_Radio.Verify().Selected();
            TopicOfInterest_Drp.Verify().Text(topicOfInterest);
            Gender_Radio.Verify().Selected();
            Race_Drp.Verify().Text(race);
            MilitaryExperience_Drp.Verify().Text(militaryExperience);
            EmploymentStatus_Drp.Verify().Text(employmentStatus);
            Employer_Field.Verify().Value(nameOfEmployer);
            TuitionAssistance_Radio.Verify().Selected();
            AnnualIncome_Drp.Verify().Text(annualIncome);
            LaborUnion_Radio.Verify().Selected();
            ReceivedTraining_Radio.Verify().Selected();
            NameOrTypeOfTrainnig_Field.Verify().Text(typeOfTraining);
            HowDidYouHear_Drp.Verify().Text(howHear);

            return this;

        }

        /// <summary>
        ///     Verifies the form error text for each element on the form
        /// </summary>
        /// <param name="education">Education element error text</param>
        /// <param name="nameOfCollege">College element error text</param>
        /// <param name="gender">Gener element error text</param>
        /// <param name="race">Race element error text</param>
        /// <param name="militaryExperience">Military element error text</param>
        /// <param name="employmentStatus">Employment element error text</param>
        /// <param name="annualIncome">Annual Income element error text</param>
        /// <returns></returns>
        public ProfilePage VerifyProfileFormValidations(string education, string nameOfCollege, string gender,
            string race, string militaryExperience, string employmentStatus, string annualIncome)
        {
            EducationAchieved_Drp.VerifyTextValidation(education);
            NameOfCollege_Drp.VerifyTextValidation(nameOfCollege);
            Gender_Radio.VerifyTextValidation(gender);
            Race_Drp.VerifyTextValidation(race);
            MilitaryExperience_Drp.VerifyTextValidation(militaryExperience);
            EmploymentStatus_Drp.VerifyTextValidation(employmentStatus);
            AnnualIncome_Drp.VerifyTextValidation(annualIncome);

            return this;
        }

        public override void WaitForElements()
        {
            SaveChanges_Btn.Verify().Visible();
            //CollegeCredit_Radio.Verify().Visible();
           // AnnualIncome_Drp.Verify().Visible();
        }
    }
}
