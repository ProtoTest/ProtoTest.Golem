using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Golem.Framework;
using OpenQA.Selenium;

namespace Golem.PageObjects.Cael.MyAccount
{
    public class ContactInfoPage : MyAccountPage
    {
        public Element FirstNameFIeld = new Element("First Name Field", By.Id("fnameTextBox"));
        public Element LastNameField = new Element("Last Name Field", By.Id("lnameTextBox"));
        public Element DOB_Month_Dropdown = new Element("DOB_Month Dropdown", By.Id("uniform-monthDDList"));
        public Element DOB_Day_Dropdown = new Element("DOB_Day_Dropdown", By.Id("dayDDList"));
        public Element DOB_Year_Dropdown = new Element("DOB_Year dropdown", By.Id("yearDDList"));
        public Element Address1Field = new Element("Address1 Field", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_addressTextBox1"));
        public Element Address2Field = new Element("Address 2 Field", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_addressTextBox2"));
        public Element CityField = new Element("City FIeld", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_cityTextBox"));
        public Element StateDropdown = new Element("State Dropdown", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_stateDDList"));
        public Element ZipField = new Element("Zip Field", By.Id(""));
        public Element PhoneField = new Element("Phone Field", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_phoneTextBox"));
        public Element SaveButton = new Element("", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_saveContactButton"));

        public ContactInfoPage EnterContactInfo(string fName, string lName, string DOB_Month, string DOB_Day,
            string DOB_Year, string addr1, string addr2, string city, string state, string zip, string phone)
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
            return new ContactInfoPage();
        }

        public ContactInfoPage VerifyContactInfo(string fName, string lName, string DOB_Month, string DOB_Day,
            string DOB_Year, string addr1, string addr2, string city, string state, string zip, string phone)
        {
            FirstNameFIeld.VerifyText(fName);
            LastNameField.VerifyText(lName);
            DOB_Month_Dropdown.VerifyText(DOB_Month);
            DOB_Day_Dropdown.VerifyText(DOB_Day);
            DOB_Year_Dropdown.VerifyText(DOB_Year);
            Address1Field.VerifyText(addr1);
            Address2Field.VerifyText(addr2);
            CityField.VerifyText(city);
            StateDropdown.VerifyText(state);
            ZipField.VerifyText(zip);
            PhoneField.VerifyText(phone);
            return this;
        }

        public override void WaitForElements()
        {
            SaveButton.VerifyVisible(30);
            FirstNameFIeld.VerifyVisible(30);
        }
    }
}
