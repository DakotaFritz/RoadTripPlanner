using System;
using System.Collections.Generic;
using System.Linq;

namespace RoadTripPlannerProject
{
    public static class NumericExtensions
    {
        /// <summary>
        /// Converts a number (coordinate, in this case) to Radians.
        /// </summary>
        /// <param name="val">This will be used inside the HaversineDistance method and the double is a latitude and longitude</param>
        /// <returns>The coordinate in radians</returns>
        public static double ToRadians(this double val)
        {
            return (Math.PI / 180) * val;
        }

        /// <summary>
        /// Finds the distance between two points on the Earth given the latitude and longitude. For more information on the Haversine Formula, see here: https://en.wikipedia.org/wiki/Haversine_formula.
        /// Source for this method: https://stormconsultancy.co.uk/blog/storm-news/the-haversine-formula-in-c-and-sql/
        /// </summary>
        /// <param name="pos1">Latitude of a point</param>
        /// <param name="pos2">Longitude of a point</param>
        /// <param name="unit">Unit being measured (Mile vs. Km)</param>
        /// <returns>double that is the number of miles between the points</returns>
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

        /// <summary>
        /// Creates a list of points along a path up to a specified distance
        /// </summary>
        /// <param name="points">The list of points along the path of the directions from the API response</param>
        /// <param name="distanceToday">The number of desired miles left to drive today</param>
        /// <returns>A list of points along the path of the directions up to the number of miles in the distanceToday variable</returns>
        public static List<PolyLineCoordinates> DistanceToInput(IEnumerable<PolyLineCoordinates> points, double distanceToday)
        {
            List<PolyLineCoordinates> PointsInDistanceToday = new List<PolyLineCoordinates>();
            double distance = 0;
            var i = 0;
            var j = 1;

            while (distanceToday > distance)
            {
                distance += HaversineDistance(points.ToList()[i], points.ToList()[j], DistanceUnit.Miles);
                PointsInDistanceToday.Add(points.ElementAt(i));
                i++;
                j++;
            }
            return PointsInDistanceToday;
        }

        public enum DistanceUnit { Miles, Kilometers };
    }
}
