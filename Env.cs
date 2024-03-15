namespace astronomy
{
    internal class Env
    {
        private static readonly string PATH = @$"{System.Environment.GetEnvironmentVariable("USERPROFILE")}\global.txt";
        private static Dictionary<string, string> settings = [];
        public static readonly string UNDEFINED_VALUE = "UNDEFINED_VALUE";

        public static Dictionary<string, string> Settings {
            get { return settings; }
        }

        public static string GetValue(string key)
        {
            try
            {
                return settings[key];
            }
            catch (KeyNotFoundException)
            {
                if (Default.Values.ContainsKey(key))
                    return Default.Values[key].ToString();

                return UNDEFINED_VALUE;
            }
        }

        public static void SetValue(string key, string value) {
            if (!Settings.ContainsKey(key))
            {
                using StreamWriter streamWriter = File.AppendText(PATH);
                streamWriter.Write(Environment.NewLine + $"{key}: {value}");
            } else {
                int lineNumber = 0;
                using (StreamReader streamReader = new(PATH))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        lineNumber++;
                        if (line.Contains(key)) break;
                    }
                }
                Utils.ChangeLineInFile(PATH, lineNumber, $"{key}: {value}");
            }


            Initialize();
        }

        public static void Initialize()
        {
            settings = [];

            try
            {
                using StreamReader streamReader = new(PATH);
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (line == "") continue;

                    var parts = line.Split(": ");
                    settings.Add(parts[0], parts[1]);
                }
            }
            catch (FileNotFoundException)
            {

            }
        }

        public static void Reset()
        {

            File.WriteAllText(PATH, "");
            settings = [];
        }

        public static bool Exists(string key)
        {
            return GetValue(key) != null && GetValue(key) != "" && GetValue(key) != UNDEFINED_VALUE;
        }

        static Env() {
            Initialize();
        }
    }
}
