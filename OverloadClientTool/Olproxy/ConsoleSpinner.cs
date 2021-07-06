using System;

namespace olproxy
{
    class ConsoleSpinner
    {
        private static readonly char[] spinChars = new[] { '/', '-', '\\', '|' };
        //private int idx;
        public DateTime LastUpdateTime = DateTime.UtcNow;
        public bool Active = false;

        public void Spin()
        {
            // -- Maestro change start --
            return;
            // -- Maestro change end --

            /*
            Console.Write(new[] { spinChars[idx], '\r' });
            idx = (idx + 1) % spinChars.Length;
            LastUpdateTime = DateTime.UtcNow;
            Active = true;
            */
        }

        public void Clear()
        {
            // -- Maestro change start --
            return;
            // -- Maestro change end  --

            //Console.Write(" \r");
            //Active = false;
        }
    }
}
