using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using GoogleApi.Entities.Places.Search;
using Newtonsoft.Json;

namespace RoadTripPlannerProject

{
    public class CurrentLocation
    {
        public string Location;
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public static CurrentLocation CallApi(string location, string apiKey)
        {
            string geoCodeApiLink = String.Format("https://maps.googleapis.com/maps/api/geocode/json?");
            string locationQuery = location.Replace(" ", "+");
            using (var webClient = new WebClient())
            {
                string apiResponse = webClient.DownloadString($"{geoCodeApiLink}address={locationQuery}&key={apiKey}");
                GeoCodeApiResponse.RootObject ResponseObjects = JsonConvert.DeserializeObject<GeoCodeApiResponse.RootObject>(apiResponse);
                CurrentLocation currentLocation = new CurrentLocation(ResponseObjects.Results1[0].AddressComponents[0].short_name, ResponseObjects.Results1[0].Geometry.Location.Lng, ResponseObjects.Results1[0].Geometry.Location.Lat) ;
                return currentLocation;
            }
        }

public CurrentLocation(string location, double longitude, double latitude)
        {
            Location = location;
            Longitude = longitude;
            Latitude = latitude;
        }

    }
}
