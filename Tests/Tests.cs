using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;
using Processes.Application;
using System.Diagnostics;

namespace Tests
{
    [TestFixture]
    public class ProcessMonitorTests
    {
        [Test]
        public void Test_01_KillsProcessWithExceedingLifeTime()
        {
            // Arrange
            string processName = "notepad";
            int maxLifetimeSeconds = 2;
            int monitorFrequencySeconds = 1;
            string pathToFile = Path.Combine(Environment.CurrentDirectory, @"TestData.txt");

            // Act
            Process.Start(@"notepad.exe", pathToFile);
            Thread.Sleep(4000);
            bool flag = true;
            while (flag)
            {
                Programm.KillProcessesExceedingLifeTime(processName, maxLifetimeSeconds);
                Thread.Sleep(monitorFrequencySeconds * 1000);
                if (!Process.GetProcessesByName(processName).Any(p => p.ProcessName == processName))
                {
                    flag = false;
                }
            }
            bool itTrue = Process.GetProcessesByName(processName).Any(p => p.ProcessName == processName);

            //Assert
            Assert.That(!itTrue, Is.True, $"Process: {processName} still running.");
        }

        [Test]
        public void Test_02_KillsProcessNotExceedingLifeTime()
        {
            // Arrange
            string processName = "notepad";
            int maxLifetimeSeconds = 4;
            int monitorFrequencySeconds = 1;
            bool flag = true;
            string pathToFile = Path.Combine(Environment.CurrentDirectory, @"TestData.txt");

            // Act
            Process.Start(@"notepad.exe", pathToFile);
            Thread.Sleep(2000);
            while (flag)
            {
                Programm.KillProcessesExceedingLifeTime(processName, maxLifetimeSeconds);
                Thread.Sleep(monitorFrequencySeconds * 1000);
                if (!Process.GetProcessesByName(processName).Any(p => p.ProcessName == processName))
                {
                    flag = false;
                }
            }
            bool isTrue = Process.GetProcessesByName(processName).Any(p => p.ProcessName == processName);

            Assert.That(isTrue, Is.False, $"Process: {processName} was stopped.");
            Thread.Sleep(3000);

            Programm.KillProcessesExceedingLifeTime(processName, maxLifetimeSeconds);

            isTrue = Process.GetProcessesByName(processName).Any(p => p.ProcessName == processName);

            //Assert
            Assert.That(!isTrue, Is.True, $"Process: {processName} still running.");
        }
    }
}