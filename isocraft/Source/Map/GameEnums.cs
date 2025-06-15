
using System.Collections.Generic;

namespace isocraft
{
    public static class GameEnums
    {
        public static Dictionary<string, int> TrapDictionary = new Dictionary<string, int>();

        public static void Init()
        {
           // TrapDictionary["abc"] = 1;

        }




        public enum Type
        { 
            None = 0,
            Hero = 1,
            Enemy = 2,
            Tile = 3,
            Wall = 4,
        }

        public enum Hero
        { 
            male = 0,
        
        }

        public enum Enemy
        {
            zombie = 0,

        }




    }
}
