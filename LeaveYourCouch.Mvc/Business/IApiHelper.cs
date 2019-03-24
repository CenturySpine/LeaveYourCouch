using System.Threading.Tasks;
using LeaveYourCouch.Mvc.GooglePlaceApiModels.DirectionApi;

namespace LeaveYourCouch.Mvc.Business
{
    public interface IApiHelper
    {
        Task<DirectionObject> GetDirections(string userAdress, string eventAddress,
            string unit, DirectionModes md);
        string GenerateMapLink(string usrPostalCode, string targeteventAddress, DirectionModes mode);
    }
}