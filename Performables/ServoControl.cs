﻿using Pololu.UsbWrapper;
using Pololu.Usc;
using System.Text;

namespace astronomy.Performables
{
    internal class ServoControl : IPerformable
    {
        private static byte FindServo(ushort availableChannels)
        {
            byte selection = Utils.GetInput($"Select servo channel (0-{availableChannels - 1})", input => input[0] >= '0' && input[0] <= availableChannels + 48 - 1, input => (byte)(Encoding.ASCII.GetBytes(input)[0] - 48));

            return selection;
        }

        private static ushort FindTarget(ushort min, ushort max)
        {
            ushort targetNumber = Utils.GetInput($"Target ({min}-{max})", input => ushort.TryParse(input, out _) && ushort.Parse(input) >= min && ushort.Parse(input) <= max, input => ushort.Parse(input));

            return targetNumber;
        }

        public static void Perform()
        {
            var servo = new Servo();
            servo.Execute(device =>
            {
                byte servoNumber = FindServo(device.servoCount);

                ushort targetNumber = FindTarget(UInt16.Parse(Env.GetValue("Min_Range_Servo")), UInt16.Parse(Env.GetValue("Max_Range_Servo")));

                device.setAcceleration(servoNumber, UInt16.Parse(Env.GetValue("Acceleration_Servo")));
                device.setSpeed(servoNumber, UInt16.Parse(Env.GetValue("Speed_Servo")));

                device.setTarget(servoNumber, targetNumber);
            });
        }
    }
}
