using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment6.Models
{
    public class UserProfile
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PreferredEvent { get; set; }

        // Constructor
        public UserProfile(string fullName, string email, string preferredEvent)
        {
            FullName = fullName;
            Email = email;
            PreferredEvent = preferredEvent;
        }
    }
}