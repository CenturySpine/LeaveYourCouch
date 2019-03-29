using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Threading.Tasks;
using LeaveYourCouch.Mvc.Business;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LeaveYourCouch.Mvc.Models
{
    public enum ParticipationStatus
    {
        Confirmed,
        WaitingList
    }

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public string FirstName { get; set; }
        public string Address { get; set; }
        public string Pseudo { get; set; }
        public Gender Gender { get; set; }
        public string ProfilePictureName { get; set; }
    }


    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ApplicationUser Owner { get; set; }
        public int MaxSeats { get; set; }
        
        public string Address { get; set; }
        public string MeetingDetails { get; set; }
        public bool IsPrivate { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        //public EventRelativeToUserInformation UserSpecificData { get; set; }
    }

    public class EventParticipation
    {
        public int Id { get; set; }
        public Event Event { get; set; }
        public ApplicationUser User { get; set; }
        public ParticipationStatus Status { get; set; }
        public DateTime SubscriptionTime { get; set; }

    }
    public class EventRelativeToUserInformation
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public Event Event { get; set; }
        public string  Unit { get; set; }
        public double Distance  { get; set; }
        public double Duration { get; set; }
        public DirectionModes DirectionMode { get; set; }
        public string MapLink { get; set; }
    }

    public class UserRelationship
    {
        public int Id { get; set; }
        public ApplicationUser Issuer { get; set; }
        public ApplicationUser Recipient { get; set; }
        public RelationshipStatus Status { get; set; }
    }

    public enum RelationshipStatus
    {
        Pending,
        Accepted,
        Rejected,
        Blacklisted
    }

    public enum Gender
    {
        Undefined,
        Female,
        Male,
    }
}