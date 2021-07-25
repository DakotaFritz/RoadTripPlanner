using System;
using System.Collections.Generic;

namespace RoadTripPlannerProject
{
    public class Directions
    {
        public string Status { get; set; }
        public string Distance { get; set; }
        public string Duration { get; set; }
        public string RouteSummary { get; set; }
        public IEnumerable<PolyLineCoordinates> Points { get; set; }

        /// <summary>
        /// This method is to decode Google's encoded polyline strings based on their polyline encoding algorithm. For more on that algorithm, see here: https://developers.google.com/maps/documentation/utilities/polylinealgorithm.
        /// Inspiration and most of the code for this method was found here: https://gist.github.com/shinyzhu/4617989.
        /// </summary>
        /// <param name="encodedPoints">This is the encoded string of that contains the points on a directions route returned from the Directions API</param>
        /// <returns>IEnumerable of PolyLineCoordinates is returned</returns>
        public static IEnumerable<PolyLineCoordinates> Decode(string encodedPoints)
        {
            // Puts each character in string into an array item
            char[] polylineChars = encodedPoints.ToCharArray();
            int index = 0;

            int currentLat = 0;
            int currentLng = 0;
            int next5bits;
            int sum;
            int shifter;

            while (index < polylineChars.Length)
            {
                // Calculates next latitude
                sum = 0;
                shifter = 0;
                do
                {
                    // Converts character to int and subtracts 63 from that number, per algorithm
                    next5bits = (int)polylineChars[index++] - 63;
                    // Converts to bit and shifts left 5 digits
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length)
                    break;

                // If negative, invert the encoding
                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                //Calculate next longitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylineChars[index++] - 63;
                    // Converts to bit and shifts left 5 digits
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylineChars.Length);

                if (index >= polylineChars.Length && next5bits >= 32)
                    break;

                // If negative, invert the encoding
                currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                // Dividing by 1E5 rounds to give precision to the nummbers
                yield return new PolyLineCoordinates(Convert.ToDouble(currentLng) / 1E5, Convert.ToDouble(currentLat) / 1E5);
            }
        }

        public Directions(string status, string distance, string duration, string routeSummary, IEnumerable<PolyLineCoordinates> points)
        {
            Status = status;
            Distance = distance;
            Duration = duration;
            RouteSummary = routeSummary;
            Points = points;
        }
    }
}
