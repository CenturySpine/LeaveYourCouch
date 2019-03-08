using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LeaveYourCouch.Services
{
    public class Bootstrapper
    {
        public static ICityServices CityService() { return new CityServices(); }
    }
}
