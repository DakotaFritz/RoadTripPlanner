using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace RoadTripPlannerProject
{
    public class PlacesNearby
    {
        public string Name { get; set; }
        public string PlaceId { get; set; }
        public bool OpenNow { get; set; }

        public static List<PlacesNearby> PlacesNearbyApi(List<PolyLineCoordinates> endLocation, string typeOfPlace, string apiKey)
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
                    var placesResponse = new PlacesNearby(r.name, r.place_id, r.opening_hours.open_now);
                    ResultsList.Add(placesResponse);
                }
                return ResultsList;
            }
        }

        public PlacesNearby(string name, string placeId, bool openNow)
        {
            Name = name;
            PlaceId = placeId;
            OpenNow = openNow;
        }
    }
}
