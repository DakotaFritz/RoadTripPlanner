using System;
using System.IO;

namespace RoadTripPlannerProject
{
    public class FileWriter
    {
        public static void WriteToFile(string origin, string destination, string distance, string duration, string routeSummary)
        {
            string[] lines =
        {
            origin, destination, distance, duration, routeSummary
        };
            File.WriteAllLines("directions.txt", lines);
        }
    }
}
