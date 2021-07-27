using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace RoadTripPlannerProject
{
    public class DirectionsApiResponse
    {
        public class GeocodedWaypoint
        {
            [JsonProperty(PropertyName = "geocoder_status")]
            public string Geocoder_Status { get; set; }
            [JsonProperty(PropertyName = "place_id")]
            public string Place_Id { get; set; }
        }

        public class Distance
        {
            [JsonProperty(PropertyName = "text")]
            public string Text { get; set; }
            [JsonProperty(PropertyName = "value")]
            public int Value { get; set; }
        }

        public class Duration
        {
            [JsonProperty(PropertyName = "text")]
            public string Text { get; set; }
            [JsonProperty(PropertyName = "value")]
            public int Value { get; set; }
        }

        public class StartLocation
        {
            [JsonProperty(PropertyName = "lat")]
            public double Lat { get; set; }
            [JsonProperty(PropertyName = "lng")]
            public double Lng { get; set; }
        }

        public class Polyline
        {
            [JsonProperty(PropertyName = "points")]
            public string Points { get; set; }
        }

        public class Step
        {
            [JsonProperty(PropertyName = "distance")]
            public Distance Distance { get; set; }
            [JsonProperty(PropertyName = "duration")]
            public Duration Duration { get; set; }
            [JsonProperty(PropertyName = "polyline")]
            public Polyline Polyline { get; set; }
        }

        public class Leg
        {
            [JsonProperty(PropertyName = "distance")]
            public Distance Distance { get; set; }
            [JsonProperty(PropertyName = "duration")]
            public Duration Duration { get; set; }
            [JsonProperty(PropertyName = "steps")]
            public List<Step> Steps { get; set; }
            [JsonProperty(PropertyName = "via_waypoint")]
            public List<object> Via_Waypoint { get; set; }
        }

        public class OverviewPolyline
        {
            [JsonProperty(PropertyName = "points")]
            public string Points { get; set; }
        }

        public class Route
        {
            [JsonProperty(PropertyName = "legs")]
            public List<Leg> Legs { get; set; }
            [JsonProperty(PropertyName = "overview_polyline")]
            public OverviewPolyline Overview_Polyline { get; set; }
            [JsonProperty(PropertyName = "summary")]
            public string Summary { get; set; }
            [JsonProperty(PropertyName = "warnings")]
            public List<object> Warnings { get; set; }
            [JsonProperty(PropertyName = "waypoint_order")]
            public List<object> Waypoint_Order { get; set; }
        }

        public class Root
        {
            [JsonProperty(PropertyName = "geocoded_waypoints")]
            public List<GeocodedWaypoint> Geocoded_Waypoints { get; set; }
            [JsonProperty(PropertyName = "routes")]
            public List<Route> Routes { get; set; }
            [JsonProperty(PropertyName = "status")]
            public string Status { get; set; }
        }

    }
}
