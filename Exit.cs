using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using astronomy.Performables;

namespace astronomy
{
    internal class Exit : IPerformable
    {
        public static void Perform()
        {
            Console.WriteLine("Goodbye!");
        }
    }
}
