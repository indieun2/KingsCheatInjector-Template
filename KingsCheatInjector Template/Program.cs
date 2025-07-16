using KingsCheatInjector_Template;
using static KingsCheatInjector_Template.Injector;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace KingsCheatInjector_Template
{
    public class Program
    {
        static void Main()
        {
            string asciiTitle = @"
 ██████╗██╗  ██╗███████╗ █████╗ ████████╗    ██╗███╗   ██╗     ██╗███████╗ ██████╗████████╗ ██████╗ ██████╗ 
██╔════╝██║  ██║██╔════╝██╔══██╗╚══██╔══╝    ██║████╗  ██║     ██║██╔════╝██╔════╝╚══██╔══╝██╔═══██╗██╔══██╗
██║     ███████║█████╗  ███████║   ██║       ██║██╔██╗ ██║     ██║█████╗  ██║        ██║   ██║   ██║██████╔╝
██║     ██╔══██║██╔══╝  ██╔══██║   ██║       ██║██║╚██╗██║██   ██║██╔══╝  ██║        ██║   ██║   ██║██╔══██╗
╚██████╗██║  ██║███████╗██║  ██║   ██║       ██║██║ ╚████║╚█████╔╝███████╗╚██████╗   ██║   ╚██████╔╝██║  ██║
 ╚═════╝╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝   ╚═╝       ╚═╝╚═╝  ╚═══╝ ╚════╝ ╚══════╝ ╚═════╝   ╚═╝    ╚═════╝ ╚═╝  ╚═╝";

            Console.Title = "Kings Cheat Injector Template";
            Console.WriteLine(asciiTitle + "\n");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Version 1 \nDisable Windows Defender \n\n");
            Console.ResetColor();

            Console.Write("Press any key to start the injection...");
            Console.ReadKey(true);
            Console.Clear();

            try
            {
                string resourceName = "KingsCheatInjector_Template.Cheat.dll"; // Change Cheat.dll with your dlls name you dragged into the project
                string tempDllPath = Path.Combine(Path.GetTempPath(), "Cheat.dll"); // Same here

                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                {
                    if (stream == null) throw new Exception("Failed to locate embedded DLL.");
                    using (MemoryStream ms = new MemoryStream())
                    {
                        stream.CopyTo(ms);
                        File.WriteAllBytes(tempDllPath, ms.ToArray());
                    }
                }

                Console.WriteLine("DLL extracted to: " + tempDllPath);
                Console.WriteLine("Searching for target process...");

                int pid = FindProcessId("GorillaTag");
                if (pid == -1) throw new Exception("Target process not found.");

                Console.WriteLine("Injecting...");
                Thread.Sleep(500);
                Injector.Inject(pid, tempDllPath);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Injection successful.");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Injection failed: " + ex.Message);
            }

            Console.ResetColor();
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey(true);
        }

        static int FindProcessId(string name)
        {
            var procs = Process.GetProcessesByName(name);
            return procs.Length > 0 ? procs[0].Id : -1;
        }
    }
}