using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Assignment6.Services
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class SwimPaceService : System.Web.Services.WebService
    {

        [WebMethod(Description = "Returns the estimated time for a different pool length.")]
        public double ConvertCourse(double time, string fromUnit, string toUnit)
        {
            double timeInYards = 0;

            if (fromUnit == "25 Yards") timeInYards = time;
            else if (fromUnit == "25 Meters") timeInYards = time / 1.11;
            else if (fromUnit == "50 Meters") timeInYards = (time / 1.02) / 1.11; //account for less turns

            if (toUnit == "25 Yards") time = timeInYards;
            else if (toUnit == "25 Meters") time = timeInYards * 1.11;
            else if (toUnit == "50 Meters") time = (timeInYards * 1.11) * 1.02; //account for less turns
            return time;

        }

        [WebMethod(Description = "Converts a time value into seconds")]
        public double ConvertTimeToSeconds(string time)
        {
            string format = @"mm\:ss\.ff";
            string format2 = @"mm\:ss";
            string format3 = @"m\:ss";
            string format4 = @"m\:ss\.ff";
            double timeInSeconds = 0;
            if (TimeSpan.TryParseExact(time, format, CultureInfo.InvariantCulture, out TimeSpan interval)) {
                timeInSeconds = interval.TotalSeconds;
            } else if (TimeSpan.TryParseExact(time, format2, CultureInfo.InvariantCulture, out TimeSpan interval2))
            {
                timeInSeconds = interval2.TotalSeconds;
            } else if (TimeSpan.TryParseExact(time, format3, CultureInfo.InvariantCulture, out TimeSpan interval3))
            {
                timeInSeconds = interval3.TotalSeconds;
            }
            else if (TimeSpan.TryParseExact(time, format4, CultureInfo.InvariantCulture, out TimeSpan interval4))
            {
                timeInSeconds = interval4.TotalSeconds;
            }
            return timeInSeconds;

        }
        [WebMethod(Description = "Returns the value of the time for the xml file")]
        public string ConvertTimeToString(double seconds)
        {
            string format = @"mm\:ss\.ff";
            
            string time = "";
            time = TimeSpan.FromSeconds(seconds).ToString(format);
            return time;

        }
    }
}

