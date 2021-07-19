using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RoadTripPlannerProject
{
    class Program
    {
        public static object PolyLineCoordinates { get; private set; }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello, I’m here to help you plan stops on your road trip! As we begin, I’m going to ask you a few questions to help make some calculations and get you the best results.");
            Console.WriteLine("Are you ready to begin ?");
            string ready = Console.ReadLine();
            string[] PosInputOptArr = { "yes", "yep", "yea", "yup", "hell yeah", "heck yeah", "y", "yeah" };
            List<string> PosInputOpt = new List<string>(PosInputOptArr);
            string[] NegInputOptArr = { "nah", "no", "nope" };
            List<string> NegInputOpt = new List<string>(NegInputOptArr);

            if (PosInputOpt.Contains(ready.ToLower()))
            {
                Console.WriteLine("Great! Please start by telling me your current location.");
                string CurrentLocationInput = Console.ReadLine();
                Console.WriteLine("Sorry to bug you, but can you give me your API Key?");
                string ApiKey = Console.ReadLine();
                LocationWithGeoCode CurrentLocation = ApiCalls.CallGeoCodeApi(CurrentLocationInput, ApiKey);
                Console.WriteLine($"Thank you for letting me know that you are in {CurrentLocation.Location}. Where are you driving to?");
                string DestinationInput = Console.ReadLine();
                LocationWithGeoCode DestinationLocation = ApiCalls.CallGeoCodeApi(DestinationInput, ApiKey);
                Console.WriteLine($"Thank you! Just to confirm, you are currently in {CurrentLocation.Location} and driving to {DestinationLocation.Location}, right? If this is incorrect, please type \"No\"");
                string confirmLocations = Console.ReadLine();
                if (confirmLocations.ToLower() != "no")
                {
                    Console.WriteLine("Thank you for confirming! Please give me just a second as I calculate your route.");
                    Directions CurrentDirections = ApiCalls.CallDirectionsApi(CurrentLocation.PlaceId, DestinationLocation.PlaceId, ApiKey);
                    Console.WriteLine(CurrentDirections.Distance);
                    Console.WriteLine(CurrentDirections.Duration);
                    Console.WriteLine(CurrentDirections.RouteSummary);
                    Console.WriteLine("How far do you want to go today?");
                    string drivingStillToday = Console.ReadLine();
                    double DrivingStillTodayDouble = Double.Parse(drivingStillToday);
                    List<PolyLineCoordinates> PointsLeftToday = NumericExtensions.DistanceToInput(CurrentDirections.Points, DrivingStillTodayDouble);
                    Console.WriteLine($"Hold on one second as I gather information about gas stations around {drivingStillToday} miles from here along your route.");
                    var GasStationsNearby = ApiCalls.PlacesNearbyApi(PointsLeftToday, "gas_station", ApiKey);
                    foreach (var p in GasStationsNearby)
                    {
                        Console.WriteLine($"Would you like to go to {p.Name}? I have confirmed that it is currently open.");
                        string GasChoice = Console.ReadLine();
                        if (GasChoice.ToLower() == "Yes")
                        {
                            Console.WriteLine($"Please hold one second as I get directions to {p.Name}");
                            Directions DirectionToGas = ApiCalls.CallDirectionsApi(CurrentLocation.PlaceId, p.PlaceId, ApiKey);
                            Console.WriteLine($"{p.Name} is {DirectionToGas.Distance} from here. It will take you {DirectionToGas.Duration} to get there.");
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("That's okay! Please type \"I'm ready\" when you are ready to begin.");
            }
        }
    }
}
