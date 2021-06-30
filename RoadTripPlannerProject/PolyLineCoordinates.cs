using System;
namespace RoadTripPlannerProject
{
    public class PolyLineCoordinates
    {
        public double Longitude {get; set;}
        public double Latitude { get; set; }

        public PolyLineCoordinates(double lng, double lat)
        {
            Longitude = lng;
            Latitude = lat;
        }
    }
}
