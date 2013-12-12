using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Golem.PageObjects.Cael;
using MbUnit.Framework;
using ProtoTest.Golem.WebDriver;
using ProtoTest.Golem.WebDriver.Elements.Types;
using ProtoTest.Golem.Core;
using ProtoTest.Golem.WebDriver.Elements.Validation;

namespace Golem.PageObjects.Cael
{
    public class ResetPasswordPage : BasePageObject
    {
        public LoggedOutHeader header = new LoggedOutHeader();

        public ValidationElement PasswordText_Field = new ValidationElement("Password Text Field",
            By.Id("passStrength"),
            By.XPath("//*[@class='passStrengthformError parentFormform formError']/div"));
        
        public ValidationElement PasswordConfirmText_Field = new ValidationElement("Password Confirm Text Field",
            By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LCResetPassword1_txtConfirmPassword"),
            By.XPath("//*[@class='p_lt_ctl02_pageplaceholder_p_lt_ctl00_LCResetPassword1_txtConfirmPasswordformError parentFormform formError']/div"));

        public Element FormErrorLabel = new Element("Error label", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LCResetPassword1_rfvConfirmPassword"));
        public Button ResetPassword_Button = new Button("Password Reset button", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LCResetPassword1_btnReset"));

        public ResetPasswordPage VerifyFormValidations()
        {
            string item_error = "* This field is required";
            string form_error = "Please enter a value.";

            ResetPassword_Button.Click();
            PasswordText_Field.VerifyTextValidation(item_error);
            PasswordConfirmText_Field.VerifyTextValidation(item_error);
            FormErrorLabel.Verify().Visible().Verify().Text(form_error);
            
            return this;
        }

        public void ResetPassword(string password)
        {
            PasswordText_Field.Text = password;
            PasswordConfirmText_Field.Text = password;
            ResetPassword_Button.Click();

            // I don't want to create another page to verify the notification
            WebDriverTestBase.driver.FindElement(OpenQA.Selenium.By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_LCResetPassword1_plcMess_lS")).Verify().Visible().Verify().Text("Password has been reset.");

        }

        public override void WaitForElements()
        {
            PasswordText_Field.Verify().Visible();
            PasswordConfirmText_Field.Verify().Visible();
            ResetPassword_Button.Verify().Visible();
        }
    }
}
