using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace isocraft
{
    public class WorldTimer
    {
        private static WorldTimer _instance;
        private static double Time = 0f;

        private WorldTimer()
        { 
        
        }

        public static WorldTimer Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new WorldTimer();

                return _instance;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add_Time(double gametime)
        {
            Time += gametime;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double totalTime()
        {
            return Time;
        }

    }
}
