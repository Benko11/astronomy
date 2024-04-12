using System.IO;
using System.Text;

namespace astronomy.Performables
{
    internal class GlobalSettings : IPerformable
    {
        public static void Perform()
        {
            var indexedKeys = new List<string>();
            var header = new StringBuilder("\nGLOBAL SETTINGS FILE\n");
            Enumerable.Range(0, Console.WindowWidth).ToList().ForEach(_ => header.Append('-'));
            header.Append('\n');
            Console.WriteLine(header.ToString());

            var keys = Env.Settings.Keys;
            while (true)
            {
                Console.WriteLine();

                int i = 1;

                foreach (var key in keys)
                {
                    Console.WriteLine($"{i++}) {key}: {Env.GetValue(key)}");
                    indexedKeys.Add(key);
                }

                i = 1;

                Console.WriteLine("n) New Entry");
                Console.WriteLine("r) Reset all settings");
                Console.WriteLine("x) Exit");

                string userOption = Utils.GetInput("Select option", input => input != "" && input != null, input => input.Trim().ToLower());
                if (userOption.Equals("x", StringComparison.CurrentCultureIgnoreCase))
                    break;

                if (userOption.Equals("r", StringComparison.CurrentCultureIgnoreCase))
                {
                    var confirmation = Utils.GetInput("WARNING! This will remove all of your settings and custom properties and replace them with default values! You will have to manually set the path for the relay application again! Are you sure? (y/n)", (_) => true, (input) => input.Trim().ToLower());

                    if (confirmation == "y")
                    {
                        Env.Reset();

                        foreach (KeyValuePair<string, string> item in Default.Values)
                        {
                            Env.SetValue(item.Key, item.Value);
                        }
                    }
                }

                if (userOption.Equals("n", StringComparison.CurrentCultureIgnoreCase))
                {
                    string keyName = Utils.GetInput("Key name", (input) => input.Trim().Length > 0);
                    string value = Utils.GetInput("Value", (input) => input.Trim().Length > 0);
                    Env.SetValue(keyName, value);
                }

                if (int.TryParse(userOption, out int option))
                {
                    string keyName = indexedKeys.ToArray()[option - 1];
                    string newValue = Utils.GetInput($"Enter new value for {keyName}", (input) => input.Trim().Length > 0, (input) => input.Trim());

                    Env.SetValue(keyName, newValue);
                }

                Env.Initialize();
            }

            Console.WriteLine();
        }
    }
}
