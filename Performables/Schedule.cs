namespace astronomy.Performables
{
    internal class Schedule : IPerformable
    {
        public static void Perform()
        {
            Console.WriteLine("1) Schedule for sunset");
            Console.WriteLine("2) Schedule for sunrise");
            Console.WriteLine("x) Exit");

            var sDate = DateTime.Now.ToString();
            var datevalue = Convert.ToDateTime(sDate.ToString());
            var day = datevalue.Day.ToString();
            var month = datevalue.Month.ToString();
            var year = datevalue.Year.ToString();

            var dailyScheduler = new DailyScheduler(int.Parse(day), int.Parse(month), int.Parse(year), 1, Convert.ToDouble(Env.GetValue("Glong")), Convert.ToDouble(Env.GetValue("Glat")), 1);
            dailyScheduler.GetSchedule();
        }
    }   
}
