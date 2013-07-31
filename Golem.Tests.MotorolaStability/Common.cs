using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using Gallio.Framework;
using System.Diagnostics;
using Gallio.Model;
using MbUnit.Framework;

namespace ProtoTest.TestRunner.Eggplant
{
    /// <summary>
    /// Contains various random shared methods
    /// </summary>
    public class Common
    {
        private static string configFilePath = Directory.GetCurrentDirectory() + "\\TestConfig.xml";

        /// <summary>
        /// Kills a process by name
        /// </summary>
        /// <param name="name"></param>
        public static void KillProcess(string name)
        {
            Process[] runningProcesses = Process.GetProcesses();
            foreach (Process process in runningProcesses)
            {
                if (process.ProcessName == name)
                {
                    DiagnosticLog.WriteLine("Killing Process : " + name);
                    process.Kill();
                }
            }
        }

        /// <summary>
        /// Executes a batch file at a specific path;
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Process ExecuteBatchFile(string filePath)
        {
            if (!File.Exists(filePath))
                Assert.Fail("Could not find batch file : " + filePath);
            return System.Diagnostics.Process.Start(filePath);
        }

        /// <summary>
        /// Gets a value from the config file.
        /// </summary>
        /// <param name="xpath">
        /// Pass an xpath expression
        /// </param>
        /// <returns>
        /// Returns the value string
        /// </returns>
        public static string GetValueFromConfigFile(string xpath)
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

        /// <summary>
        /// Looks in the config file and returns an XmlNodeList containing all nodes matching the xpath expression.
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public static XmlNodeList GetNodesFromConfigFile(string xpath)
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

        /// <summary>
        /// Pass an xpath expression and xmlnode, and get the value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static string GetValueFromNode(string value, XmlNode node)
        {
            if (node.SelectNodes(value).Count == 0)
            {
                throw new KeyNotFoundException("Could not find an element matching xpath : " + value + " in file " + configFilePath);
            }
            return node.SelectSingleNode(value).Value;
        }

        /// <summary>
        /// Deletes all files in the results directory for the suitePath parameter
        /// </summary>
        /// <param name="suitePath"></param>
        public static void DeleteResultsDirectory(string suitePath)
        {
            if (Directory.Exists(suitePath + "\\Results"))
            {
                DiagnosticLog.WriteLine("Old results found, deleting Eggplant results directory");
                Directory.Delete(suitePath + "\\Results", true);
            }
        }

        /// <summary>
        /// Resizes an image and returns the new image
        /// </summary>
        /// <param name="image"></param>
        /// <param name="scale"></param>
        /// <returns></returns>
        public static Image ScaleImage(Image image, double scale = .5)
        {
            var newWidth = (int)(image.Width * scale);
            var newHeight = (int)(image.Height * scale);

            var newImage = new Bitmap(newWidth, newHeight);
            Graphics.FromImage(newImage).DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }
    }
}
