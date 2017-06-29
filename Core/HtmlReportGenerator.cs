using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using ResultState = NUnit.Framework.Interfaces.ResultState;

namespace Golem.Core
{
    public class HtmlReportGenerator
    {
        public StringWriter stringWriter;
        public HtmlTextWriter htmlTextWriter;

        public HtmlReportGenerator()
        {
            this.stringWriter = new StringWriter();
            this.htmlTextWriter = new HtmlTextWriter(stringWriter);

        }

        public string ConvertPathToRelative(string path)
        {
            path = path.Replace("file:///", "");

            var reportpath = Config.settings.reportSettings.reportPath;
            var relative_path = path.Replace(reportpath, "").Replace(reportpath.Replace("\\", "/"), "");

            if (relative_path.StartsWith("\\"))
            {
                relative_path = relative_path.Replace("\\", "/");
            }

            return "." + relative_path;
        }

        public void GenerateHtml()
        {
            GenerateStartTags();
            GenerateSuiteBody();
            GenerateEndTags();
        }

        public void WriteToFile()
        {
            var path = $"{Config.settings.reportSettings.reportPath}\\{Common.GetCurrentTestName()}.html".Replace("\\", "/");
            var fullPath = Path.GetFullPath(path).Replace("\\", "/");
            var css_path = $"{Common.GetCodeDirectory()}\\dashboard.css";
            var final_path = $"{Config.settings.reportSettings.reportPath}\\dashboard.css";

            if (!File.Exists(Path.GetFullPath(final_path)))
            {
                File.Copy(css_path, Path.GetFullPath(final_path));
            }

            var teamcity = Environment.GetEnvironmentVariable("TEAMCITY_VERSION");
            if (teamcity != null)
            {
                TestContext.WriteLine("url(file:///" + fullPath + ")");
            }
            else
            {
                TestContext.WriteLine(@"file:///" + fullPath);
            }

            File.WriteAllText(fullPath, this.stringWriter.ToString());
            TestBase.testData.ReportPath = fullPath;
        }

        public void GenerateStartTags()
        {
            this.htmlTextWriter.WriteLine("<!DOCTYPE html>");
            this.htmlTextWriter.WriteLine("<head>");
            this.htmlTextWriter.WriteLine("<meta charset=\"utf-8\">");
            this.htmlTextWriter.WriteLine("<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">");
            this.htmlTextWriter.WriteLine("<meta name = \"viewport\" content = \"width=device-width, initial-scale=1\" > ");
            this.htmlTextWriter.WriteLine("<meta name=\"description\" content=\"\">");
            this.htmlTextWriter.WriteLine("<meta name=\"author\" content=\"\">");
            this.htmlTextWriter.WriteLine("<title>Test Dashboard</title>");
            this.htmlTextWriter.WriteLine("<link rel=\"stylesheet\" href=\"http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/css/bootstrap.min.css\">");
            this.htmlTextWriter.WriteLine("<link href=\"dashboard.css\" rel=\"stylesheet\">");
        //    this.htmlTextWriter.WriteLine("<script src=\"http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/ie-emulation-modes-warning.js\"></script>");
            this.htmlTextWriter.WriteLine("</head>");
            this.htmlTextWriter.WriteBeginTag("body");
        }

        public void GenerateSuiteHeader(string name)
        {
            this.htmlTextWriter.WriteLine("<div border='1'>");
        }

        public void GenerateSuiteFooter()
        {
            this.htmlTextWriter.WriteLine("</div>");
        }

        public void GenerateLogHeader()
        {
            this.htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Class, "table-responsive");
            this.htmlTextWriter.RenderBeginTag("div");

            this.htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Class, "table table-striped log-table");
            this.htmlTextWriter.RenderBeginTag("table");

            this.htmlTextWriter.RenderBeginTag("thead");
            this.htmlTextWriter.RenderBeginTag("tr");

            this.htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Class, "log-timestamp");
            this.htmlTextWriter.RenderBeginTag("th");
            this.htmlTextWriter.Write("Timestamp");
            this.htmlTextWriter.RenderEndTag();

            this.htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Class, "log-text");
            this.htmlTextWriter.RenderBeginTag("th");
            this.htmlTextWriter.Write("Text");
            this.htmlTextWriter.RenderEndTag();

            this.htmlTextWriter.RenderEndTag();
            this.htmlTextWriter.RenderEndTag();

            this.htmlTextWriter.RenderBeginTag("tbody");

        }

        public void GenerateIndexHead()
        {
            this.htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Class, "sub-header");
            this.htmlTextWriter.RenderBeginTag("h2");
            this.htmlTextWriter.Write("Index");
            this.htmlTextWriter.RenderEndTag();

            this.htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Class, "table-responsive");
            this.htmlTextWriter.RenderBeginTag("div");

            this.htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Class, "table table-striped log-table");
            this.htmlTextWriter.RenderBeginTag("table");

            this.htmlTextWriter.RenderBeginTag("thead");
            this.htmlTextWriter.RenderBeginTag("tr");

            this.htmlTextWriter.RenderBeginTag("td");
            this.htmlTextWriter.Write("Name");
            this.htmlTextWriter.RenderEndTag();

            this.htmlTextWriter.RenderBeginTag("td");
            this.htmlTextWriter.Write("Status");
            this.htmlTextWriter.RenderEndTag();

            this.htmlTextWriter.RenderEndTag();
            this.htmlTextWriter.RenderEndTag();

            this.htmlTextWriter.RenderBeginTag("tbody");

        }

        public void GenerateIndexRow(string name, string url, string status, string message)
        {
            if (Config.settings.reportSettings.relativePath)
            {
                url = ConvertPathToRelative(url);
            }

            this.htmlTextWriter.RenderBeginTag("tr");

            this.htmlTextWriter.RenderBeginTag("td");
            this.htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Href, url);
            this.htmlTextWriter.RenderBeginTag("a");

            this.htmlTextWriter.Write(name);
            this.htmlTextWriter.RenderEndTag(); //a
            this.htmlTextWriter.RenderEndTag(); //td

            this.htmlTextWriter.RenderBeginTag("td");
            this.htmlTextWriter.Write(status);
            this.htmlTextWriter.RenderEndTag(); //td

            this.htmlTextWriter.RenderBeginTag("td");
            this.htmlTextWriter.Write(message);
            this.htmlTextWriter.RenderEndTag(); //td

            this.htmlTextWriter.RenderEndTag(); //tr
        }

        public void GenerateLogStatus(string statusMessage, string exceptionMessage, string stackTrace, string screenshotPath, string videoPath)
        {
            screenshotPath = ConvertPathToRelative(screenshotPath);
            videoPath = ConvertPathToRelative(videoPath);
            this.htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Class, "log-status");
            this.htmlTextWriter.RenderBeginTag("div");

            this.htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Class, "status");
            this.htmlTextWriter.RenderBeginTag("div");
            this.htmlTextWriter.RenderBeginTag("h2");
            this.htmlTextWriter.Write($"{TestBase.testData.ClassName} {TestBase.testData.MethodName} {TestBase.testData.Status}");
            this.htmlTextWriter.RenderEndTag();
            this.htmlTextWriter.RenderEndTag();

            this.htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Class, "exception-message");
            this.htmlTextWriter.RenderBeginTag("div");
            this.htmlTextWriter.Write(this.escape_text(exceptionMessage));
            this.htmlTextWriter.RenderEndTag();

            this.htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Class, "exception-stacktrace");
            this.htmlTextWriter.RenderBeginTag("div");
            this.htmlTextWriter.Write(this.escape_text(stackTrace));
            this.htmlTextWriter.RenderEndTag();

            if (TestBase.testData.Result.Outcome.Status != TestStatus.Passed)
            {
                this.htmlTextWriter.RenderBeginTag("div");

                this.htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Class, "screenshot");
                this.htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Src, screenshotPath);
                this.htmlTextWriter.RenderBeginTag("img");
                this.htmlTextWriter.RenderEndTag();

                this.htmlTextWriter.RenderEndTag();
            }
           

            this.htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Href, videoPath);
            this.htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Class, "log-video");
            this.htmlTextWriter.RenderBeginTag("a");
            this.htmlTextWriter.WriteLine("Video.flv");
            this.htmlTextWriter.RenderEndTag();

            this.htmlTextWriter.RenderEndTag();

        }

        public void GenerateLogEnd()
        {
            this.htmlTextWriter.RenderEndTag();//tbody
            this.htmlTextWriter.RenderEndTag();//table
            this.htmlTextWriter.RenderEndTag();//div    
        }

        public string escape_text(string text)
        {
            try
            {
                return text.Replace("<", "&lt;").Replace(">", "&gt;");
            }
            catch (Exception)
            {
                return "";
            }
            
        }

        public void GenerateLogMessage(string timestamp, string value)
        {
            GenerateLogRow(timestamp,value);
        }

        public void GenerateLogError(string timestamp, string value)
        {
            GenerateLogRow(timestamp, value,"error");
        }

        public void GenerateLogWarning(string timestamp, string value)
        {
            GenerateLogRow(timestamp, value, "warning");
        }

        public void GenerateLogRow(string timestamp, string value, string type="message")
        {

            this.htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Class, "log-" + type);
            this.htmlTextWriter.RenderBeginTag("tr");

            this.htmlTextWriter.RenderBeginTag("td");
            this.htmlTextWriter.RenderBeginTag("div");
            this.htmlTextWriter.Write(timestamp);
            this.htmlTextWriter.RenderEndTag();
            this.htmlTextWriter.RenderEndTag();

            this.htmlTextWriter.RenderBeginTag("td");
            this.htmlTextWriter.Write(this.escape_text(value));
            this.htmlTextWriter.RenderEndTag();

            this.htmlTextWriter.RenderEndTag();
        }

        public void GenerateLogImage(string timestamp, string path)
        {
            path = ConvertPathToRelative(path);
            this.htmlTextWriter.RenderBeginTag("tr");

            this.htmlTextWriter.RenderBeginTag("td");
            this.htmlTextWriter.Write(timestamp);
            this.htmlTextWriter.RenderEndTag();
            
            this.htmlTextWriter.RenderBeginTag("td");
            this.htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Class, "log-image");
            this.htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Width, "75%");
            this.htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Src, path);
            this.htmlTextWriter.RenderBeginTag(HtmlTextWriterTag.Img);
            this.htmlTextWriter.RenderEndTag();
            this.htmlTextWriter.RenderEndTag();
            
            this.htmlTextWriter.RenderEndTag();
        }

        public void GenerateLogVideo(string timestamp, string path)
        {
            path = ConvertPathToRelative(path);
            this.htmlTextWriter.RenderBeginTag("tr");

            this.htmlTextWriter.RenderBeginTag("td");
            this.htmlTextWriter.Write(timestamp);
            this.htmlTextWriter.RenderEndTag();

            this.htmlTextWriter.RenderBeginTag("td");
            this.htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Href, path);
            this.htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Class, "log-video");
            this.htmlTextWriter.RenderBeginTag("a");
            this.htmlTextWriter.WriteLine("Video.flv");
            this.htmlTextWriter.RenderEndTag();
            this.htmlTextWriter.RenderEndTag();

            this.htmlTextWriter.RenderEndTag();
        }

        public void GenerateLogLink(string timestamp, string path)
        {
            path = ConvertPathToRelative(path);
            this.htmlTextWriter.RenderBeginTag("tr");

            this.htmlTextWriter.RenderBeginTag("td");
            this.htmlTextWriter.Write(timestamp);
            this.htmlTextWriter.RenderEndTag();

            this.htmlTextWriter.RenderBeginTag("td");
            this.htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Class, "log-link");
            this.htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Href, path);
            this.htmlTextWriter.RenderBeginTag("a");
            this.htmlTextWriter.WriteLine(path);
            this.htmlTextWriter.RenderEndTag();
            this.htmlTextWriter.RenderEndTag();

            this.htmlTextWriter.RenderEndTag();
        }


        public void GenerateSuiteBody()
        {
            this.htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Class, "sub-header");
            this.htmlTextWriter.RenderBeginTag("h2");
            this.htmlTextWriter.Write("Log");
            this.htmlTextWriter.RenderEndTag();

            this.htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Class, "table-responsive");
            this.htmlTextWriter.RenderBeginTag("div");


            this.htmlTextWriter.AddAttribute(HtmlTextWriterAttribute.Class, "table table-striped");
            this.htmlTextWriter.RenderBeginTag("table");

            this.htmlTextWriter.RenderBeginTag("thead");
            this.htmlTextWriter.RenderBeginTag("tr");

            this.htmlTextWriter.RenderBeginTag("td");
            this.htmlTextWriter.Write("#");
            this.htmlTextWriter.RenderEndTag();

            this.htmlTextWriter.RenderBeginTag("td");
            this.htmlTextWriter.Write("Name");
            this.htmlTextWriter.RenderEndTag();

            this.htmlTextWriter.RenderBeginTag("td");
            this.htmlTextWriter.Write("Status");
            this.htmlTextWriter.RenderEndTag();

            this.htmlTextWriter.RenderBeginTag("td");
            this.htmlTextWriter.Write("Pass/Fail");
            this.htmlTextWriter.RenderEndTag();

            this.htmlTextWriter.RenderEndTag();
            this.htmlTextWriter.RenderEndTag();

            this.htmlTextWriter.RenderBeginTag("tbody");
            this.htmlTextWriter.RenderBeginTag("tr");

            this.htmlTextWriter.RenderBeginTag("td");
            this.htmlTextWriter.Write("1");
            this.htmlTextWriter.RenderEndTag();

            this.htmlTextWriter.RenderBeginTag("td");
            this.htmlTextWriter.Write("TestClass1");
            this.htmlTextWriter.RenderEndTag();

            this.htmlTextWriter.RenderBeginTag("td");
            this.htmlTextWriter.Write("Pass");
            this.htmlTextWriter.RenderEndTag();

            this.htmlTextWriter.RenderBeginTag("td");
            this.htmlTextWriter.Write("10/10");
            this.htmlTextWriter.RenderEndTag();

            this.htmlTextWriter.RenderEndTag();//tr
            this.htmlTextWriter.RenderEndTag();//tbody

            this.htmlTextWriter.RenderEndTag();//table
            this.htmlTextWriter.RenderEndTag();//div        
          }

        public void GenerateEndTags()
        {
            this.htmlTextWriter.WriteLine("<script src=\"https://ajax.googleapis.com/ajax/libs/jquery/1.11.3/jquery.min.js\"></script>");
           // this.htmlTextWriter.WriteLine("<script> window.jQuery || document.write('<script src=\"http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/vendor/jquery.min.js\"></script>') </script>");
            this.htmlTextWriter.WriteLine("<script src=\"http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/bootstrap.min.js\"></script>");
         //   this.htmlTextWriter.WriteLine("<script src=\"http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/vendor/holder.min.js\"></script>");
         //   this.htmlTextWriter.WriteLine("<script src=\"http://maxcdn.bootstrapcdn.com/bootstrap/3.3.6/js/ie10-viewport-bug-workaround.js\"></script>");
            this.htmlTextWriter.WriteLine("</body>");
            this.htmlTextWriter.WriteLine("</html>");
        }

        public void WriteToIndexFile()
        {
            var items = TestBase.testDataCollection;
            var path = $"{Config.settings.reportSettings.reportPath}\\index.html";
            var css_path = $"{Common.GetCodeDirectory()}\\dashboard.css";
            var final_path = $"{Config.settings.reportSettings.reportPath}\\dashboard.css";
            if (!File.Exists(final_path)) File.Copy(css_path, final_path);
            TestContext.WriteLine(@"file:\\\" + path);
            File.WriteAllText(path, this.stringWriter.ToString());
           
        }

        public void GenerateIndexSummary()
        {
            int totalTests = TestBase.testDataCollection.Count(X => X.Value.Result != null);
            int totalPassed = TestBase.testDataCollection.Where(x => x.Value.Result != null && x.Value.Result.Outcome == ResultState.Success).Count();
            int totalFailed = TestBase.testDataCollection.Where(x => x.Value.Result != null && x.Value.Result.Outcome == ResultState.Failure).Count();
            int totalSkipped = TestBase.testDataCollection.Where(x => x.Value.Result != null && x.Value.Result.Outcome == ResultState.Skipped).Count();
            this.htmlTextWriter.RenderBeginTag("div");
            if (totalPassed == totalTests)
            {
                this.htmlTextWriter.AddAttribute("class","success");
                this.htmlTextWriter.RenderBeginTag("h2");
                this.htmlTextWriter.Write($"SUCCESS!  All tests passed");
                this.htmlTextWriter.RenderEndTag();
            }
            else
            {
                this.htmlTextWriter.AddAttribute("class", "failure");
                this.htmlTextWriter.RenderBeginTag("h2");
                this.htmlTextWriter.Write($"FAILURE! {(float)totalPassed/(float)totalTests}% {totalPassed}/{totalTests} tests passed ({totalSkipped} Skipped)");
                this.htmlTextWriter.RenderEndTag();
            }
            this.htmlTextWriter.RenderEndTag();
            
            // number of tests pass / skip / total
            // graph and % pass
            //
        }
    }
}
