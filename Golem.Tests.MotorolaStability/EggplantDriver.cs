using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using CookComputing.XmlRpc;
using System.Threading;
using System.Diagnostics;

namespace ProtoTest.TestRunner.Eggplant
{

    /// <summary>
    /// Eggplant drive is an RPC service that we can use to execute eggplant commands
    /// This class wraps all calls to the RPC service with easy to use methods.  
    /// The following methods should be called in the following order: 
    /// StartEggplantDrive, StartSuiteSession, Connect, Execute (or ExecuteScript), EndSession, StopEggplantDrive
    /// </summary>
    public class EggplantDriver
    {
        private IEggplantDriveService driver;
        public string suitePath;
        public bool driveRunning = false;
        public bool connectedToDevice = false;
        public bool suiteStarted = false;

        public EggplantDriver(int timeoutMs)
        {
            driver = (IEggplantDriveService)XmlRpcProxyGen.Create(typeof(IEggplantDriveService));
            driver.Timeout = timeoutMs;
        }

        /// <summary>
        /// Starts the eggplantDrive process by executing a batch file, and waiting x number of ms
        /// </summary>
        /// <param name="batchPath"></param>
        /// <param name="waitForDriveMs"></param>
        /// <returns></returns>
        public Process StartEggPlantDrive(string batchPath,int waitForDriveMs)
        {
            try
            {
                DiagnosticLog.WriteLine("Starting Eggplant Drive using batch file : " + batchPath);
                System.Diagnostics.Process cmdProcess = Common.ExecuteBatchFile(batchPath);
                Thread.Sleep(waitForDriveMs);
                driveRunning = true;
                return cmdProcess;
            }
            catch (Exception e)
            {
                Assert.Fail("Exception Caught Starting EggPLant Drive: " + e.Message);
                return null;
            }
        }

        /// <summary>
        /// Stops eggplant drive by searching the process list and killing the process named Eggplant
        /// </summary>
        public void StopEggPlantDrive()
        {
            try
            {
                DiagnosticLog.WriteLine("Trying to stop Eggplant Drive");
                Common.KillProcess("Eggplant");
                Thread.Sleep(5000);
                driveRunning = false;
            }
            catch (Exception e)
            {
                Assert.Fail("Exception caught stopping eggplant drive " + e.Message);
            }

        }

        /// <summary>
        /// Runs a .script file.  A suite session should already have been started, so only pass the name of the script, not the path
        /// </summary>
        /// <param name="scriptName"></param>
        /// <param name="description"></param>
        public void ExecuteScript(string scriptName, string description)
        {
            DiagnosticLog.WriteLine("Executing test : " + scriptName);
            TestLog.WriteLine(description);
            driver.Execute("RunWithNewResults(" + scriptName + ")");
        }

        /// <summary>
        /// Connects to a device, must be run before any steps can be executed.
        /// </summary>
        /// <param name="host"></param>
        public void Connect(string host)
        {
            try
            {

                DiagnosticLog.WriteLine("Trying to connect to device : " + host);
                driver.Execute("Connect (name:\"" + host + "\")");

            }
            catch (Exception e)
            {
                Assert.Fail("Error caught connecting to device " + host + " : " + e.Message);
            }
        }

        /// <summary>
        /// Executes any Eggplant command.  Full list of commands available in eggplant drive documentation.
        /// </summary>
        /// <param name="command"></param>
        public void Execute(string command)
        {
            try
            {

                DiagnosticLog.WriteLine("Executing command : " + command);
                driver.Execute(command);

            }
            catch (Exception e)
            {
                Assert.Fail("Error caught executing command " + command + " : " + e.Message);
            }
        }

        /// <summary>
        /// Ends a suite session.  Should be called before Stopping Drive, or when changing suites.
        /// </summary>
        public void EndSuiteSession()
        {
            try
            {
                DiagnosticLog.WriteLine("Ending Eggplant Session");
                driver.EndSession();
            }
            catch (Exception e)
            {
                //Assert.Fail("Exception Caught Ending EggPlant Session for suite : " + suitePath + e.Message);
            }
        }

        /// <summary>
        /// Starts a Suite Session. Should be called before executing any commands.
        /// </summary>
        /// <param name="suitePath"></param>
        public void StartSuiteSession(string suitePath)
        {
            try
            {
                this.suitePath = suitePath;
                DiagnosticLog.WriteLine("Starting eggplant session for suite : " + suitePath);
                driver.StartSession(suitePath);
            }
            catch (Exception e)
            {
                Assert.Fail("Exception Caught Starting EggPLant Session for suite : " + suitePath + " Check the log to see if drive started correctly : " + e.Message);
            }

        }
    }
}
