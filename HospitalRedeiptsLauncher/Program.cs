using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.Http;
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

        if (!File.Exists(serverExe))
        {
            MessageBox.Show($"Cannot find HospitalReceipts.exe at:\n{serverExe}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        //  Find a free port
        int port;
        using (var listener = new TcpListener(IPAddress.Loopback, 0))
        {
            listener.Start();
            port = ((IPEndPoint)listener.LocalEndpoint).Port;
        }

        string url = $"http://localhost:{port}";

        //  Start the server with dynamic port
        var serverProcess = Process.Start(new ProcessStartInfo
        {
            FileName = serverExe,
            Arguments = $"--urls {url}",   // pass port to server
            WorkingDirectory = Path.GetDirectoryName(serverExe)!,
            UseShellExecute = false,
            CreateNoWindow = true,
            WindowStyle = ProcessWindowStyle.Hidden
        });

        if (serverProcess == null)
        {
            MessageBox.Show("Failed to start HospitalReceipts.exe", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        //  Wait for server to respond
        using (var httpClient = new HttpClient())
        {
            var timeout = TimeSpan.FromSeconds(15);
            var start = DateTime.Now;

            while (DateTime.Now - start < timeout)
            {
                try
                {
                    var response = httpClient.GetAsync(url).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        break; // server is ready
                    }
                }
                catch
                {
                    // server not ready yet
                }

                Thread.Sleep(500);
            }
        }

        // Open browser once server is ready
        Process.Start(new ProcessStartInfo
        {
            FileName = url,
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

        serverProcess?.Dispose();
        Environment.Exit(0);
    }
}
