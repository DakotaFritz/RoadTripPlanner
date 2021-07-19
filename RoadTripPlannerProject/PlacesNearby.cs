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

        public PlacesNearby(string name, string placeId)
        {
            Name = name;
            PlaceId = placeId;
        }
    }
}
