﻿using System.Text;

namespace astronomy
{
    internal class DailyScheduler
    {
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int Duration { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int TimeZone { get; set; }

        public DailyScheduler(int day, int month, int year, int duration, double longitude, double latitude, int timeZone = 0)
        {
            Day = day;
            Month = month;
            Year = year;
            Duration = duration;
            Longitude = longitude;
            Latitude = latitude;
            TimeZone = timeZone;
        }

        public string HrsMin(double hours)
        {
            var hrs = Math.Floor(hours * 60 + 0.5) / 60.0;
            var h = Math.Floor(hrs);
            var m = Math.Floor(60 * (hrs - h) + 0.5);
            StringBuilder sb = new();
            var dum = h * 100 + m;
            sb.Append(dum);



            if (dum < 1000) sb.Insert(0, "0");
            if (dum < 100) sb.Insert(0, "0");
            if (dum < 10) sb.Insert(0, "0");

            return sb.ToString();
        }

        public double Ipart(double x)
        {
            if (x > 0)
            {
                return Math.Floor(x);
            }

            return Math.Ceiling(x);
        }

        public double Frac(double x)
        {
            var decimalPart = x - Math.Floor(x);
            return decimalPart < 0 ? decimalPart + 1 : decimalPart;
        }

        public long Round(long num, long dp)
        {
            return (long)(Math.Round(num * Math.Pow(10, dp)) / Math.Pow(10, dp));
        }

        public double Range(double x)
        {
            var b = x / 360;
            var a = 360 * (b - Ipart(b));

            return a < 0 ? a + 360 : a;
        }

        public double Mjd(long hour)
        {
            var useMonth = Month;
            var useYear = Year;
            if (useMonth <= 2)
            {
                useMonth += 12;
                useYear--;
            }

            double a = 10000.0 * useYear + 100.0 * useYear + Day;
            double b = Math.Floor(useYear / 400.0) - Math.Floor(useYear / 100.0) + Math.Floor(useYear / 4.0); ;
            if (a <= 15821004.1)
            {
                b = -2 * Math.Floor((useYear + 4716) / 4.0) - 1179;
            }
            a = 365.0 * useYear - 679004.0;

            return a + b + Math.Floor(30.6001 * (useMonth + 1.0)) + Day + hour / 24.0;
        }

        public long Caldat(double mjd)
        {
            var jd = mjd + 2400000.5;
            var jd0 = Math.Floor(jd + 0.5);

            var b = Math.Floor((jd0 - 1867216.25) / 36524.25);
            double c = jd0 + (b - Math.Floor(b / 4)) + 1525.0;
            if (jd0 < 2299161.0) c = jd0 + 1524.0;

            var d = Math.Floor((c - 122.1) / 365.25);
            var e = 365.0 * d + Math.Floor(d / 4);
            var f = Math.Floor((c - e) / 30.6001);
            var day = Math.Floor(c - e + 0.5) - Math.Floor(30.6001 * f);
            var month = f - 1 - 12 * Math.Floor(f / 14);
            var year = d - 4715 - Math.Floor((7 + month) / 10);
            var hour = 24.0 * (jd + 0.5 - jd0);
            var stringHour = HrsMin(hour);

            return Round((long)(year * 10000.0 + month * 100.0 + day + Convert.ToDouble(stringHour) / 10000), 4);
        }

        public double[] Quad(double ym, double yz, double yp)
        {
            var a = 0.5 * (ym + yp) - yz;
            var b = 0.5 * (yp - ym);
            var c = yz;
            var xe = -b / (2 * a);
            var ye = (a * xe + b) * xe + c;
            var dis = b * b - 4.0 * a * c;

            double z1 = 0, z2 = 0, nz = 0;
            if (dis > 0)
            {
                var dx = (0.5 * Math.Sqrt(dis)) / Math.Abs(a);
                z1 = xe - dx;
                z2 = xe + dx;
                if (Math.Abs(z1) <= 1.0) nz += 1;
                if (Math.Abs(z2) <= 1.0) nz += 1;
                if (z1 < -1.0) z1 = z2;
            }

            return new double[5] { nz, z1, z2, xe, ye };
        }

        public double Lmst(double mjd)
        {
            var d = mjd - 51544.5;
            var t = d / 36525.0;
            var lst = Range(
              280.46061837 +
                360.98564736629 * d +
                0.000387933 * t * t -
                (t * t * t) / 38710000
            );

            return lst / 15.0 + Longitude / 15;
        }

        public double[] MiniSun(double t)
        {
            var p2 = 6.283185307;
            var Coseps = 0.91748;
            var Sineps = 0.39778;

            var M = p2 * Frac(0.993133 + 99.997361 * t);
            var DL = 6893.0 * Math.Sin(M) + 72.0 * Math.Sin(2 * M);
            var L = p2 * Frac(0.7859453 + M / p2 + (6191.2 * t + DL) / 1296000);

            var SL = Math.Sin(L);
            var X = Math.Cos(L);
            var Y = Coseps * SL;
            var Z = Sineps * SL;

            var RHO = Math.Sqrt(1 - Z * Z);
            var dec = (360.0 / p2) * Math.Atan(Z / RHO);

            var ra = (48.0 / p2) * Math.Atan(Y / (X + RHO));
            if (ra < 0) ra += 24;

            double[] suneq = new double[] { 0, dec, ra };

            return suneq;
        }

        public double[] MiniMoon(double t)
        {
            var p2 = 6.283185307;
            var arc = 206264.8062;
            var Coseps = 0.91748;
            var Sineps = 0.39778;

            var L0 = Frac(0.606433 + 1336.855225 * t);
            var L = p2 * Frac(0.374897 + 1325.55241 * t);
            var LS = p2 * Frac(0.993133 + 99.997361 * t);
            var D = p2 * Frac(0.827361 + 1236.853086 * t);
            var F = p2 * Frac(0.259086 + 1342.227825 * t);

            var DL = 22640 * Math.Sin(L);
            DL += -4586 * Math.Sin(L - 2 * D);
            DL += +2370 * Math.Sin(2 * D);
            DL += +769 * Math.Sin(2 * L);
            DL += -668 * Math.Sin(LS);
            DL += -412 * Math.Sin(2 * F);
            DL += -212 * Math.Sin(2 * L - 2 * D);
            DL += -206 * Math.Sin(L + LS - 2 * D);
            DL += +192 * Math.Sin(L + 2 * D);
            DL += -165 * Math.Sin(LS - 2 * D);
            DL += -125 * Math.Sin(D);
            DL += -110 * Math.Sin(L + LS);
            DL += +148 * Math.Sin(L - LS);
            DL += -55 * Math.Sin(2 * F - 2 * D);

            var S = F + (DL + 412 * Math.Sin(2 * F) + 541 * Math.Sin(LS)) / arc;
            var H = F - 2 * D;

            var N = -526 * Math.Sin(H);
            N += +44 * Math.Sin(L + H);
            N += -31 * Math.Sin(-L + H);
            N += -23 * Math.Sin(LS + H);
            N += +11 * Math.Sin(-LS + H);
            N += -25 * Math.Sin(-2 * L + F);
            N += +21 * Math.Sin(-L + F);

            var LMoon = p2 * Frac(L0 + DL / 1296000);
            var BMoon = (18520.0 * Math.Sin(S) + N) / arc;
            var CB = Math.Cos(BMoon);
            var X = CB * Math.Cos(LMoon);
            var V = CB * Math.Sin(LMoon);
            var W = Math.Sin(BMoon);
            var Y = Coseps * V - Sineps * W;
            var Z = Sineps * V + Coseps * W;
            var RHO = Math.Sqrt(1.0 - Z * Z);
            var dec = (360.0 / p2) * Math.Atan(Z / RHO);

            var ra = (48.0 / p2) * Math.Atan(Y / (X + RHO));
            if (ra < 0) ra += 24;

            double[] mooneq = new double[] { 0, dec, ra };
            return mooneq;
        }

        public double SinAlt(double iobj, double mjd0, double hour, double cglat, double sglat)
        {
            var rads = 0.0174532925;

            double[] objpos;
            var mjd = mjd0 + hour / 24.0;
            var t = (mjd - 51544.5) / 36525.0;
            if (iobj == 1)
            {
                objpos = MiniMoon(t);
            }
            else
            {
                objpos = MiniSun(t);
            }

            var ra = objpos[2];
            var dec = objpos[1];

            var tau = 15.0 * (Lmst(mjd) - ra);

            return (
              sglat * Math.Sin(rads * dec) +
              cglat * Math.Cos(rads * dec) * Math.Cos(rads * tau)
            );
        }

        public List<string> GetSchedule()
        {
            List<string> stringTimes = [];

            for (int i = 0; i < Duration; i++)
            {
                foreach (var time in FindTimes(i))
                {

                    stringTimes.Add(time.ToString());
                }
            }

            return stringTimes;
        }

        public string[] FindTimes(int delta = 0)
        {
            var mjd = Mjd(-TimeZone) + delta;
            var rads = 0.0174532925;
            var Sinho = new double[]
            {
                Math.Sin(rads * -0.833), // sunrise/sunset
                //Math.Sin(rads * -6.0), // civic twilight
                //Math.Sin(rads * -12.0), // nautical twilight
                //Math.Sin(rads * -9.0) // METEOR
            };

            var sglat = Math.Sin(rads * Latitude);
            var cglat = Math.Cos(rads * Latitude);
            var date = mjd - TimeZone / 24;

            string[] s = new string[2];

            for (int j = 0; j < Sinho.Length; j++)
            {

                var hour = 1.0;
                var ym = SinAlt(2, date, hour - 1.0, cglat, sglat) - Sinho[j];

                double utrise = 0, utset = 0;
                bool rise = false;
                bool dusk = false;
                while (hour < 25 && (dusk == false || rise == false))
                {
                    var yz = SinAlt(2, date, hour, cglat, sglat) - Sinho[j];
                    var yp = SinAlt(2, date, hour + 1.0, cglat, sglat) - Sinho[j];
                    var quadout = Quad(ym, yz, yp);
                    var nz = quadout[0];
                    var z1 = quadout[1];
                    var z2 = quadout[2];
                    var xe = quadout[3];
                    var ye = quadout[4];

                    if (nz == 1)
                    {
                        if (ym < 0.0)
                        {
                            utrise = hour + z1;
                            rise = true;
                        }
                        else
                        {
                            utset = hour + z1;
                            dusk = true;
                        }
                    }

                    if (nz == 2)
                    {
                        if (ye < 0.0)
                        {
                            utrise = hour + z2;
                            utset = hour + z1;
                        }
                        else
                        {
                            utrise = hour + z1;
                            utset = hour + z2;
                        }
                    }

                    ym = yp;
                    hour += 2.0;
                }

                s[0] = rise ? HrsMin(utrise) : "";
                s[1] = rise ? HrsMin(utset) : "";
            }

            return s;
        }
    }
}