using System.IO;
using Golem.Framework;
using Golem.PageObjects.Emcon.FMX;
using MbUnit.Framework;

namespace Emcon.FMXTests
{
    internal class Job_DocumentsTab : TestBaseClass
    {
        private string pathToFile = Directory.GetCurrentDirectory() + "\\sampledoc.docx";
        private static string envUrl = Config.GetConfigValue("EnvironmentUrl", "http://demo.fmx.bz/");

        [Test(Order = 6)]
        [TestsOn("6 - Documents")]
        public void Test_Job_Documents_Tab()
        {
            FmXwelcomePage.OpenFMX(envUrl)
                          .Login("PROTOTEST", "!TEST1234")
                          .ClickJobs()
                          .SearchJobs()
                          .DynamicSearch("Emcon","Completed","Blue Team")
                          .CloseSearchPopUp()
                          .SelectCompletedJob()
                          .ClickDocumentsTab()
                          .ClickUploadDocument()
                          .UploadDocument("Client Submitted", "Viewable By All", "This is a sample document", pathToFile);


        }
    }
}
