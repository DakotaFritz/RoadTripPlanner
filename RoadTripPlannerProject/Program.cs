using System;
using System.Collections.Generic;
using System.Linq;

namespace RoadTripPlannerProject
{
    class Program
    {
        static void Main(string[] args)
        {
            bool QuitProgram = false;
            while (QuitProgram == false)
            {
                ApiCalls.TextSentimentApi("hell yeah");

                // API Key, and API Response variables are stored in global scope so it will be accesible to multiple methods
                string ApiKey = "";
                LocationWithGeoCode CurrentLocation = new LocationWithGeoCode("", "", "", 0, 0);
                LocationWithGeoCode DestinationLocation = new LocationWithGeoCode("", "", "", 0, 0);
                IEnumerable<PolyLineCoordinates> emptyPolyLine = null;
                Directions CurrentDirections = new Directions("", "", "", "", emptyPolyLine);
                List<PlacesNearby> GasStationsNearby = new List<PlacesNearby>();


                Console.WriteLine("Hello, I’m here to help you plan stops on your road trip! As we begin, I’m going to ask you a few questions to help make some calculations and get you the best results.");
                Console.WriteLine("Are you ready to begin ?");
                string Ready = UserInput.ReadyToStartInput();
                if (Ready == "quit")
                {
                    break;
                }

                ApiKey = UserInput.ApiKeyInput();
                if (ApiKey == "quit")
                {
                    break;
                }

                CurrentLocation = ApiCalls.CallIpGeoCodeApi();
                bool IpLocationConfirmed = false;
                while (IpLocationConfirmed == false)
                {
                    Console.WriteLine($"I was able to locate you (loosely) from your IP address. Are you starting from {CurrentLocation.Location}? If yes, please say \"Yes\". If not, please give say \"No\" and you'll have the opportunity to change your current location.");
                    string IpLocationCorrect = Console.ReadLine();
                    if (UserInput.NegInputOpt.Contains(IpLocationCorrect.ToLower()))
                    {
                        Console.WriteLine("Sorry to hear that the location I got from your IP was not accurate. Please tell me your current location.");
                        string CurrentLocationInput = Console.ReadLine();
                        CurrentLocation = ApiCalls.CallGeoCodeApi(CurrentLocationInput, ApiKey);
                        IpLocationConfirmed = true;
                    }
                    else if (UserInput.PosInputOpt.Contains(IpLocationCorrect.ToLower()))
                    {
                        IpLocationConfirmed = true;
                    }
                    else
                    {
                        Console.WriteLine("I'm sorry, but there was an error with your input. Please try again.");
                    }
                }

                bool FirstApiSuccess = false;
                while (FirstApiSuccess == false)
                {
                    
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

                bool GasStationsInArea = false;
                while (GasStationsInArea == false)
                {
                    Console.WriteLine("How far do you want to go today?");
                    double DrivingToday = UserInput.DrivingTodayInput(CurrentDirections.Points, CurrentDirections.DistanceDouble);
                    List<PolyLineCoordinates> PointsLeftToday = NumericExtensions.DistanceToInput(CurrentDirections.Points, DrivingToday);
                    Console.WriteLine($"Hold on one second as I gather information about gas stations around {DrivingToday} miles from here along your route.");
                    GasStationsNearby = ApiCalls.CallPlacesNearbyApi(PointsLeftToday, "gas_station", ApiKey);

                    if (GasStationsNearby.Count != 0)
                    {
                        GasStationsInArea = true;
                    }
                    else
                    {
                        Console.WriteLine("Well, that's embarrassing. It looks like there are no options within a 5 mile radius of that location. Please try again.");
                    }
                }

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
                            FileWriter.WriteToFile(CurrentLocation.Location, g.Name, DirectionToGas.Distance, DirectionToGas.Duration, DirectionToGas.RouteSummary);
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
