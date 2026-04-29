using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Assignment6.Models
{
    public class SwimEvent
    {
        public string EventName { get; set; }
        public string SwimmerName { get; set; }
        public double TimeInSeconds { get; set; }
        public int Points { get; set; }

        // Constructor
        public SwimEvent(string eventName, string swimmerName, double timeInSeconds)
        {
            EventName = eventName;
            SwimmerName = swimmerName;
            TimeInSeconds = timeInSeconds;
            Points = CalculatePoints(timeInSeconds);
        }

        // Points calculated based on time — faster = more points
        private int CalculatePoints(double time)
        {
            if (time <= 30) return 100;
            else if (time <= 45) return 80;
            else if (time <= 60) return 60;
            else if (time <= 90) return 40;
            else return 20;
        }
    }
}
