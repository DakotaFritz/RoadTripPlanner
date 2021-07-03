using System;
using System.Collections.Generic;
using System.Linq;

namespace RoadTripPlannerProject
{
    public static class NumericExtensions
    {
        public static double ToRadians(this double val)
        {
            return (Math.PI / 180) * val;
        }

        public static double HaversineDistance(PolyLineCoordinates pos1, PolyLineCoordinates pos2, DistanceUnit unit)
        {
            double R = (unit == DistanceUnit.Miles) ? 3960 : 6371;
            var lat = (pos2.Latitude - pos1.Latitude).ToRadians();
            var lng = (pos2.Longitude - pos1.Longitude).ToRadians();
            var h1 = Math.Sin(lat / 2) * Math.Sin(lat / 2) +
                          Math.Cos(pos1.Latitude.ToRadians()) * Math.Cos(pos2.Latitude.ToRadians()) *
                          Math.Sin(lng / 2) * Math.Sin(lng / 2);
            var h2 = 2 * Math.Asin(Math.Min(1, Math.Sqrt(h1)));
            return R * h2;
        }

        public static List<PolyLineCoordinates> DistanceToInput(IEnumerable<PolyLineCoordinates> points, double distanceToday)
        {
            List<PolyLineCoordinates> PointsInDistanceToday = new List<PolyLineCoordinates>();
            double distance = 0;

            for (var i=0; i < points.Count(); i++)
            {
                for (var j=1; j< points.Count(); j++)
                {
                    while (distanceToday > distance)
                    {
                        distance += HaversineDistance(points.ToList()[i], points.ToList()[j], DistanceUnit.Miles);
                        PointsInDistanceToday.Add(points.ToList()[i]);
                    }
                }
            }
            return PointsInDistanceToday;
        }

        public enum DistanceUnit { Miles, Kilometers };
    }
}
