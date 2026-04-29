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
    public class FundraisingService : System.Web.Services.WebService
    {
        // Returns progress percentage toward a fundraising goal
        [WebMethod(Description = "Returns the percentage progress toward a fundraising goal.")]
        public string GetFundraisingProgress(double goal, double raised)
        {
            if (goal <= 0)
                return "Error: Goal must be greater than zero.";
            if (raised < 0)
                return "Error: Amount raised cannot be negative.";

            double percentage = (raised / goal) * 100;
            percentage = Math.Min(percentage, 100); // cap at 100%

            return string.Format(
                "Goal: ${0:F2} | Raised: ${1:F2} | Progress: {2:F1}%",
                goal, raised, percentage
            );
        }

        // Returns a status message based on progress
        [WebMethod(Description = "Returns a motivational status message based on fundraising progress.")]
        public string GetFundraisingStatus(double goal, double raised)
        {
            if (goal <= 0)
                return "Error: Goal must be greater than zero.";

            double percentage = (raised / goal) * 100;

            if (percentage >= 100)
                return "🎉 Goal reached! Great work team!";
            else if (percentage >= 75)
                return "Almost there! Over 75% funded!";
            else if (percentage >= 50)
                return "Halfway there! Keep pushing!";
            else if (percentage >= 25)
                return "Good start! 25% funded so far.";
            else
                return "Just getting started — every dollar counts!";
        }
    }
}