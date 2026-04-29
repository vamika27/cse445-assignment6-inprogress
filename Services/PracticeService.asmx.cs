using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Assignment6.Services
{
    //[WebService(Namespace = "http://tempuri.org/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    //[System.ComponentModel.ToolboxItem(false)]

    [WebService(Namespace = "http://vamikaswimclub.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class PracticeService : System.Web.Services.WebService
    {

        // Returns practice schedule for a given day
        [WebMethod(Description = "Returns the practice schedule for a given day of the week.")]
        public string GetPracticeSchedule(string day)
        {
            if (string.IsNullOrEmpty(day))
                return "Error: No day provided.";

            switch (day.Trim().ToLower())
            {
                case "monday":
                    return "Monday: 6:00 AM - 8:00 AM | Drills & Endurance | Coach Smith | Pool A";
                case "tuesday":
                    return "Tuesday: 5:30 AM - 7:30 AM | Sprint Training | Coach Lee | Pool B";
                case "wednesday":
                    return "Wednesday: 6:00 AM - 8:00 AM | Technique & Turns | Coach Smith | Pool A";
                case "thursday":
                    return "Thursday: 5:30 AM - 7:30 AM | Race Simulation | Coach Lee | Pool B";
                case "friday":
                    return "Friday: 6:00 AM - 7:30 AM | Recovery & Stretching | Coach Smith | Pool A";
                case "saturday":
                    return "Saturday: 7:00 AM - 10:00 AM | Time Trials & Competition Prep | All Coaches | Pool A & B";
                case "sunday":
                    return "Sunday: No practice scheduled. Rest day.";
                default:
                    return "Error: Invalid day. Please enter a day of the week (e.g. Monday).";
            }
        }

        // Returns all practice days as a list
        [WebMethod(Description = "Returns all days that have scheduled practices.")]
        public string GetAllPracticeDays()
        {
            return "Practice Days: Monday, Tuesday, Wednesday, Thursday, Friday, Saturday";
        }
    }
}
