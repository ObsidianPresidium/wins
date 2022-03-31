using System;
using System.Diagnostics;
using System.ComponentModel;

namespace wins
{
    class Wins
    {
        static void Main(string[] args)
        {
            string wingetExecutablePath = Environment.GetEnvironmentVariable("localappdata") + @"\Microsoft\WindowsApps\winget.exe";

            static void ensure(string wingetExecutablePath)
            {
                if (!File.Exists(wingetExecutablePath))
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
                string commandConstruct = "winget install";
                for (int i = 1; i < args.Length; i++)
                {
                    if (args[i] == "-y")
                    {
                        commandConstruct += " --accept-package-agreements --accept-source-agreements --force";
                    }
                    else
                    {
                        Console.Write("Installing ");
                        Console.WriteLine(args[i]);
                        Process myProcess = System.Diagnostics.Process.Start(commandConstruct += args[i]);
                        myProcess.WaitForExit();
                    }
                }
            }

            Console.WriteLine(wingetExecutablePath);
            switch (args[0])
            {
                case "ensure":
                    Console.WriteLine("Ensuring that Winget is installed... please accept any GUI windows.");
                    ensure(wingetExecutablePath);
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