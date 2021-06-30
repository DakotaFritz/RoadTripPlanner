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
        public string Location;
        public string PlaceId { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public static LocationWithGeoCode CallGeoCodeApi(string location, string apiKey)
        {
            string GeoCodeApiLink = "https://maps.googleapis.com/maps/api/geocode/json?";
            string LocationQuery = location.Replace(" ", "+");
            using (var webClient = new WebClient())
            {
                string apiResponse = webClient.DownloadString($"{GeoCodeApiLink}address={LocationQuery}&key={apiKey}");
                GeoCodeApiResponse.RootObject ResponseObjects = JsonConvert.DeserializeObject<GeoCodeApiResponse.RootObject>(apiResponse);
                LocationWithGeoCode currentLocation = new LocationWithGeoCode(ResponseObjects.Results1[0].FormattedAddress, ResponseObjects.Results1[0].PlaceId, ResponseObjects.Results1[0].Geometry.Location.Lng, ResponseObjects.Results1[0].Geometry.Location.Lat) ;
                return currentLocation;
            }
        }

        public static Directions CallDirectionsApi(string currentLocation, string destination, string apiKey)
        {
            string DirectionsApiLink = "https://maps.googleapis.com/maps/api/directions/json?";
            string CurrentLocation = currentLocation.Replace(" ", "+");
            string Destination = destination.Replace(" ", "+");
            using (var webClient = new WebClient())
            {
                string apiResponse = webClient.DownloadString($"{DirectionsApiLink}origin=place_id:{CurrentLocation}&destination=place_id:{Destination}&key={apiKey}");
                DirectionsApiResponse.Root ResponseObjects = JsonConvert.DeserializeObject<DirectionsApiResponse.Root>(apiResponse);
                Console.WriteLine($"It looks like you'll arrive in approximately {ResponseObjects.routes[0].legs[0].duration.text}");
                return new Directions(ResponseObjects.routes[0].legs[0].distance.text, ResponseObjects.routes[0].legs[0].duration.text, ResponseObjects.routes[0].summary, Directions.Decode(ResponseObjects.routes[0].overview_polyline.points));
            }
        }

        public LocationWithGeoCode(string location, string placeId, double longitude, double latitude)
        {
            Location = location;
            PlaceId = placeId;
            Longitude = longitude;
            Latitude = latitude;
        }

    }
}
