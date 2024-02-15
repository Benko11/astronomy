using System.Diagnostics;

namespace astronomy.Performables
{
    internal class RelayControls : IPerformable
    {
        public static void Perform()
        {
            if (!Env.Settings.ContainsKey("Relay_Path"))
            {
                string path = Utils.GetInput("Relay path", (_) => true, (input) => input.Replace("\"", ""));
                Env.SetValue("Relay_Path", path);
                return;
            }

            ProcessStartInfo relayProcess = new();
            relayProcess.CreateNoWindow = false;
            relayProcess.UseShellExecute = false;
            relayProcess.FileName = Env.GetValue("Relay_Path");

            Console.WriteLine("Relay path set, executing ...");
                
            try
            {
                using (Process exeProcess = Process.Start(relayProcess))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch
            {
                throw new Exception("Something went wrong. Please try again later.");
            }
        }
    }
}
