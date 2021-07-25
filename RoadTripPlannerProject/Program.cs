using System;
using System.Collections.Generic;

namespace RoadTripPlannerProject
{
    class Program
    {
        static void Main(string[] args)
        {
            bool QuitProgram = false;
            while (QuitProgram == false)
            {
                // API Key, and API Response variables are stored in global scope so it will be accesible to multiple methods
                string ApiKey = "";
                LocationWithGeoCode CurrentLocation = new LocationWithGeoCode("", "", "", 0,0);
                LocationWithGeoCode DestinationLocation = new LocationWithGeoCode("", "", "", 0, 0);
                IEnumerable<PolyLineCoordinates> emptyPolyLine = null;
                Directions CurrentDirections = new Directions("", "", "", "", emptyPolyLine);
               
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

                bool DirApiSuccess = false;
                while (DirApiSuccess == false)
                {
                    (LocationWithGeoCode Current, LocationWithGeoCode Destination) ConfirmedLocation = UserInput.ConfirmLocationsInput(CurrentLocation, DestinationLocation, ApiKey);
                    if (ConfirmedLocation.Current.Location != CurrentLocation.Location)
                    {
                        CurrentLocation = ConfirmedLocation.Current;
                    }
                    else if (ConfirmedLocation.Destination.Location != DestinationLocation.Location)
                    {
                        DestinationLocation = ConfirmedLocation.Destination;
                    }

                    CurrentDirections = ApiCalls.CallDirectionsApi(CurrentLocation.PlaceId, DestinationLocation.PlaceId, ApiKey);
                    if (!ApiCalls.DirErr.Contains(CurrentDirections.Status))
                    {
                        DirApiSuccess = true;
                    }
                    else
                    {
                        Console.WriteLine("There was an error with one of your locations. Please correct your location and try again.");
                    }
                }

                Console.WriteLine(CurrentDirections.Distance);
                Console.WriteLine(CurrentDirections.Duration);
                Console.WriteLine(CurrentDirections.RouteSummary);


                Console.WriteLine("How far do you want to go today?");
                double DrivingToday = UserInput.DrivingTodayInput();
                List<PolyLineCoordinates> PointsLeftToday = NumericExtensions.DistanceToInput(CurrentDirections.Points, DrivingToday);
                Console.WriteLine($"Hold on one second as I gather information about gas stations around {DrivingToday} miles from here along your route.");
                var GasStationsNearby = ApiCalls.CallPlacesNearbyApi(PointsLeftToday, "gas_station", ApiKey);

                bool GasStationSelected = false;
                while (GasStationSelected == false)
                {
                    foreach (var g in GasStationsNearby)
                    {
                        Console.WriteLine($"Would you like to go to {g.Name}? I have confirmed that it is currently open.");
                        string GasChoice = Console.ReadLine();
                        if (UserInput.PosInputOpt.Contains(GasChoice.ToLower()))
                        {
                            GasStationSelected = true;
                            Console.WriteLine($"Please hold one second as I get directions to {g.Name}");
                            Directions DirectionToGas = ApiCalls.CallDirectionsApi(CurrentLocation.PlaceId, g.PlaceId, ApiKey);
                            Console.WriteLine($"{g.Name} is {DirectionToGas.Distance} from here. It will take you {DirectionToGas.Duration} to get there.");
                            break;
                        }
                        else if (GasChoice.ToLower() == "quit")
                        {
                            GasStationSelected = true;
                            QuitProgram = true;
                        }
                    }
                    if (GasStationSelected == false)
                    {
                        Console.WriteLine("You didn't select any of the options that we found. Please select one from the options.");
                    }
                }
                break;
            }
        }
    }
}
