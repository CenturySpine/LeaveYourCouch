using System.Threading.Tasks;
using LeaveYourCouch.Mvc.GooglePlaceApiModels;

namespace LeaveYourCouch.Mvc.Business.Services
{
    public interface ICityServices
    {
        Task<nearbyplaces> SearchCity(string input);
    }
}