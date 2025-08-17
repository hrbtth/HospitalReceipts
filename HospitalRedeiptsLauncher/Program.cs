using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

class Program
{
    [STAThread]
    static void Main()
    {
        // Folder where HospitalReceiptsLauncher.exe is located
        string launcherDir = AppDomain.CurrentDomain.BaseDirectory;

        // Go up TWO levels to reach the main solution folder
        string parentDir = Directory.GetParent(launcherDir)!.Parent!.FullName;

        // Now into HospitalReceiptsServerApp
        string serverExe = Path.Combine(parentDir, "HospitalReceiptsServerApp", "HospitalReceipts.exe");

        // Debug info
        //MessageBox.Show($"Looking for HospitalReceipts.exe at:\n{serverExe}", "Debug Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

        if (!File.Exists(serverExe))
        {
            MessageBox.Show($"Cannot find HospitalReceipts.exe at:\n{serverExe}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        //-------
        // Folder where HospitalReceiptsLauncher.exe is located
        //string launcherDir = AppDomain.CurrentDomain.BaseDirectory;

        //// Go up one level, then into HospitalReceiptsServerApp
        //string parentDir = Directory.GetParent(launcherDir)!.FullName;
        //string serverExe = Path.Combine(parentDir, "HospitalReceiptsServerApp", "HospitalReceipts.exe");
        //// Show the full path being used
        //MessageBox.Show("Looking for HospitalReceipts.exe at:\n" + serverExe,
        //                "Debug Info",
        //                MessageBoxButtons.OK,
        //                MessageBoxIcon.Information);
        //if (!File.Exists(serverExe))
        //{
        //    MessageBox.Show(
        //        "Cannot find HospitalReceipts.exe at:\n" + serverExe,
        //        "Error",
        //        MessageBoxButtons.OK,
        //        MessageBoxIcon.Error
        //    );
        //    return;
        //}
        ////------------------
        // Start the server with correct working directory
        var serverProcess = Process.Start(new ProcessStartInfo
        {
            FileName = serverExe,
            WorkingDirectory = Path.GetDirectoryName(serverExe)!,
            UseShellExecute = true
        });

        if (serverProcess == null)
        {
            MessageBox.Show(
                "Failed to start HospitalReceipts.exe",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
            );
            return;
        }

        // Wait a little for server to boot
        Thread.Sleep(2000);

        // Open browser
        Process.Start(new ProcessStartInfo
        {
            FileName = "http://localhost:5000",
            UseShellExecute = true
        });

        // -------- Monitor browsers ----------- 
        string[] browserNames = { "chrome", "msedge", "firefox", "iexplore", "opera", "brave" };

        while (true)
        {
            Thread.Sleep(3000);

            var runningBrowsers = browserNames
                .SelectMany(name => Process.GetProcessesByName(name))
                .ToList();

            if (runningBrowsers.Count == 0)
            {
                try
                {
                    if (!serverProcess.HasExited)
                    {
                        serverProcess.Kill(true);
                    }
                }
                catch { }

                break;
            }
        }
    }
}
