﻿using System;
using System.Collections.Generic;

namespace RoadTripPlannerProject
{
    public class Directions
    {
        public string Distance { get; set; }
        public string Duration { get; set; }
        public string RouteSummary { get; set; }
        public IEnumerable<PolyLineCoordinates> Points { get; set; }

        public static IEnumerable<PolyLineCoordinates> Decode(string encodedPoints)
        {
            if (string.IsNullOrEmpty(encodedPoints))
                throw new ArgumentNullException("encodedPoints");

            char[] polylineChars = encodedPoints.ToCharArray();
            int index = 0;

            int currentLat = 0;
            int currentLng = 0;
            int next5bits;
            int sum;
            int shifter;

            while (index < polylineChars.Length)
            {
                // calculate next latitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylineChars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length)
                    break;

                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                //calculate next longitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylineChars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length && next5bits >= 32)
                    break;

                currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                yield return new PolyLineCoordinates(Convert.ToDouble(currentLng) / 1E5, Convert.ToDouble(currentLat) / 1E5);
            }
        }

        public Directions(string distance, string duration, string routeSummary, IEnumerable<PolyLineCoordinates> points)
        {
            Distance = distance;
            Duration = duration;
            RouteSummary = routeSummary;
            Points = points;
        }
    }
}