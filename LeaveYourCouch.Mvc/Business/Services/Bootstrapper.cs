namespace LeaveYourCouch.Mvc.Business.Services
{
    public class Bootstrapper
    {
        public static ICityServices CityService() { return new CityServices(); }
    }
}
