using Assignment6.Models;
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
    public class SwimEventService : WebService
    {
        // Returns points earned based on swimmer name, event, and time
        [WebMethod(Description = "Calculates points for a swimmer based on their event time.")]
        public int GetSwimmerPoints(string swimmerName, string eventName, double timeInSeconds)
        {
            if (string.IsNullOrEmpty(swimmerName) || string.IsNullOrEmpty(eventName) || timeInSeconds <= 0)
                return -1; // invalid input

            SwimEvent swimEvent = new SwimEvent(eventName, swimmerName, timeInSeconds);
            return swimEvent.Points;
        }

        // Returns a summary string of the swim result
        [WebMethod(Description = "Returns a full result summary for a swimmer's event entry.")]
        public string GetEventResult(string swimmerName, string eventName, double timeInSeconds)
        {
            if (string.IsNullOrEmpty(swimmerName) || string.IsNullOrEmpty(eventName) || timeInSeconds <= 0)
                return "Error: Invalid input provided.";

            SwimEvent swimEvent = new SwimEvent(eventName, swimmerName, timeInSeconds);
            return string.Format(
                "Swimmer: {0} | Event: {1} | Time: {2}s | Points Earned: {3}",
                swimEvent.SwimmerName,
                swimEvent.EventName,
                swimEvent.TimeInSeconds,
                swimEvent.Points
            );
        }
    }
}