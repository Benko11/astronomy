using astronomy;
using astronomy.Performables;

namespace Astronomy
{
    internal class Index
    {
        static void Main()
        {
            char userOption;

            while (true) {
                Console.WriteLine("0) Relay controls (external application)");
                Console.WriteLine("1) Control servo motors");
                Console.WriteLine("2) Execute XML sequence");
                Console.WriteLine("3) Create XML sequence");
                Console.WriteLine("4) Schedule");
                Console.WriteLine("5) Global settings");
                Console.WriteLine("x) Exit");
                Console.Write("Enter option: ");
                string? raw = Console.ReadLine();

                if (raw == "" || raw == null) continue;
                userOption = raw.ToCharArray()[0];

                Console.WriteLine();

                if (userOption == '0') RelayControls.Perform();
                if (userOption == '1') ServoControl.Perform();
                if (userOption == '2') ParseXML.Perform();
                if (userOption == '3') CreateXML.Perform();
                if (userOption == '4') Schedule.Perform();
                if (userOption == '5') GlobalSettings.Perform();
                if (char.ToLower(userOption) == 'x')
                {
                    Exit.Perform();
                    break;
                }
            }
        }
    }
}