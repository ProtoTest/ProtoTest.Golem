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
using Golem.Framework;
using MbUnit.Framework;

namespace Golem.TestRunners.EggPlant
{
    public class EggPlantScriptRunner
    {
        private static string configFilePath = Directory.GetCurrentDirectory() + "\\TestConfig.xml";
        private static Process cmdProcess;
        private string runScriptPath;
        private string suitePath;
        private XmlNodeList tests;
        private string drivePort;
        private IEggPlantDriver driver;

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
        [MultipleAsserts]
        [Timeout(0)]
        public void RunScriptsFromConfigFile()
        {
            string scriptName;
            string host;
            string port;
            int timeout;
            int repeat;
            int retry;
            bool testFailed = false;
            foreach (XmlNode test in tests)
            {
                XmlNodeList scripts = test.ChildNodes;
                testFailed = false;
                repeat = int.Parse(GetValueFromNode("@repeat",test));
                retry = int.Parse(GetValueFromNode("@retry",test));
                TestOutcome outcome = TestOutcome.Inconclusive;

                for (int i = 0; i < repeat; i++)
                {
                        foreach (XmlNode script in scripts)
                        {
                            scriptName = script.SelectSingleNode("@scriptName").Value;
                            host = GetValueFromNode("@host",script);
                            port = GetValueFromNode("@port", script);
                            timeout = int.Parse(GetValueFromNode("@timeout",script));//min 
                            string name = scriptName + " : Iteration #" + (i + 1).ToString();
                            EggPlantScript eScript = new EggPlantScript(driver,suitePath,scriptName,host,port, timeout);
                            outcome = eScript.ExecuteTest(name);
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
                                scriptName = GetValueFromNode("@scriptName", script);
                                host = GetValueFromNode("@host",script);
                                port = GetValueFromNode("@port", script);
                                timeout = int.Parse(GetValueFromNode("@timeout",script));

                                string name = scriptName + " : Iteration #" + (i + 1).ToString() + " : Retry : " + (j+1).ToString();
                                EggPlantScript eScript = new EggPlantScript(driver, suitePath, scriptName, host, port, timeout);
                                outcome = eScript.ExecuteTest(name);
                                if (outcome != TestOutcome.Passed)
                                {
                                    testFailed = true;
                                }
                            }
                        }

                    }


                }
                StopEggPlantDrive();
                StartEggPlantDrive();

            }

            if(testFailed==true)
                Assert.TerminateSilently(TestOutcome.Failed);
            StopEggPlantDrive();
            StartEggPlantDrive();
        }
    

        [FixtureSetUp]
        public void Setup()
        {
            driver = (IEggPlantDriver)XmlRpcProxyGen.Create(typeof(IEggPlantDriver));
            driver.Timeout = 600000;
            GetConfigFileSettings();
            StopEggPlantDrive();
            StartEggPlantDrive();
            StartEggPlantSession();
            DeleteResultsDirectory();
        }



        [FixtureTearDown]
        public void Teardown()
        {
            EndEggPlantSession();
            StopEggPlantDrive();
            DiagnosticLog.WriteLine("Test Finished, exiting!");
        }

        private string GetValueFromConfigFile(string xpath)
        {
            XmlDocument configFile = new XmlDocument();
            if (!File.Exists(configFilePath))
            {
                throw new FileNotFoundException("Could not find xml file at : " + configFilePath);
            }
            configFile.Load(configFilePath);
            if (configFile.SelectNodes(xpath).Count == 0)
                throw new KeyNotFoundException("Could not find an element matching xpath : " + xpath + " in file " + configFilePath);
            return configFile.SelectSingleNode(xpath).Value;

        }

        private XmlNodeList GetNodesFromConfigFile(string xpath)
        {
            XmlDocument configFile = new XmlDocument();
            if (!File.Exists(configFilePath))
            {
                throw new FileNotFoundException("Could not find xml file at : " + configFilePath);
            }
            configFile.Load(configFilePath);
            if (configFile.SelectNodes(xpath).Count == 0)
                throw new KeyNotFoundException("Could not find an element matching xpath : " + xpath + " in file " + configFilePath);
            return configFile.SelectNodes(xpath);

        }

        private string GetValueFromNode(string value, XmlNode node)
        {
            if (node.SelectNodes(value).Count == 0)
            {
                throw new KeyNotFoundException("Could not find an element matching xpath : " + value + " in file " + configFilePath);
            }
            return node.SelectSingleNode(value).Value;
        }

        private void GetConfigFileSettings()
        {
            XmlDocument configFile = new XmlDocument();
            suitePath = GetValueFromConfigFile("//Suite/@path");
            tests = GetNodesFromConfigFile("//Test");
            runScriptPath = GetValueFromConfigFile("//RunScript/@path");
            drivePort = GetValueFromConfigFile("//EggPlantSettings/@drivePort");

            if (!Directory.Exists(suitePath))
                throw new SilentTestException(TestOutcome.Failed, "Could not find suite. Check your TestConfig.xml suite path");
            if (!File.Exists(runScriptPath))
                throw new SilentTestException(TestOutcome.Failed, "Could not find runScript. Check your TestConfig.xml runscript path");
        }

        private void StopEggPlantDrive()
        {
            try
            {
                
                Common.KillProcess("Eggplant");
            }
            catch (Exception e)
            {

            }

        }

        private void EndEggPlantSession()
        {
            try
            {
                driver.EndSession();
            }
            catch (Exception e)
            {
                throw new SilentTestException(TestOutcome.Failed, "Exception Caught Ending EggPLant Session for suite : " + suitePath + e.Message);
            }
        }

        private void StartEggPlantSession()
        {
            try
            {
                Common.Log("Starting eggplant session for suite : " + suitePath);
                driver.StartSession(suitePath);
            }
            catch (Exception e)
            {
                throw new SilentTestException(TestOutcome.Failed, "Exception Caught Starting EggPLant Session for suite : " + suitePath + " Check the log to see if drive started correctly : " + e.Message);
            }

        }

        private void DeleteResultsDirectory()
        {
            if (Directory.Exists(suitePath + "\\Results"))
            {
                Common.Log("Old results found, deleting Eggplant results directory");
                Directory.Delete(suitePath + "\\Results", true);
            }
        }

        public void StartEggPlantDrive()
        {
            try
            {
                Common.Log("Starting Eggplant Drive");
                // string command = "";
                // command += String.Format("\"{0}\" -driveport {1}", runScriptPath, drivePort);
                
                cmdProcess = Common.ExecuteBatchFile(suitePath + "\\RunDrive.bat");
                Thread.Sleep(15000);
            }
            catch (Exception e)
            {
                throw new SilentTestException(TestOutcome.Failed, "Exception Caught Starting EggPLant Drive: " + e.Message);
            }
        }
    }
}
