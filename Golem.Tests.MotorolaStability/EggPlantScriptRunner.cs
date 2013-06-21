using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
        private static Process cmdProcess;
        private string runScriptPath;
        private string suitePath;
        private XmlNodeList tests;
        private string drivePort;
        private IEggPlantDriver driver;

        [Factory("GetNumRepetitions")] public int repetition;

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

        [FixtureSetUp]
        public void Setup()
        {
            driver = (IEggPlantDriver)XmlRpcProxyGen.Create(typeof(IEggPlantDriver));
            driver.Timeout = 600000;
            GetConfigFileSettings();
          //  StartEggPlantDrive();
            StartEggPlantSession();
            DeleteResultsDirectory();
        }



        [FixtureTearDown]
        public void Teardown()
        {
            EndEggPlantSession();
           // StopEggPlantDrive();
            DiagnosticLog.WriteLine("Test Finished, exiting!");
        }

        private void GetConfigFileSettings()
        {
            XmlDocument configFile = new XmlDocument();
            configFile.Load(Directory.GetCurrentDirectory() + "\\TestConfig.xml");
            suitePath = configFile.SelectSingleNode("//Suite/@path").Value;
            tests = configFile.SelectNodes("//Test");
            runScriptPath = configFile.SelectSingleNode("//RunScript/@path").Value;
            drivePort = configFile.SelectSingleNode("//EggPlantSettings/@drivePort").Value;

            if (!Directory.Exists(suitePath))
                Assert.Fail("Could not find suite. Check your TestConfig.xml suite path");
            if (!File.Exists(runScriptPath))
                Assert.Fail("Could not find runScript. Check your TestConfig.xml runscript path");
        }

        private void StopEggPlantDrive()
        {
            try
            {
                cmdProcess.Close();
                cmdProcess.Kill();
            }
            catch (Exception)
            {

            }
            
        }

        private void EndEggPlantSession()
        {
            try
            {
                driver.EndSession();
            }
            catch (Exception)
            {
            }
        }

        private void StartEggPlantSession()
        {
            try
            {
                driver.StartSession(suitePath);
            }
            catch (Exception)
            {
                
            }
            
        }

        private void DeleteResultsDirectory()
        {

            if (Directory.Exists(suitePath + "\\Results"))
                Directory.Delete(suitePath + "\\Results", true);
        }

        public void StartEggPlantDrive()
        {
            try
            {
                string command = "";
                command += String.Format("\"{0}\" -driveport {1}", runScriptPath, drivePort);
                cmdProcess = Common.ExecuteDosCommand(command,false);
            }
            catch (Exception e)
            {
                DiagnosticLog.WriteLine("Exception Caught Starting EggPLant Drive: " + e.Message);
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
                            string name = scriptName + " : Iteration #" + (i + 1).ToString();
                            EggPlantScript eScript = new EggPlantScript(driver,suitePath,scriptName,host,port);
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
                                scriptName = script.SelectSingleNode("@scriptName").Value;
                                host = script.SelectSingleNode("@host").Value;
                                port = script.SelectSingleNode("@port").Value;
                                timeout = int.Parse(script.SelectSingleNode("@timeout").Value);

                                string name = scriptName + " : Iteration #" + (i + 1).ToString() + " : Retry : " + (j+1).ToString();
                                EggPlantScript eScript = new EggPlantScript(driver, suitePath, scriptName, host, port);
                                outcome = eScript.ExecuteTest(name);
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
