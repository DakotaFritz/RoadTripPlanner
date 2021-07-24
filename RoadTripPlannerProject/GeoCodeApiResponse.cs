using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RoadTripPlannerProject
{
    public class GeoCodeApiResponse
    {

        public class Location
        {
            [JsonProperty(PropertyName = "lat")]
            public double Lat { get; set; }
            [JsonProperty(PropertyName = "lng")]
            public double Lng { get; set; }
        }

        public class Geometry
        {
            [JsonProperty(PropertyName = "location")]
            public Location Location { get; set; }
        }

        public class Results
        {
            [JsonProperty(PropertyName = "formatted_address")]
            public string FormattedAddress { get; set; }
            [JsonProperty(PropertyName = "geometry")]
            public Geometry Geometry { get; set; }
            [JsonProperty(PropertyName = "place_id")]
            public string PlaceId { get; set; }

        }

        public class RootObject
        {
            [JsonProperty(PropertyName = "results")]
            public List<Results> Results { get; set; }
            [JsonProperty(PropertyName = "status")]
            public string Status { get; set; }
        }

    }
}
