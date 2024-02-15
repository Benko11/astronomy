using System.Xml.Linq;

namespace astronomy.Performables
{
    class Frame
    {
        public Frame() { }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private uint _duration;
        public uint Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        private ushort[] _positions;
        public ushort[] Positions
        {
            get { return _positions; }
            set { _positions = value; }
        }

        private SequenceType _sequenceType;
        public SequenceType SequenceType
        {
            get { return _sequenceType; }
            set { _sequenceType = value; }
        }

        public override string ToString()
        {
            return $"{Name} ({Duration}): {string.Join(", ", Positions)}";
        }
    }

    internal class CreateXML : IPerformable
    {
        private static XElement AddFrame(Frame frame)
        {
            XElement element = new("Frame");
            element.Add(new XAttribute("name", frame.Name));
            element.Add(new XAttribute("duration", frame.Duration.ToString()));
            element.Value = string.Join(" ", frame.Positions);
            return element;
        }

        private static List<Frame> AskForFrames()
        {
            List<Frame> frames = new();

            while (true)
            {
                Frame frame = new();
                Console.WriteLine("(press Enter to finish adding frames)\n");

                frame.Name = Utils.GetInput("Enter frame name", input => true);
                if (frame.Name == "") break;

                char sequenceType = Utils.GetInput("(O)pen or (C)lose sequence", input => input.ToLower().ToCharArray()[0] == 'o' || input.ToLower().ToCharArray()[0] == 'c', input => input.ToLower().ToCharArray()[0]);
                if (sequenceType == 'c') frame.SequenceType = SequenceType.CLOSE;
                else if (sequenceType == 'o') frame.SequenceType = SequenceType.OPEN;

                frame.Duration = Utils.GetInput("Frame duration (ms)", input => int.TryParse(input, out _), input => Convert.ToUInt32(input));

                frame.Positions = Utils.GetInput("Frame states (space-separated integers)", input => input.Split(' ').All(item => ushort.TryParse(item, out _)), input => input.Split(" ").Select(ushort.Parse).ToArray());

                frames.Add(frame);
            };

            return frames;
        }

        public static string EnterFileName()
        {
            string fileName = Utils.GetInput("Enter file name (no suffix)", Utils.IsValidWindowsPath);
            if (fileName == null || fileName == "")
            {
                fileName = "file";
            }

            string file = fileName.Replace(' ', '-');
            return string.Format(@"{0}\{2}-{1}.txt", Environment.GetEnvironmentVariable("USERPROFILE"), file, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ffff"));
        }

        public static void Perform()
        {
            List<Frame> closeFrames = new();
            List<Frame> openFrames = new();

            List<Frame> frames = AskForFrames();
            frames.ForEach(frame =>
            {
                if (frame.SequenceType == SequenceType.CLOSE)
                    closeFrames.Add(frame);

                if (frame.SequenceType == SequenceType.OPEN)
                    openFrames.Add(frame);
            });


            XElement closeSequence = new("Sequence",
                new XAttribute("name", "Close"),
                closeFrames.Select(frame => AddFrame(frame))
            );

            XElement openSequence = new("Sequence",
                new XAttribute("name", "Open"),
                openFrames.Select(frame => AddFrame(frame))
            );

            XElement sequences = new("Sequences", closeSequence, openSequence);
            XDocument main = new(
                new XElement("UscSettings", new XAttribute("version", Env.GetValue("Usc_Settings")),
                new XElement("NeverSuspend", Env.GetValue("Never_Suspend")),
                new XElement("SerialMode", Env.GetValue("Serial_Mode")),
                new XElement("FixedBaudRate", Env.GetValue("Fixed_Baud_Rate")),
                new XElement("SerialTimeout", Env.GetValue("Serial_Timeout")),
                new XElement("EnableCrc", Env.GetValue("Enable_Crc")),
                new XElement("SerialDeviceNumber", Env.GetValue("Serial_Device_Number")),
                new XElement("SerialMiniSscOffset", Env.GetValue("Serial_Mini_Ssc_Offset")),
                new XElement("Script", new XAttribute("ScriptDone", false)),
            sequences));

            string path = EnterFileName();
            main.Save(path);

            Console.WriteLine($"\nSaved to {path}\n");
        }
    }
}
