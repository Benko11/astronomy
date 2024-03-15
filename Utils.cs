using astronomy.Performables;

namespace astronomy
{
    public delegate bool ShouldContinue(string input);
    public delegate dynamic ProcessInput(string input);
    public delegate void OptionWorkload();

    internal class Utils
    {
        public static dynamic GetInput(string description, ShouldContinue shouldContinue, ProcessInput? processInput = null)
        {
            string input;
            do
            {
                Console.Write(description + ": ");
                input = Console.ReadLine();
            } while (input == null || !shouldContinue(input));

            if (processInput == null) return input;
            return processInput(input);
        }
        
        public static bool IsValidWindowsPath(string path)
        {
            return !new[] { '<', '>', '|', '?', '*' }.Any(c => path.Contains(c));
        }

        public static void ChangeLineInFile(string fileName, int n, string newText)
        {
            string[] arrLine = File.ReadAllLines(fileName);
            arrLine[n - 1] = newText;
            File.WriteAllLines(fileName, arrLine);
        }

        public static int GetDeviceCount()
        {
            int deviceCount = -1;
            var servo = new Servo();
            servo.Execute(device => {
                deviceCount = device.servoCount;
            });
            return deviceCount;
        }

        public static bool DeviceCountMatches(List<int> values)
        {
            return GetDeviceCount() == values.Count;
        }

        public static List<int> GetArrayedValues(string key) {
            return Env.GetValue(key).Split(' ').ToList().Select(value => Int32.Parse(value)).ToList();
        }
    }
}
