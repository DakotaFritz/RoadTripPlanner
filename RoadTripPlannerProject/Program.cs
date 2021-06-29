using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RoadTripPlannerProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, I’m here to help you plan stops on your road trip! As we begin, I’m going to ask you a few questions to help make some calculations and get you the best results.");
            Console.WriteLine("Are you ready to begin ?");
            string ready = Console.ReadLine();

            if (ready.ToLower() == "yes" || ready.ToLower() == "yep" || ready.ToLower() == "yeah")
            {
                Console.WriteLine("Great! Please start by telling me your current location.");
                string CurrentLocationInput = Console.ReadLine();
                Console.WriteLine("Sorry to bug you, but can you give me your API Key?");
                string ApiKey = Console.ReadLine();
                var currentLocationApiCall = LocationWithGeoCode.CallGeoCodeApi(CurrentLocationInput, ApiKey);
                LocationWithGeoCode CurrentLocation = new LocationWithGeoCode(currentLocationApiCall.Location, currentLocationApiCall.PlaceId, currentLocationApiCall.Longitude, currentLocationApiCall.Latitude);
                Console.WriteLine($"Thank you for letting me know that you are in {CurrentLocation.Location}. Where are you driving to?");
                string DestinationInput = Console.ReadLine();
                LocationWithGeoCode DestinationLocation = new LocationWithGeoCode(LocationWithGeoCode.CallGeoCodeApi(DestinationInput, ApiKey).Location, LocationWithGeoCode.CallGeoCodeApi(DestinationInput, ApiKey).PlaceId, LocationWithGeoCode.CallGeoCodeApi(DestinationInput, ApiKey).Longitude, LocationWithGeoCode.CallGeoCodeApi(DestinationInput, ApiKey).Latitude);
                Console.WriteLine($"Thank you! Just to confirm, you are currently in {CurrentLocation.Location} and driving to {DestinationLocation.Location}, right? If this is incorrect, please type \"No\"");
                string confirmLocations = Console.ReadLine();
                if (confirmLocations.ToLower() != "no")
                {
                    Console.WriteLine("Thank you for confirming! Please give me just a second as I calculate your route.");
                    LocationWithGeoCode.CallDirectionsApi(CurrentLocation.PlaceId, DestinationLocation.PlaceId, ApiKey);
                }
            }
            else
            {
                Console.WriteLine("That's okay! Please type \"I'm ready\" when you are ready to begin.");
            }
        }
    }
}
