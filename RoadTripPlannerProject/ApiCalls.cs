using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace RoadTripPlannerProject
{
    public class ApiCalls
    {
        private static string[] GeoCodeErrArr = { "ZERO_RESULTS", "OVER_DAILY_LIMIT", "OVER_QUERY_LIMIT", "REQUEST_DENIED", "INVALID_REQUEST", "UNKNOWN_ERROR" };
        public static List<string> GeoCodeErr = new List<string>(GeoCodeErrArr);

        private static string[] DirErrArr = { "NOT_FOUND", "ZERO_RESULTS", "MAX_WAYPOINTS_EXCEEDED", "MAX_ROUTE_LENGTH_EXCEEDED", "INVALID_REQUEST", "OVER_DAILY_LIMIT", "OVER_QUERY_LIMIT", "REQUEST_DENIED", "UNKNOWN_ERROR" };
        public static List<string> DirErr = new List<string>(DirErrArr);

        public static LocationWithGeoCode CallIpGeoCodeApi(string ip)
        {
            string IpGeoCodeApiLink = "http://www.geoplugin.net/json.gp?ip=";
            using (var webClient = new WebClient())
            {
                string apiResponse = webClient.DownloadString($"{IpGeoCodeApiLink}{ip}");
                IpGeoCodeApiResponse.Root ResponseObjects = JsonConvert.DeserializeObject<IpGeoCodeApiResponse.Root>(apiResponse);
                if (ResponseObjects.Status == 200)
                {
                    var LatDoubParsed = double.TryParse(ResponseObjects.Latitude, out double LatParsed);
                    var LngDoubParsed = double.TryParse(ResponseObjects.Longitude, out double LngParsed);
                    if (LatDoubParsed == true && LngDoubParsed == true)
                    {
                        return new LocationWithGeoCode(ResponseObjects.Status.ToString(), string.Concat(ResponseObjects.City, ", ", ResponseObjects.RegionCode, " ", ResponseObjects.CountryName), "", LngParsed, LatParsed);
                    }
                    else
                    {
                        return new LocationWithGeoCode(ResponseObjects.Status.ToString(), string.Concat(ResponseObjects.City, ", ", ResponseObjects.RegionCode, " ", ResponseObjects.CountryName), "", 0, 0);
                    }
                }
                else
                {
                    return new LocationWithGeoCode(ResponseObjects.Status.ToString(), "", "", 0, 0);
                }
            }
        }

        public static LocationWithGeoCode CallGeoCodeApi(string location, string apiKey)
        {
            string GeoCodeApiLink = "https://maps.googleapis.com/maps/api/geocode/json?";
            string LocationQuery = location.Replace(" ", "+");
            using (var webClient = new WebClient())
            {
                string apiResponse = webClient.DownloadString($"{GeoCodeApiLink}address={LocationQuery}&key={apiKey}");
                GeoCodeApiResponse.RootObject ResponseObjects = JsonConvert.DeserializeObject<GeoCodeApiResponse.RootObject>(apiResponse);
                if (!GeoCodeErr.Contains(ResponseObjects.Status))
                {
                    return new LocationWithGeoCode(ResponseObjects.Status, ResponseObjects.Results[0].FormattedAddress, ResponseObjects.Results[0].PlaceId, ResponseObjects.Results[0].Geometry.Location.Lng, ResponseObjects.Results[0].Geometry.Location.Lat);
                }
                else
                {
                    return new LocationWithGeoCode(ResponseObjects.Status, "", "", 0, 0);
                }
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
                DirectionsApiResponse.Route Route = ResponseObjects.Routes.First();
                if (!DirErr.Contains(ResponseObjects.Status))
                {
                    return new Directions(ResponseObjects.Status, Route.Legs.First().Distance.Text, Route.Legs.First().Duration.Text, Route.Summary, Directions.Decode(Route.Overview_Polyline.Points));
                }
                else
                {
                    IEnumerable<PolyLineCoordinates> emptyPolyLine = null;
                    return new Directions(ResponseObjects.Status, "", "", "", emptyPolyLine);
                }
            }
        }

        public static List<PlacesNearby> CallPlacesNearbyApi(List<PolyLineCoordinates> endLocation, string typeOfPlace, string apiKey)
        {
            string PlacesNearbyApiLink = "https://maps.googleapis.com/maps/api/place/nearbysearch/json?";
            string EndLocation = $"{endLocation.Last().Latitude}, {endLocation.Last().Longitude}";
            using (var webClient = new WebClient())
            {
                string apiResponse = webClient.DownloadString($"{PlacesNearbyApiLink}location={EndLocation}&radius=8000&type={typeOfPlace}&key={apiKey}");
                PlacesNearbyApiResponse.Root ResponseObjects = JsonConvert.DeserializeObject<PlacesNearbyApiResponse.Root>(apiResponse);
                var ResultsList = new List<PlacesNearby>();
                foreach (var r in ResponseObjects.results)
                {
                    var placesResponse = new PlacesNearby(r.name, r.place_id);
                    ResultsList.Add(placesResponse);
                }
                return ResultsList;
            }
        }

    }
}
