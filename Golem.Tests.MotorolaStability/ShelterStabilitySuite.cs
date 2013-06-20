using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CookComputing.XmlRpc;
using Gallio.Common.Markup;
using Gallio.Framework;
using Gallio.Model;
using Golem.Framework;
using MbUnit.Framework;

namespace Golem.Tests.MotorolaStability
{
    public class ShelterStabilitySuite
    {
        private string suitePath;
        private XmlNodeList tests;

        [FixtureSetUp]
        public void Setup()
        {
            
            XmlDocument configFile = new XmlDocument();
            configFile.Load(Directory.GetCurrentDirectory() + "\\TestConfig.xml");
            suitePath = configFile.SelectSingleNode("//Suite/@path").Value;
 
            tests = configFile.SelectNodes("//Test");
            
            if (Directory.Exists(suitePath + "\\Results"))
             Directory.Delete(suitePath + "\\Results",true);
        }


        [Test]
        [MultipleAsserts]
        [Timeout(0)]
        [Repeat(5)]
        public void RunShelterStabilityTests()
        {
            TestOutcome outcome = TestOutcome.Inconclusive;

            foreach (XmlNode test in tests)
            {
                int retry = int.Parse(test.SelectSingleNode("@retry").Value);
                int repeat = int.Parse(test.SelectSingleNode("@repeat").Value);

                for (var i = 0; i < repeat; i++)
                {

                    XmlNodeList scripts = test.ChildNodes;
                    foreach (XmlNode script in scripts)
                    {
                        string scriptName = script.SelectSingleNode("@scriptName").Value;
                        string host = script.SelectSingleNode("@host").Value;
                        string port = script.SelectSingleNode("@port").Value;
                        int timeout = int.Parse(script.SelectSingleNode("@timeout").Value);
                        string name = "";
                        Gallio.Common.Action executeTest = new Gallio.Common.Action(delegate
                        {

                            EggPlantScript eggplant = new EggPlantScript(suitePath, scriptName, host, port, timeout);
                            eggplant.ExecuteScript();
                            eggplant.VerifySuccess();
                            
                        });
                        name = scriptName + " : Iteration #" + (i + 1).ToString();

                        outcome = TestStep.RunStep(name, executeTest, new TimeSpan(0, 0, timeout, 0), true, null).Outcome;

                    }

                    for (var j = 0; (j < retry) && (outcome != TestOutcome.Passed); j++)
                    {
                        foreach (XmlNode script in scripts)
                        {
                            string scriptName = script.SelectSingleNode("@scriptName").Value;
                            string host = script.SelectSingleNode("@host").Value;
                            string port = script.SelectSingleNode("@port").Value;
                            int timeout = int.Parse(script.SelectSingleNode("@timeout").Value);
                            string name = "";

                            //retry test if failed
                            name = scriptName + " : Iteration #" + (i + 1).ToString() + " Retry : " + (j + 1).ToString();
                            Gallio.Common.Action executeTest = new Gallio.Common.Action(delegate
                            {

                                EggPlantScript eggplant = new EggPlantScript(suitePath, scriptName, host, port, timeout);
                                 eggplant.ExecuteScript();
                                eggplant.VerifySuccess();
                            });
                            outcome =
                                TestStep.RunStep(name, executeTest, new TimeSpan(0, 0, timeout, 0), true, null).Outcome;
                        }

                    }

                }
                if(outcome!=TestOutcome.Passed)
                    Assert.TerminateSilently(TestOutcome.Failed);
            }
        }
    }
}
