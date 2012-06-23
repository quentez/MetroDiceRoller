using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetroDiceRoller.Helpers
{
    public class RandomHelpers
    {
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();

        public static int Next()
        {
            lock (syncLock)
            {
                return random.Next();
            }
        }

        public static int Next(int max)
        {
            lock (syncLock)
            {
                return random.Next(max);
            }
        }

        public static int Next(int min, int max)
        {
            lock (syncLock)
            {
                return random.Next(min, max);
            }
        }
    }
}
