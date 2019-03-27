using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LeaveYourCouch.Mvc.Controllers;

namespace LeaveYourCouch.Mvc.Business.Services
{
    public class ViewBagMessageFactory : IViewBagMessageFactory
    {
        private Dictionary<ManageMessageId, string> _manageProfileMessages;

        public ViewBagMessageFactory()
        {
            _manageProfileMessages = new Dictionary<ManageMessageId, string>
            {
                { ManageMessageId.ChangePasswordSuccess , "Your password has been changed."},
                { ManageMessageId.SetPasswordSuccess , "Your password has been set." },
                {ManageMessageId.SetTwoFactorSuccess ,"Your two-factor authentication provider has been set."},
                { ManageMessageId.Error , "An error has occurred."},
                { ManageMessageId.AddPhoneSuccess,  "Your phone number was added." },
                { ManageMessageId.RemovePhoneSuccess , "Your phone number was removed." },
                { ManageMessageId.PersonnalInfos , "Your personal infos have been updated."},
                {ManageMessageId.EmailconfirmationSent , "Confirmation e-mail has been successfuly sent." }
            };
        }

        public string ManageMessage(ManageMessageId? message)
        {
            string messageDisplay = string.Empty;
            if (message != null)
            {
                _manageProfileMessages.TryGetValue(message.Value, out messageDisplay);
            }

            return messageDisplay;
        }
    }

    public interface IViewBagMessageFactory
    {
        string ManageMessage(ManageMessageId? message);
    }
}