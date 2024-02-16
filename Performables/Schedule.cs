using System.IO;

namespace astronomy.Performables
{
    internal class Schedule : IPerformable
    {
        public delegate void ScheduledWorkload();

        public static void Perform()
        {
            while (true)
            {
                Console.WriteLine("1) Schedule for sunset");
                Console.WriteLine("2) Schedule for sunrise");
                Console.WriteLine("3) Schedule for specific time");
                Console.WriteLine("x) Exit");
                string option = Utils.GetInput("Select option", (input) => input.Trim() == "1" || input.Trim() == "2" || input.Trim() == "3" || input.Trim().ToLower() == "x", input => input.Trim().ToLower());

                if (option == "x") break;

                var sDate = DateTime.Now.ToString();
                var datevalue = Convert.ToDateTime(sDate.ToString());

                var hour = datevalue.Hour.ToString();
                var minute = datevalue.Minute.ToString();

                var day = datevalue.Day.ToString();
                var month = datevalue.Month.ToString();
                var year = datevalue.Year.ToString();
                var currentDaySchedule = new DailyScheduler(int.Parse(day), int.Parse(month), int.Parse(year), 1, Convert.ToDouble(Env.GetValue("Glong")), Convert.ToDouble(Env.GetValue("Glat")), 1);
                var times = currentDaySchedule.GetSchedule();

                if (option == "1")
                {
                    var parTime = times.Last();
                    DailyScheduler useTime = FindRightSchedule(CompareTimes(parTime, $"{hour.PadLeft(2, '0')}{minute.PadLeft(2, '0')}"));

                    var time = useTime.GetSchedule().Last();

                    var xml = new Xml();
                    xml.SetPathInteractive();

                    string mode = Utils.GetInput("(O)pen/(C)lose sequence", input => input.Trim().ToLower() == "o" || input.Trim().ToLower() == "c", input => input.Trim().ToLower());

                    var sequence = mode == "o" ? xml.GetSequence(SequenceType.OPEN) : xml.GetSequence(SequenceType.CLOSE);

                    Console.WriteLine($"Executing code at {time} ...");
                    var ts = new DateTime(useTime.Year, useTime.Month, useTime.Day, int.Parse(time.Substring(0, 2)), int.Parse(time.Substring(2)), 0);
                    RunAt(ts, () =>
                    {
                        Servo servo = new();
                        servo.Execute(device => xml.RunFrames(sequence, device));
                    });
                }

                if (option == "2")
                {
                    var parTime = times.First();
                    int compared = CompareTimes(parTime, $"{hour.PadLeft(2, '0')}{minute.PadLeft(2, '0')}");

                    DailyScheduler useTime = FindRightSchedule(compared);
                    var time = useTime.GetSchedule().First();

                    var xml = new Xml();
                    xml.SetPathInteractive();

                    string mode = Utils.GetInput("(O)pen/(C)lose sequence", input => input.Trim().ToLower() == "o" || input.Trim().ToLower() == "c", input => input.Trim().ToLower());
                                   
                    var sequence = mode == "o" ? xml.GetSequence(SequenceType.OPEN) : xml.GetSequence(SequenceType.CLOSE);

                    Console.WriteLine($"Executing code at {time} ...");
                    var ts = new DateTime(useTime.Year, useTime.Month, useTime.Day, int.Parse(time.Substring(0, 2)), int.Parse(time.Substring(2)), 0);
                    RunAt(ts, () =>
                    {
                        Servo servo = new();
                        servo.Execute(device => xml.RunFrames(sequence, device));
                    });
                }

                if (option == "3")
                {
                    string parTime = Utils.GetInput("Execution time (hhmm)", input => input.Trim().Length == 4);
                    DailyScheduler useTime = FindRightSchedule(CompareTimes(parTime, $"{hour.PadLeft(2, '0')}{minute.PadLeft(2, '0')}"));

                    Console.WriteLine(CompareTimes(parTime, $"{hour.PadLeft(2, '0')}{minute.PadLeft(2, '0')}"));
                    var xml = new Xml();
                    xml.SetPathInteractive();

                    string mode = Utils.GetInput("(O)pen/(C)lose sequence", input => input.Trim().ToLower() == "o" || input.Trim().ToLower() == "c", input => input.Trim().ToLower());

                    var sequence = mode == "o" ? xml.GetSequence(SequenceType.OPEN) : xml.GetSequence(SequenceType.CLOSE);

                    Console.WriteLine($"Executing code at {parTime} ...");
                    var ts = new DateTime(useTime.Year, useTime.Month, useTime.Day, int.Parse(parTime.Substring(0, 2)), int.Parse(parTime.Substring(2)), 0);
                   
                    RunAt(ts, () =>
                    {
                        Servo servo = new();
                        servo.Execute(device => xml.RunFrames(sequence, device));
                    });
                }
            }
        }

        public static int CompareTimes(string time1, string time2)
        {
            var t1 = new DateTime(2024, 1, 1, int.Parse(time1.Substring(0, 2)), int.Parse(time1.Substring(2)), 0);
            var t2 = new DateTime(2024, 1, 1, int.Parse(time2.Substring(0, 2)), int.Parse(time2.Substring(2)), 0);
            return DateTime.Compare(t1, t2);
        }

        public static DailyScheduler FindRightSchedule(int comparator)
        {
            var sDate = comparator < 0 ? DateTime.Now.AddDays(1).ToString() : DateTime.Now.ToString();
            var datevalue = Convert.ToDateTime(sDate.ToString());

            var day = datevalue.Day.ToString();
            var month = datevalue.Month.ToString();
            var year = datevalue.Year.ToString();
            return new DailyScheduler(int.Parse(day), int.Parse(month), int.Parse(year), 1, Convert.ToDouble(Env.GetValue("Glong")), Convert.ToDouble(Env.GetValue("Glat")), 1);
        }

        public static void RunAt(DateTime ts, ScheduledWorkload scheduledWorkload)
        {
            while (true) {
                Thread.Sleep(1000);

                int cmp = DateTime.Compare(ts, DateTime.Now);
                if (cmp < 0) break;
            }

            scheduledWorkload();
        }
    }   
}
