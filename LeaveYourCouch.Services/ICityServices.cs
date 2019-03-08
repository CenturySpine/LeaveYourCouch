using System.Threading.Tasks;
using LeaveYourCouch.Models.GooglePlaceApiModels;

namespace LeaveYourCouch.Services
{
    public interface ICityServices
    {
        Task<nearbyplaces> SearchCity(string input);
    }
}