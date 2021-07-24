using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RoadTripPlannerProject
{
    class Program
    {
        static void Main(string[] args)
        {
            bool QuitProgram = false;
            while (QuitProgram == false)
            {
                string ApiKey = "";
                LocationWithGeoCode CurrentLocation = new LocationWithGeoCode("", "", "", 0,0);
                LocationWithGeoCode DestinationLocation = new LocationWithGeoCode("", "", "", 0, 0);

                Console.WriteLine("Hello, I’m here to help you plan stops on your road trip! As we begin, I’m going to ask you a few questions to help make some calculations and get you the best results.");
                Console.WriteLine("Are you ready to begin ?");
                string Ready = UserInput.ReadyToStartInput();
                if (Ready == "quit")
                {
                    break;
                }

                bool FirstApiSuccess = false;
                while (FirstApiSuccess == false)
                {
                    Console.WriteLine("Great! Please start by telling me your current location. Note: If you enter this incorrectly, you will get a chance to update your location in a moment.");
                    string CurrentLocationInput = Console.ReadLine();

                    ApiKey = UserInput.ApiKeyInput();
                    if (ApiKey == "quit")
                    {
                        break;
                    }

                    CurrentLocation = ApiCalls.CallGeoCodeApi(CurrentLocationInput, ApiKey);

                    if (!ApiCalls.GeoCodeErr.Contains(CurrentLocation.Status))
                    {
                        FirstApiSuccess = true;
                        Console.WriteLine($"Thank you for letting me know that you are in {CurrentLocation.Location}.");
                    }
                    else
                    {
                        Console.WriteLine("I'm sorry, but there was an error with your input. Please try again.");
                        continue;
                    }
                }

                bool SecondApiSuccess = false;
                while (SecondApiSuccess == false)
                {
                    Console.WriteLine("Where are you driving to?");
                    string DestinationInput = Console.ReadLine();
                    DestinationLocation = ApiCalls.CallGeoCodeApi(DestinationInput, ApiKey);
                    if (!ApiCalls.GeoCodeErr.Contains(DestinationLocation.Status))
                    {
                        SecondApiSuccess = true;
                    }
                    else
                    {
                        Console.WriteLine("I'm sorry, but there was an error with your input. Please try again.");
                        continue;
                    }
                }


                UserInput.ConfirmLocationsInput(CurrentLocation, DestinationLocation, ApiKey);

                Directions CurrentDirections = ApiCalls.CallDirectionsApi(CurrentLocation.PlaceId, DestinationLocation.PlaceId, ApiKey);
                Console.WriteLine(CurrentDirections.Distance);
                Console.WriteLine(CurrentDirections.Duration);
                Console.WriteLine(CurrentDirections.RouteSummary);

                bool DrivingTodayIsDouble = false;
                while (DrivingTodayIsDouble == false)
                {
                    Console.WriteLine("How far do you want to go today?");
                    double DrivingToday = UserInput.DrivingTodayInput();
                    List<PolyLineCoordinates> PointsLeftToday = NumericExtensions.DistanceToInput(CurrentDirections.Points, DrivingToday);
                    Console.WriteLine($"Hold on one second as I gather information about gas stations around {DrivingToday} miles from here along your route.");
                    var GasStationsNearby = ApiCalls.CallPlacesNearbyApi(PointsLeftToday, "gas_station", ApiKey);
                    foreach (var p in GasStationsNearby)
                    {
                        Console.WriteLine($"Would you like to go to {p.Name}? I have confirmed that it is currently open.");
                        string GasChoice = Console.ReadLine();
                        if (UserInput.PosInputOpt.Contains(GasChoice.ToLower()))
                        {
                            Console.WriteLine($"Please hold one second as I get directions to {p.Name}");
                            Directions DirectionToGas = ApiCalls.CallDirectionsApi(CurrentLocation.PlaceId, p.PlaceId, ApiKey);
                            Console.WriteLine($"{p.Name} is {DirectionToGas.Distance} from here. It will take you {DirectionToGas.Duration} to get there.");
                            break;
                        }
                        else if (GasChoice.ToLower() == "quit")
                        {
                            QuitProgram = true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
        }
    }
}
