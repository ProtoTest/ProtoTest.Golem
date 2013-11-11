using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using CookComputing.XmlRpc;
using Gallio.Framework;
using Gallio.Model;
using MbUnit.Framework;

namespace ProtoTest.TestRunner.Eggplant
{
    public class EggPlantTestRunner
    {
        private static string configFilePath = Directory.GetCurrentDirectory() + "\\TestConfig.xml";
        private static string batchFilePath = Common.GetValueFromConfigFile(@"//DriveBatchFile/@path");
        private string suitePath = Common.GetValueFromConfigFile("//Suite/@path");
        private XmlNodeList tests = Common.GetNodesFromConfigFile("//Test");
        private int driveTimeoutMs = int.Parse(Common.GetValueFromConfigFile("//EggPlantSettings/@driveTimeoutMs"));
        private int waitForDriveMs = int.Parse(Common.GetValueFromConfigFile("//EggPlantSettings/@waitForDriveMs"));
        private static Process cmdProcess;
        private EggplantDriver driver;
        string scriptName;
        string host;
        string port;
        int timeout;
        int repeat;
        int retry;
        bool testFailed = false;

        [Factory("GetNumRepetitions")]
        public int repetition;

        public static IEnumerable<int> GetNumRepetitions()
        {
            XmlDocument configFile = new XmlDocument();
            configFile.Load(Directory.GetCurrentDirectory() + "\\TestConfig.xml");
            int num = int.Parse(configFile.SelectSingleNode("//Suite/@repeat").Value);
            for (var i = 1; i <= num; i++)
            {

                yield return i;
            }

        }


        [Test]
        [Timeout(0)]
        public void RunScriptsFromConfigFile()
        {
            driver = new EggplantDriver(driveTimeoutMs);
            driver.StopEggPlantDrive();
            driver.StartEggPlantDrive(batchFilePath, waitForDriveMs);
            driver.StartSuiteSession(suitePath);
            Common.DeleteResultsDirectory(suitePath);

            foreach (XmlNode test in tests)
            {
                XmlNodeList scripts = test.ChildNodes;
                testFailed = false;
                repeat = int.Parse(Common.GetValueFromNode("@repeat", test));
                retry = int.Parse(Common.GetValueFromNode("@retry", test));
                TestOutcome outcome = TestOutcome.Inconclusive;

                for (int i = 0; i < repeat; i++)
                {
                    foreach (XmlNode script in scripts)
                    {
                        scriptName = script.SelectSingleNode("@scriptName").Value;
                        host = Common.GetValueFromNode("@host", script);
                        port = Common.GetValueFromNode("@port", script);
                        timeout = int.Parse(Common.GetValueFromNode("@timeout", script));
                        string name = scriptName + " : Iteration #" + (i + 1).ToString();
                        EggplantTest eScript = new EggplantTest(driver, suitePath, scriptName, host, port, timeout);
                        outcome = eScript.ExecuteTest(name);
                        if (outcome != TestOutcome.Passed)
                        {
                            testFailed = true;
                        }

                    }
                    if (testFailed == true)
                    {
                        for (var j = 0; ((j < retry) && (outcome != TestOutcome.Passed)); j++)
                        {
                            foreach (XmlNode script in scripts)
                            {
                                scriptName = Common.GetValueFromNode("@scriptName", script);
                                host = Common.GetValueFromNode("@host", script);
                                port = Common.GetValueFromNode("@port", script);
                                timeout = int.Parse(Common.GetValueFromNode("@timeout", script));

                                string name = scriptName + " : Iteration #" + (i + 1).ToString() + " : Retry : " + (j + 1).ToString();
                                EggplantTest eScript = new EggplantTest(driver, suitePath, scriptName, host, port, timeout);
                                outcome = eScript.ExecuteTest(name);
                                if (outcome != TestOutcome.Passed)
                                {
                                    testFailed = true;
                                }
                            }
                        }

                    }


                }
                driver.EndSuiteSession();
                driver.StopEggPlantDrive();
                driver.StartEggPlantDrive(batchFilePath, waitForDriveMs);
                driver.StartSuiteSession(suitePath);

            }
            driver.EndSuiteSession();
            driver.StopEggPlantDrive();

            if (testFailed == true)
                Assert.TerminateSilently(TestOutcome.Failed);

        }
        [TearDown]
        public void TearDown()
        {
            driver.EndSuiteSession();
            driver.StopEggPlantDrive();
        }

    }
}
   
