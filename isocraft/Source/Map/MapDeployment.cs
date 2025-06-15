using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace isocraft
{
    public class MapDeployment
    {

        private static MapDeployment _instance;


        private MapDeployment()
        {

        }

        public static MapDeployment Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MapDeployment();
                return _instance;
            }
        }



    }
}
