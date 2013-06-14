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
        [XmlData("//Script", FilePath = "TestConfig.xml")]
        public void RunShelterStabilityTests(
            [Bind("@suitePath")] string suitePath,
            [Bind("@scriptName")] string scriptName,
            [Bind("@host")] string host,
            [Bind("@port")] string port,
            [Bind("@repeat")] int repeat)
        {
            for (int i = 0; i < 1; i++)
            {
                string name = scriptName + " : Iteration #" + (i + 1).ToString();
                TestOutcome outcome =  TestStep.RunStep(name, delegate
                    {
                        Common.Log("Test Name : " + name);
                        Assert.Fail("Fail");
                        //EggPlantScript script = new EggPlantTestBaseClass.EggPlantScript(runscriptPath,suitePath, scriptName, host, port);
                        //script.ExecuteScript();
                        //script.VerifySuccess();
                    }, new TimeSpan(0, 0, 10, 0), true, null).Outcome;

                Assert.TerminateSilently(TestOutcome.Inconclusive);
               // if(outcome!=TestOutcome.Passed)
               //     Assert.TerminateSilently(TestOutcome.Failed);


            }
      
        }
    }
}
