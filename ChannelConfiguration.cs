using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

enum Mode { SERVO }
enum HomeMode { OFF, ON }

namespace astronomy
{
    internal class ChannelConfiguration
    {
        public string Name { get; set; } = string.Empty;
        public Mode Mode { get; set; } = Mode.SERVO;
        public uint Min {  get; set; }
        public uint Max { get; set; }
        public HomeMode HomeMode {  get; set; } = HomeMode.OFF;
        public uint Speed { get; set; }
        public uint Acceleration { get; set; }
        public uint Neutral { get; set; } = 3968;
        public uint Range { get; set; } = 1905;

        public ChannelConfiguration(uint min, uint max, uint speed, uint acceleration) {
            Min = min;
            Max = max;
            Speed = speed;
            Acceleration = acceleration;
        }

        public ChannelConfiguration(uint min, uint max, uint speed, uint acceleration, uint neutral, uint range, string name)
        {
            Min = min;
            Max = max;
            Speed = speed;
            Acceleration = acceleration;
            Neutral = neutral;
            Range = range;
            Name = name;
        }

        public ChannelConfiguration(uint min, uint max, uint speed, uint acceleration, uint neutral, uint range, string name, Mode mode, HomeMode homeMode)
        {
            Min = min;
            Max = max;
            Speed = speed;
            Acceleration = acceleration;
            Neutral = neutral;
            Range = range;
            Name = name;
            Mode = mode;
            HomeMode = homeMode;
        }

        public override string ToString()
        {
            return $"{Min}-{Max}, speed: {Speed}, acceleration: {Acceleration}";
        }
    }
}
