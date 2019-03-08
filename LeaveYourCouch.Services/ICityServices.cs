using System.Threading.Tasks;
using LeaveYourCouch.Models.GooglePlaceApiModels;

namespace LeaveYourCouch.Services
{
    public interface ICityServices
    {
        Task<object> SearchCity(string input);
    }
}