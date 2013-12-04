using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ProtoTest.Golem.WebDriver;
using MbUnit.Framework;
using OpenQA.Selenium;

namespace Golem.PageObjects.Cael
{
    public class CreateUserPage : BasePageObject
    {
        public LoggedOutHeader Header = new LoggedOutHeader();
        public Element EmailField = new Element("Email Field", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_CreateAccount_emailTextBox"));
        public Element PasswordField = new Element("Password Field", By.Id("passStrength"));
        public Element VerifyPasswordField = new Element("Verify Password Field", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_CreateAccount_confirmPasswordTextBox"));
        public Element FirstNameField = new Element("First Name Field", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_CreateAccount_fnameTextBox"));
        public Element LastNameField = new Element("Last Name Field", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_CreateAccount_lnameTextBox"));

        public Element AddressField = new Element("Address Field", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_CreateAccount_addressTextBox1"));
        public Element AddressField2 = new Element("AddressField2", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_CreateAccount_addressTextBox2"));
        public Element CityField = new Element("CityField", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_CreateAccount_cityTextBox"));
        public Element StateDropdown = new Element("StateDropdown", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_CreateAccount_stateDDList"));
        public Element ZipCodeField = new Element("ZipCodeField", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_CreateAccount_zipTextBox"));
        public Element PhoneNumberField = new Element("PhoneNumberField", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_CreateAccount_phoneTextBox"));
        public Element DOBDropdown_Year = new Element("DOBDropdown_Year", By.Id("yearDDList"));
        public Element DOBDropdown_Month = new Element("DOBDropdown_Month", By.Id("monthDDList"));
        public Element DOBDropdown_Day = new Element("DOBDropdown_Day", By.Id("dayDDList"));
        public Element CreateAccountButton = new Element("Create Account Button", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LC_CreateAccount_createButton"));



        public void CreateUser(string email, string password, string firstName, string lastName, string address1,
            string address2, string city, string state, string zip, string phone, string DOB_Month, string DOB_Day,
            string DOB_Year)
        {
            if(null != email) EmailField.Text = email;
            PasswordField.Text = password;
            VerifyPasswordField.Text = password;
            if (null != firstName) FirstNameField.Text = firstName;
            if (null != lastName) LastNameField.Text = lastName;
            AddressField.Text = address1;
            AddressField2.Text = address2;
            CityField.Text = city;
            StateDropdown.SelectOption(state);
            ZipCodeField.Text = zip;
            PhoneNumberField.Text = phone;
            DOBDropdown_Month.SelectOption(DOB_Month);
            DOBDropdown_Day.SelectOption(DOB_Day);
            DOBDropdown_Year.SelectOption(DOB_Year);
            CreateAccountButton.Click();
        }

        public override void WaitForElements()
        {
            EmailField.Verify().Visible();
            CreateAccountButton.Verify().Visible();
        }
    }
}
