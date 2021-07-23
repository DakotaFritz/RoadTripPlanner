using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using GoogleApi.Entities.Places.Search;
using Newtonsoft.Json;

namespace RoadTripPlannerProject

{
    public class LocationWithGeoCode
    {
        public string Status { get; set; }
        public string Location;
        public string PlaceId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public LocationWithGeoCode(string status, string location, string placeId, double longitude, double latitude)
        {
            Status = status;
            Location = location;
            PlaceId = placeId;
            Longitude = longitude;
            Latitude = latitude;
        }

    }
}
