using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RoadTripPlannerProject
{
    public class GeoCodeApiResponse
    {
        public class AddressComponent
        {
            public string long_name { get; set; }
            public string short_name { get; set; }
            public List<string> types { get; set; }
        }

        public class Bounds
        {
            public Location northeast { get; set; }
            public Location southwest { get; set; }
        }

        public class Location
        {
            [JsonProperty(PropertyName = "lat")]
            public double Lat { get; set; }
            [JsonProperty(PropertyName = "lng")]
            public double Lng { get; set; }
        }

        public class Geometry
        {
            [JsonProperty(PropertyName = "bounds")]
            public Bounds Bounds { get; set; }
            [JsonProperty(PropertyName = "location")]
            public Location Location { get; set; }
            public string location_type { get; set; }
            public Bounds viewport { get; set; }
        }

        public class Results1
        {
            [JsonProperty(PropertyName = "address_components")]
            public List<AddressComponent> AddressComponents { get; set; }
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
            public List<Results1> Results1 { get; set; }
            [JsonProperty(PropertyName = "status")]
            public string Status { get; set; }
        }

        public GeoCodeApiResponse()
        {
        }
    }
}
