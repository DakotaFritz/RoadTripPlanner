using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RoadTripPlannerProject
{
    public class SpeedLimitApiResponse
    {
        public class SpeedLimits
        {
            [JsonProperty("placeId")]
            public string PlaceId { get; set; }

            [JsonProperty("speedLimit")]
            public int SpeedLimit { get; set; }

            [JsonProperty("units")]
            public string Units { get; set; }
        }

        public class Location
        {
            [JsonProperty("latitude")]
            public double Latitude { get; set; }

            [JsonProperty("longitude")]
            public double Longitude { get; set; }
        }

        public class SnappedPoint
        {
            [JsonProperty("location")]
            public Location Location { get; set; }

            [JsonProperty("originalIndex")]
            public int OriginalIndex { get; set; }

            [JsonProperty("placeId")]
            public string PlaceId { get; set; }
        }

        public class Root
        {
            [JsonProperty("speedLimits")]
            public List<SpeedLimits> SpeedLimits { get; set; }

            [JsonProperty("snappedPoints")]
            public List<SnappedPoint> SnappedPoints { get; set; }

            [JsonProperty("warningMessage")]
            public string WarningMessage { get; set; }
        }

        public SpeedLimitApiResponse()
        {
        }
    }
}
