using Pololu.UsbWrapper;
using Pololu.Usc;
using System.Text;

namespace astronomy.Performables
{
    internal class ServoControl : IPerformable
    {
        private static byte FindServo()
        {
            char[] selection;

            do
            {
                Console.Write("Select servo channel (0-7): ");
                string str;
                str = Console.ReadLine() ?? "";
                selection = str.ToCharArray();
                if (str.Equals(""))
                {
                    selection = new char[1];
                    selection[0] = (char)0;
                    continue;
                }

            }
            while (selection[0] < '0' || selection[0] > '9');

            return (byte)(Encoding.ASCII.GetBytes(selection)[0] - 48);
        }

        private static ushort FindTarget(ushort min, ushort max)
        {
            ushort targetNumber;
            string str;
            do
            {
                Console.Write($"Target ({min}-{max}): ");
                str = Console.ReadLine() ?? "";

                bool check = ushort.TryParse(str, out _);
                if (!check || str.Equals(""))
                {
                    targetNumber = 0;
                    continue;
                }
                targetNumber = ushort.Parse(str);
            } while (targetNumber < min || targetNumber > max);

            return targetNumber;
        }

        public static void Perform()
        {
            var servo = new Servo();
            servo.Execute(device =>
            {
                byte servoNumber = FindServo();
                ushort targetNumber = FindTarget(UInt16.Parse(Env.GetValue("Min_Range_Servo")), UInt16.Parse(Env.GetValue("Max_Range_Servo")));

                device.setAcceleration(servoNumber, UInt16.Parse(Env.GetValue("Acceleration_Servo")));
                device.setSpeed(servoNumber, UInt16.Parse(Env.GetValue("Speed_Servo")));
                device.setTarget(servoNumber, targetNumber);
            });
        }
    }
}
