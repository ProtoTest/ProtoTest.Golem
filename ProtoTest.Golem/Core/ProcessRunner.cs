using System;
using System.Diagnostics;
using System.IO;

namespace ProtoTest.Golem.Core
{
    public class ProcessRunner
    {
        private Process process;
        private static bool isStarted = false;
        private string command;
        private static string batchDir = Directory.GetCurrentDirectory();
        private static string batchName = "batch.bat";
        private static string batchPath = batchDir + "\\" + batchName;
        public ProcessRunner(string command)
        {
            this.command = command;
            CreateBatchFile();
        }

        private void CreateBatchFile()
        {
            using (StreamWriter sw = File.CreateText(batchPath))
            {
                sw.WriteLine(command);
            }
        }

        public void StopProcess()
        {
            try
            {
                process.CloseMainWindow();
                process.Kill();
                isStarted = false;
            }
            catch (Exception)
            {
  
            }
            
        }
        public void StartProcess()
        {
            if (!isStarted)
            {
                process = new Process();
                var StartInfo = new ProcessStartInfo();
                StartInfo.FileName = batchPath;
                StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                StartInfo.CreateNoWindow = false;
                process.StartInfo = StartInfo;
                process.Start();
                isStarted = true;
            }
        }
    }
}
