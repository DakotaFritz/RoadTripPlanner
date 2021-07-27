using System;
using Newtonsoft.Json;
namespace RoadTripPlannerProject
{
    public class IpGeoCodeApiResponse
    {
        public class Root
        {
            [JsonProperty(PropertyName = "ip")]
            public string Ip { get; set; }

            [JsonProperty(PropertyName = "hostname")]
            public string HostName { get; set; }

            [JsonProperty(PropertyName = "city")]
            public string City { get; set; }

            [JsonProperty(PropertyName = "region")]
            public string Region { get; set; }

            [JsonProperty(PropertyName = "country")]
            public string Country { get; set; }

            [JsonProperty(PropertyName = "loc")]
            public string Location { get; set; }

            [JsonProperty(PropertyName = "postal")]
            public string Zip { get; set; }

            [JsonProperty(PropertyName = "timezone")]
            public string TimeZone { get; set; }
        }
    }
}
