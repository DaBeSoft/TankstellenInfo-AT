using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TankstellenInfo_AT.UserControls;

namespace TankstellenInfo_AT.Models
{
    class Favorite
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Name { get; set; }

        public string AddressString { get; set; }
    }
}
