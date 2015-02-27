using System;
using System.Diagnostics;
using System.IO;

namespace ProtoTest.Golem.Core
{
    public class ProcessRunner
    {
        private static bool isStarted;
        private readonly string command;
        private Process process;

        public ProcessRunner(string command)
        {
            this.command = command;
            CreateBatchFile();
        }

        private void CreateBatchFile()
        {
            using (var sw = File.CreateText(batchPath))
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

        private static readonly string batchDir = Directory.GetCurrentDirectory();
        private static readonly string batchName = "batch.bat";
        private static readonly string batchPath = batchDir + "\\" + batchName;
    }
}