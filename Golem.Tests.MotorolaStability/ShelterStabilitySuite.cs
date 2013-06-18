using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Gallio.Framework;
using Gallio.Model;
using Golem.Framework;
using MbUnit.Framework;

namespace Golem.Tests.MotorolaStability
{
    public class ShelterStabilitySuite
    {
        private string runscriptPath;

        [FixtureSetUp]
        public void Setup()
        {
            
                XmlDocument configFile = new XmlDocument();
                configFile.Load(Directory.GetCurrentDirectory() + "\\TestConfig.xml");
                runscriptPath = configFile.SelectSingleNode("//RunScript/@path").Value;
        }

        [Test]
        [MultipleAsserts]
        [Timeout(0)]
        [Repeat(10)]
        [XmlData("//Script", FilePath = "TestConfig.xml")]
        public void RunShelterStabilityTests(
            [Bind("@suitePath")] string suitePath,
            [Bind("@scriptName")] string scriptName,
            [Bind("@host")] string host,
            [Bind("@port")] string port,
            [Bind("@repeat")] int repeat,
            [Bind("@retry")] int retry,
            [Bind("@timeout")] int timeout)
        {
            Gallio.Common.Action executeTest = new Gallio.Common.Action(delegate
            {
                EggPlantScript script = new EggPlantScript(runscriptPath, suitePath, scriptName, host,
                                                           port);
                script.ExecuteScript();
                script.VerifySuccess();
                
            });
            TestOutcome outcome = TestOutcome.Inconclusive;
            bool testFailed = false;
            for (int i = 0; i < repeat; i++)
            {
                string name = scriptName + " : Iteration #" + (i + 1).ToString();
                outcome = TestStep.RunStep(name, executeTest, new TimeSpan(0, 0, timeout, 0), true, null).Outcome;


                for (var j = 0; (j < retry) && (outcome != TestOutcome.Passed); j++)
                    {
                       name = scriptName + " : Iteration #" + (i + 1).ToString() + " Retry : " + (j + 1).ToString();
                       outcome= TestStep.RunStep(name,executeTest, new TimeSpan(0, 0, timeout, 0), true, null).Outcome;
                    }
      
                if (outcome != TestOutcome.Passed)
                {
                    testFailed = true;
                }
               
            }
           if (testFailed == true)
              Assert.TerminateSilently(TestOutcome.Failed);
        }
    }
}
