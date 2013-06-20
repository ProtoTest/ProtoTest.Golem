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
        private string suitePath;
        private XmlNodeList tests;

        [FixtureSetUp]
        public void Setup()
        {

            XmlDocument configFile = new XmlDocument();
            configFile.Load(Directory.GetCurrentDirectory() + "\\TestConfig.xml");

            suitePath = configFile.SelectSingleNode("//Suite/@path").Value;
            tests  = configFile.SelectNodes("//Test");

            if(Directory.Exists(suitePath + "\\Results"))
                Directory.Delete(suitePath + "\\Results",true);
        }

        [Test]
        [MultipleAsserts]
        [Timeout(0)]
        [Repeat(1)]
        public void RunShelterStabilityTests()
        {
            string scriptName;
            string host;
            string port;
            int timeout;
            int repeat;
            int retry;
            bool testFailed = false;
            TestSuite suite = new TestSuite("Suite 1");
            foreach (XmlNode test in tests)
            {
                XmlNodeList scripts = test.ChildNodes;
                testFailed = false;
                repeat = int.Parse(test.SelectSingleNode("@repeat").Value);
                retry = int.Parse(test.SelectSingleNode("@retry").Value);
                TestOutcome outcome = TestOutcome.Inconclusive;

                //repeat x times
                for (int i = 0; i < repeat; i++)
                {
                        foreach (XmlNode script in scripts)
                        {
                            scriptName = script.SelectSingleNode("@scriptName").Value;
                            host = script.SelectSingleNode("@host").Value;
                            port = script.SelectSingleNode("@port").Value;
                            timeout = int.Parse(script.SelectSingleNode("@timeout").Value);//min 

                            Gallio.Common.Action executeTest = new Gallio.Common.Action(delegate
                                {
                                    EggPlantScript eggplant = new EggPlantScript(suitePath, scriptName, host,port);
                                     eggplant.ExecuteScript();
                                     eggplant.VerifySuccess();
                                });

                            string name = scriptName + " : Iteration #" + (i + 1).ToString();
                            outcome = TestStep.RunStep(name, executeTest, new TimeSpan(0, 0, timeout, 0), true, null).Outcome;
                            if (outcome != TestOutcome.Passed)
                            {
                                testFailed = true;
                            }

                        }
                    if(testFailed==true)
                    {
                        for(var j=0;((j<retry)&&(outcome!=TestOutcome.Passed));j++)
                        {
                            foreach (XmlNode script in scripts)
                            {
                                scriptName = script.SelectSingleNode("@scriptName").Value;
                                host = script.SelectSingleNode("@host").Value;
                                port = script.SelectSingleNode("@port").Value;
                                timeout = int.Parse(script.SelectSingleNode("@timeout").Value);

                                Gallio.Common.Action executeTest = new Gallio.Common.Action(delegate
                                    {
                                        EggPlantScript eggplant = new EggPlantScript(suitePath, scriptName, host,port);
                                         eggplant.ExecuteScript();
                                         eggplant.VerifySuccess(); 
                                    });

                                string name = scriptName + " : Iteration #" + (i + 1).ToString() + " : Retry : " + (j+1).ToString();
                                outcome =
                                    TestStep.RunStep(name, executeTest, new TimeSpan(0, 0, timeout, 0), true, null)
                                            .Outcome;
                                if (outcome != TestOutcome.Passed)
                                {
                                    testFailed = true;
                                }
                            }
                        }

                    }


                }


            }

            if(testFailed==true)
                Assert.TerminateSilently(TestOutcome.Failed);
        }
    }
}
