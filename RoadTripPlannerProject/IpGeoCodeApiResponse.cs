using System;
using Newtonsoft.Json;
namespace RoadTripPlannerProject
{
    public class IpGeoCodeApiResponse
    {
        public class Root
        {
            [JsonProperty(PropertyName = "geoplugin_request")]
            public string Request { get; set; }

            [JsonProperty(PropertyName = "geoplugin_status")]
            public int Status { get; set; }

            [JsonProperty(PropertyName = "geoplugin_delay")]
            public string Delay { get; set; }

            [JsonProperty(PropertyName = "geoplugin_credit")]
            public string Credit { get; set; }

            [JsonProperty(PropertyName = "geoplugin_city")]
            public string City { get; set; }

            [JsonProperty(PropertyName = "geoplugin_region")]
            public string Region { get; set; }

            [JsonProperty(PropertyName = "geoplugin_regionCode")]
            public string RegionCode { get; set; }

            [JsonProperty(PropertyName = "geoplugin_regionName")]
            public string RegionName { get; set; }

            [JsonProperty(PropertyName = "geoplugin_areaCode")]
            public string AreaCode { get; set; }

            [JsonProperty(PropertyName = "geoplugin_dmaCode")]
            public string DmaCode { get; set; }

            [JsonProperty(PropertyName = "geoplugin_countryCode")]
            public string CountryCode { get; set; }

            [JsonProperty(PropertyName = "geoplugin_countryName")]
            public string CountryName { get; set; }

            [JsonProperty(PropertyName = "geoplugin_latitude")]
            public string Latitude { get; set; }

            [JsonProperty(PropertyName = "geoplugin_longitude")]
            public string Longitude { get; set; }

            [JsonProperty(PropertyName = "geoplugin_locationAccuracyRadius")]
            public string LocationAccuracyRadius { get; set; }

            [JsonProperty(PropertyName = "geoplugin_timezone")]
            public string TimeZone { get; set; }
        }
    }
}
