using System;
using System.Collections.Generic;

namespace RoadTripPlannerProject
{
    public class Directions
    {
        public string Distance { get; set; }
        public string Duration { get; set; }
        public string RouteSummary { get; set; }
        public List<double> Points { get; set; }

        public Directions(string distance, string duration, string routeSummary)
        {
            Distance = distance;
            Duration = duration;
            RouteSummary = routeSummary;
            //Points = points;
        }
    }
}
