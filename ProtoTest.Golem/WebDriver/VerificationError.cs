using System.Drawing;

namespace Golem.WebDriver
{
    /// <summary>
    ///     Holds an error message and a screenshot.
    /// </summary>
    public class VerificationError
    {
        public string errorText;
        public Image screenshot;

        public VerificationError(string errorText, bool takeScreenshot)
        {
            this.errorText = errorText;
            if (takeScreenshot)
            {
                screenshot = WebDriverTestBase.driver.GetScreenshot();
            }
        }

        public VerificationError(string errorText, Image screenshot)
        {
            this.errorText = errorText;
            this.screenshot = screenshot;
        }
    }
}