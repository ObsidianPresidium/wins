using System;
using System.Diagnostics;
using System.ComponentModel;

namespace wins
{
    class Wins
    {
        static void Main(string[] args)
        {
            string wingetExecutableContainerPath = Environment.GetEnvironmentVariable("localappdata") + @"\Microsoft\WindowsApps";
            string initDirectory = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(wingetExecutableContainerPath);

            static int mkInstall(string package, bool noPrompt = false, bool dryRun = false)
            {
                string processCommand = (noPrompt) ? "winget install --accept-package-agreements --accept-source-agreements --force " + package : "winget install " + package;
                if (!dryRun)
                { 
                    Process myProcess = System.Diagnostics.Process.Start(processCommand);
                    myProcess.WaitForExit();
                    return myProcess.ExitCode;
                } else
                {
                    Console.WriteLine(processCommand);
                    return 0;
                }
                
            }
            
            static void ensure(string wingetExecutableContainerPath)
            {
                if (!File.Exists(wingetExecutableContainerPath))
                {
                    Process.Start(new ProcessStartInfo("ms-appinstaller:?source=https://aka.ms/getwinget") { UseShellExecute = true });
                    Process[] appInstallerProcesses = Process.GetProcessesByName("AppInstaller");
                    Process appInstallerProcess = appInstallerProcesses[0];
                    Console.WriteLine("Waiting for App Installer. Use a keyboard interrupt if you have already closed the window.");
                    appInstallerProcess.WaitForExit();
                } else
                {
                    Console.WriteLine("Winget is already installed on your account.");
                }
            }

            static void install(string[] args)
            {
                bool noPrompt = false;
                
                for (int i = 1; i < args.Length; i++)
                {
                    string currentPackage = args[i];

                    if (currentPackage == "-y")
                    {
                        noPrompt = true;
                    }
                    else
                    {
                        Console.Write("Installing ");
                        Console.WriteLine(currentPackage);
                        mkInstall(currentPackage, noPrompt);
                    }
                }
            }

            Console.WriteLine(wingetExecutableContainerPath);
            switch (args[0])
            {
                case "ensure":
                    Console.WriteLine("Ensuring that Winget is installed... please accept any GUI windows.");
                    ensure(wingetExecutableContainerPath);
                    break;
                case "install":
                    install(args);
                    break;
                default:
                    Console.WriteLine("Syntax Error.");
                    break;
            }
        }
    }
}