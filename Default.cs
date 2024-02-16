using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace astronomy
{
    internal class Default
    {
        public static Dictionary<string, string> Values = new()
        {
            ["Usc_Settings"] = "1",
            ["Never_Suspend"] = "false",
            ["Serial_Mode"] = "UART_FIXED_BAUD_RATE",
            ["Fixed_Baud_Rate"] = "9600",
            ["Serial_Timeout"] = "0",
            ["Enable_Crc"] = "false",
            ["Serial_Device_Number"] = "12",
            ["Serial_Mini_Ssc_Offset"] = "0",
            ["Min_Range_Servo"] = "3968",
            ["Max_Range_Servo"] = "8000",
            ["Acceleration_Servo"] = "100",
            ["Speed_Servo"] = "0",
            ["Glong"] = "17.39519",
            ["Glat"] = "48.22027"
        };
    }
}
