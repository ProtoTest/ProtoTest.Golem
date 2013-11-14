using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoTest.Golem.WebDriver;
using OpenQA.Selenium;

namespace Golem.PageObjects.Cael.MyAccount
{
    public class PasswordPage : MyAccountPage
    {
        public Element EmailField = new Element("Email Field", By.Id("passEmailTextBox"));
        public Element PasswordField = new Element("Password Field", By.Id("passTextBox"));
        public Element VerifyField = new Element("Verify Field", By.Id("verifyTextBox"));
        public Element SaveChangesButton = new Element("Save Changes Button", By.Id("p_lt_ctl02_pageplaceholder_p_lt_ctl00_MyAccount_1_savePasswordButton"));
        public Element SaveChangesComplete = new Element("Saved!", By.Id("lblSave3"));

        public PasswordPage UpdateInfo(string email, string password, string savedCompletedText = "Saved!")
        {
            EmailField.SendKeys(email);
            PasswordField.SendKeys(password);
            VerifyField.SendKeys(password);
            SaveChangesButton.Click();
            SaveChangesComplete.Verify().Visible().Verify().Text(savedCompletedText);

            return new PasswordPage();
        }

        public PasswordPage VerifyEmail(string email)
        {
            EmailField.Verify().Text(email);
            return this;
        }

        public override void WaitForElements()
        {
            EmailField.Verify().Visible();
            PasswordField.Verify().Visible();
            VerifyField.Verify().Visible();
            SaveChangesButton.Verify().Visible();
        }


    }
}
