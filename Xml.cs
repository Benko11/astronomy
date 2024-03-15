using Microsoft.VisualBasic;
using Pololu.Usc;
using System.Xml;

namespace astronomy
{
    internal class Xml
    {
        private string? path {get; set;}
        private List<FrameConfiguration>? openSequence;
        private List<FrameConfiguration>? closeSequence;
        private char openOption;
        private char closeOption;
        public Xml()
        {
        }

        public string SetPathInteractive()
        {
            while (true)
            {
                string description = "XML configuration file path";
                var lastPath = Env.GetValue("Last_Path");

                if (Env.Exists("Last_Path"))
                    description += $" ({lastPath})";

                path = Utils.GetInput(description, input => true, input => input.Replace("\"", ""));
                if (path == null || path == "")
                {
                    if (Env.Exists("Last_Path")) path = lastPath;
                }

                if (path != null && path != "" && Utils.IsValidWindowsPath(path) && File.Exists(path)) break; 
                
                Console.WriteLine("This file does not exist");
            }

            Env.SetValue("Last_Path", path);

            return path;
        }

        private string GetAttributeValue(XmlNode node, string name) {
            if (node.Attributes == null) return "";

            foreach (XmlAttribute attr in node.Attributes)
                if (attr.Name == name)
                    return attr.Value;

            return "";
        }

        public List<FrameConfiguration>? GetSequence(SequenceType? type = null)
        {
            if (path == null) return null;

            List<FrameConfiguration> seq = new();
            XmlDocument doc = new();
            doc.Load(path);

            uint duration = 0;
            ushort[] positions = Array.Empty<ushort>();
            string name = "";

            XmlNode? sequences = doc.GetElementsByTagName("Sequences")[0];
            if (sequences == null) return null;

            foreach (XmlNode sequence in sequences)
            {
                if (sequence == null || sequence.Attributes == null) break;

                string sequenceName = GetAttributeValue(sequence, "name");

                if (type == SequenceType.OPEN && sequenceName.ToLower() != "open") continue;
                if (type == SequenceType.CLOSE && sequenceName.ToLower() != "close") continue;

                foreach (XmlNode frame in sequence)
                {
                    if (frame.Attributes  == null) continue;

                    name = GetAttributeValue(frame, "name");
                    duration = Convert.ToUInt32(GetAttributeValue(frame, "duration"));
                    positions = frame.InnerText.Split(" ").Select(ushort.Parse).ToArray();

                    seq.Add(new FrameConfiguration(name, duration, positions));
                }
            }

            return seq;
        }

        public char Menu(char openOption = 'a', char closeOption = 'b')
        {
            this.openOption = openOption;
            this.closeOption = closeOption;

            char userInput;
            List<char> options = new();
            do {
                if (openSequence != null && openSequence.Count > 0)
                {
                    Console.WriteLine("{0}) Run open sequence", openOption);
                    options.Add(openOption);
                }

                if (closeSequence != null && closeSequence.Count > 0)
                {
                    Console.WriteLine("{0}) Close sequence", closeOption);
                    options.Add(closeOption);
                }

                Console.WriteLine("x) Go to Main Menu");

                Console.Write("Select: ");

                string rawInput = Console.ReadLine();
                if (rawInput == null) return (char)0;

                userInput = rawInput.ToLower().ToCharArray()[0];
                if (userInput == 'x') break;
            } while (options.Count > 0 && !options.Contains(userInput));

            return userInput;
        }

        
        public void RunFrames(List<FrameConfiguration> configurations, Usc device)
        {
            var accelerations = Utils.GetArrayedValues("Acceleration_Servo");
            var speeds = Utils.GetArrayedValues("Speed_Servo");

            foreach (FrameConfiguration configuration in configurations)
            {
                var (_, duration, positions) = configuration;
                Console.WriteLine("Running frame:");
                Console.WriteLine(configuration.ToString());

                for (byte i = 0; i < positions.Length; i++)
                {
                    device.setAcceleration(i, (ushort) accelerations[i]);
                    device.setSpeed(i, (ushort) speeds[i]);
                    device.setTarget(i, positions[i]);
                }

                Thread.Sleep(checked((int)duration));
            }
        }

    

        public void DoStuff()
        {
            List<int> accelerations = Utils.GetArrayedValues("Acceleration_Servo");
            List<int> speeds = Utils.GetArrayedValues("Speed_Servo");

            if (!Utils.DeviceCountMatches(accelerations)) throw new Exception("Number of input parameters for acceleration in global.txt file does not match the servo count of the device! Please consult Global Settings to remedy this issue.");
            if (!Utils.DeviceCountMatches(speeds)) throw new Exception("Number of input parameters for acceleration in global.txt file does not match the servo count of the device! Please consult Global Settings to remedy this issue.");

            path = SetPathInteractive();
            openSequence = GetSequence(SequenceType.OPEN);
            closeSequence = GetSequence(SequenceType.CLOSE);

            char userSelection;
            while ((userSelection = Menu()) != 'x') {
                if (userSelection == openOption && openSequence != null)
                {
                    Servo servo = new();
                    servo.Execute(device => RunFrames(openSequence, device));
                }

                if (userSelection == closeOption && closeSequence != null)
                {
                    Servo servo = new();
                    servo.Execute(device => RunFrames(closeSequence, device));
                }
            }

            Console.WriteLine();
        }
    }
}
