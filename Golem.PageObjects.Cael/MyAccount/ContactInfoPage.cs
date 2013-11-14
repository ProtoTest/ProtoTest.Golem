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
    public class ContactInfoPage : MyAccountPage
    {
        public ValidationElement FirstNameFIeld = new ValidationElement("First Name Field",
                                                              By.Id("fnameTextBox"),
                                                              By.XPath("//*[@class='fnameTextBoxformError parentFormform formError']/div"));
        public ValidationElement LastNameField = new ValidationElement("Last Name Field",
                                                                       By.Id("lnameTextBox"),
                                                                       By.XPath("//*[@class='lnameTextBoxformError parentFormform formError']/div"));
        public ValidationElement DOB_Month_Dropdown = new ValidationElement("DOB_Month Dropdown",
                                                                            By.Id("monthDDList"),
                                                                            By.XPath("//*[@class='monthDDListformError parentFormform formError']/div"));
        public ValidationElement DOB_Day_Dropdown = new ValidationElement("DOB_Day_Dropdown",
                                                                          By.Id("dayDDList"),
                                                                          By.XPath("//*[@class='dayDDListformError parentFormform formError']/div"));
        public ValidationElement DOB_Year_Dropdown = new ValidationElement("DOB_Year dropdown",
                                                                           By.Id("yearDDList"),
                                                                           By.XPath("//*[@class='yearDDListformError parentFormform formError']/div"));
        public ValidationElement Address1Field = new ValidationElement("Address1 Field",
                                                                       By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_addressTextBox1"),
                                                                       By.XPath("//*[@class='p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_addressTextBox1formError parentFormform formError']/div"));
        public Element Address2Field = new Element("Address 2 Field",
                                                   By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_addressTextBox2"));
        public ValidationElement CityField = new ValidationElement("City FIeld",
                                                                   By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_cityTextBox"),
                                                                   By.XPath("//*[@class='p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_cityTextBoxformError parentFormform formError']/div"));
        public ValidationElement StateDropdown = new ValidationElement("State Dropdown",
                                                                       By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_stateDDList"),
                                                                       By.XPath("//*[@class='p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_stateDDListformError parentFormform formError']/div"));
        public ValidationElement ZipField = new ValidationElement("Zip Field",
                                                                  By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_zipTextBox"),
                                                                  By.XPath("//*[@class='yearDDListformError parentFormform formError']/div"));
        public ValidationElement PhoneField = new ValidationElement("Phone Field",
                                                                    By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_phoneTextBox"),
                                                                    By.XPath("//*[@class='p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_zipTextBoxformError parentFormform formError']/div"));
        public Element SaveButton = new Element("", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_saveContactButton"));
        public Element SaveChangesComplete = new Element("Saved!", By.Id("lblSave1"));

        public ContactInfoPage EnterContactInfo(string fName, string lName, string DOB_Month, string DOB_Day,
            string DOB_Year, string addr1, string addr2, string city, string state, string zip, string phone, string savedCompletedText = "Saved!")
        {
            FirstNameFIeld.Text = fName;
            LastNameField.Text = lName;
            DOB_Month_Dropdown.SelectOption(DOB_Month);
            DOB_Day_Dropdown.SelectOption(DOB_Day);
            DOB_Year_Dropdown.SelectOption(DOB_Year);
            Address1Field.Text = addr1;
            Address2Field.Text = addr2;
            CityField.Text = city;
            StateDropdown.SelectOption(state);
            ZipField.Text = zip;
            PhoneField.Text = phone;
            SaveButton.Click();
            SaveChangesComplete.Verify().Visible().Verify().Text(savedCompletedText);
            return new ContactInfoPage();
        }

        public ContactInfoPage VerifyContactInfo(string fName, string lName, string DOB_Month, string DOB_Day,
            string DOB_Year, string addr1, string addr2, string city, string state, string zip, string phone)
        {
            FirstNameFIeld.Verify().Value(fName);
            LastNameField.Verify().Value(lName);
            DOB_Month_Dropdown.Verify().Text(DOB_Month);
            DOB_Day_Dropdown.Verify().Text(DOB_Day);
            DOB_Year_Dropdown.Verify().Text(DOB_Year);
            Address1Field.Verify().Value(addr1);
            Address2Field.Verify().Value(addr2);
            CityField.Verify().Value(city);
            StateDropdown.Verify().Text(state);
            ZipField.Verify().Value(zip);
            PhoneField.Verify().Value(phone);
            return this;
        }

        public ContactInfoPage VerifyContactInfoFormValidations(string fNameError, string lNameError,
            string DOB_MonthError, string DOB_DayError, string DOB_YearError, string addr1Error,
            string cityError, string stateError, string zipError, string phoneError)
        {
            FirstNameFIeld.VerifyTextValidation(fNameError);
            LastNameField.VerifyTextValidation(lNameError);
            DOB_Month_Dropdown.VerifyTextValidation(DOB_MonthError);
            DOB_Day_Dropdown.VerifyTextValidation(DOB_DayError);
            DOB_Year_Dropdown.VerifyTextValidation(DOB_YearError);
            Address1Field.VerifyTextValidation(addr1Error);
            CityField.VerifyTextValidation(cityError);
            StateDropdown.VerifyTextValidation(stateError);
            ZipField.VerifyTextValidation(zipError);
            PhoneField.VerifyTextValidation(phoneError);

            return this;
        }
        public override void WaitForElements()
        {
            SaveButton.Verify().Visible();
            FirstNameFIeld.Verify().Visible();
        }
    }
}
