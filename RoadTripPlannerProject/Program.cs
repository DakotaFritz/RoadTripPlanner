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
                Console.WriteLine("Hello, I’m here to help you plan stops on your road trip! As we begin, I’m going to ask you a few questions to help make some calculations and get you the best results.");
                Console.WriteLine("Are you ready to begin ?");
                string ready = Console.ReadLine();
                string[] PosInputOptArr = { "yes", "yep", "yea", "yup", "hell yeah", "heck yeah", "y", "yeah" };
                List<string> PosInputOpt = new List<string>(PosInputOptArr);
                string[] NegInputOptArr = { "nah", "no", "nope" };
                List<string> NegInputOpt = new List<string>(NegInputOptArr);

                if (PosInputOpt.Contains(ready.ToLower()))
                {
                    bool FirstApiSuccess = false;
                    while (FirstApiSuccess == false)
                    {
                        Console.WriteLine("Great! Please start by telling me your current location.");
                        string CurrentLocationInput = Console.ReadLine();

                        bool ValidApiKey = false;
                        while (ValidApiKey == false)
                        {
                            Console.WriteLine("Sorry to bug you, but can you give me your API Key?");
                            string ApiKey = Console.ReadLine();
                            string RgKey = "[^-a-zA-Z\\d]";
                            if (!Regex.Match(ApiKey, RgKey).Success)
                            {
                                ValidApiKey = true;
                                LocationWithGeoCode CurrentLocation = ApiCalls.CallGeoCodeApi(CurrentLocationInput, ApiKey);
                                if (!ApiCalls.GeoCodeErr.Contains(CurrentLocation.Status))
                                {
                                    FirstApiSuccess = true;
                                    Console.WriteLine($"Thank you for letting me know that you are in {CurrentLocation.Location}.");
                                    bool SecondApiSuccess = false;
                                    while (SecondApiSuccess == false)
                                    {
                                        Console.WriteLine("Where are you driving to?");
                                        string DestinationInput = Console.ReadLine();
                                        LocationWithGeoCode DestinationLocation = ApiCalls.CallGeoCodeApi(DestinationInput, ApiKey);
                                        if (!ApiCalls.GeoCodeErr.Contains(DestinationLocation.Status))
                                        {
                                            SecondApiSuccess = true;

                                            bool LocationConfirmed = false;
                                            while (LocationConfirmed == false)
                                            {
                                                Console.WriteLine($"Thank you! Just to confirm, you are currently in {CurrentLocation.Location} and driving to {DestinationLocation.Location}, right? If this is incorrect, please type \"No\"");
                                                string confirmLocations = Console.ReadLine();
                                                if (PosInputOpt.Contains(confirmLocations.ToLower()))
                                                {
                                                    LocationConfirmed = true;
                                                    Console.WriteLine("Thank you for confirming! Please give me just a second as I calculate your route.");
                                                    Directions CurrentDirections = ApiCalls.CallDirectionsApi(CurrentLocation.PlaceId, DestinationLocation.PlaceId, ApiKey);
                                                    Console.WriteLine(CurrentDirections.Distance);
                                                    Console.WriteLine(CurrentDirections.Duration);
                                                    Console.WriteLine(CurrentDirections.RouteSummary);

                                                    bool DrivingTodayIsDouble = false;
                                                    while (DrivingTodayIsDouble == false)
                                                    {
                                                        //Need to add an if here to verify that a number was input.
                                                        Console.WriteLine("How far do you want to go today?");
                                                        string drivingStillToday = Console.ReadLine();
                                                        double.TryParse(drivingStillToday, out double DrivingStillTodayDouble);
                                                        List<PolyLineCoordinates> PointsLeftToday = NumericExtensions.DistanceToInput(CurrentDirections.Points, DrivingStillTodayDouble);
                                                        Console.WriteLine($"Hold on one second as I gather information about gas stations around {DrivingStillTodayDouble} miles from here along your route.");
                                                        var GasStationsNearby = ApiCalls.CallPlacesNearbyApi(PointsLeftToday, "gas_station", ApiKey);
                                                        foreach (var p in GasStationsNearby)
                                                        {
                                                            Console.WriteLine($"Would you like to go to {p.Name}? I have confirmed that it is currently open.");
                                                            string GasChoice = Console.ReadLine();
                                                            if (PosInputOpt.Contains(GasChoice.ToLower()))
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
                                                else if (NegInputOpt.Contains(confirmLocations.ToLower()))
                                                {
                                                    Console.WriteLine("It looks like you found an error in one of the locations that I just mentioned. Please indicate which location is incorrect, so that we can go ahead and get it fixed.");
                                                    Console.WriteLine("If it is the first location (Origin), please indicate that by stating that it was the first or the origin. If it is the second location (Destination), please indicate that by stating that it was the second or the destination.");
                                                    string[] OriginArr = { "origin", "first", "1st", "1", "start" };
                                                    List<string> Origin = new List<string>(OriginArr);
                                                    string[] DestArr = { "destination", "second", "2nd", "2", "end" };
                                                    List<string> Destination = new List<string>(DestArr);
                                                    string IncorrectLocation = Console.ReadLine();
                                                    if (Origin.Contains(IncorrectLocation.ToLower()))
                                                    {
                                                        //needs to go back to origin
                                                        Console.WriteLine("Thank you for letting me know that the origin for your trip is incorrect. Please go ahead and enter the correct location below.");
                                                        string CorrectedOrigin = Console.ReadLine();
                                                        CurrentLocation = ApiCalls.CallGeoCodeApi(CorrectedOrigin, ApiKey);
                                                        continue;
                                                    }
                                                    else if (Destination.Contains(IncorrectLocation.ToLower()))
                                                    {
                                                        //needs to go back to destination
                                                        Console.WriteLine("Thank you for letting me know that the destination for your trip is incorrect. Please go ahead and enter the correct location below.");
                                                        string CorrectedDest = Console.ReadLine();
                                                        DestinationLocation = ApiCalls.CallGeoCodeApi(CorrectedDest, ApiKey);
                                                        continue;
                                                    }
                                                    else if (IncorrectLocation.ToLower() == "quit")
                                                    {
                                                        QuitProgram = true;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("I wasn't able to determine which location was incorrect from your input. You are about to be re-directed to where you can confirm locations or change them again.");
                                                        continue;
                                                    }
                                                }
                                                else if (confirmLocations.ToLower() == "quit")
                                                {
                                                    QuitProgram = true;
                                                }
                                                else
                                                {
                                                    Console.WriteLine("I'm sorry, but I couldn't determine what you meant. Please give me a more direct positive or negative response.");
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("I'm sorry, but there was an error with your input. Please try again.");
                                            continue;
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("I'm sorry, but there was an error with your input. Please try again.");
                                    continue;
                                }
                            }
                            else if (ApiKey.ToLower() == "quit")
                            {
                                QuitProgram = true;
                            }
                            else
                            {
                                Console.WriteLine("The API Key that you entered is incorrect. Google's API keys only contain alphanumeric values and dashes. Please try typing the API key again. For more information, read here: https://cloud.google.com/docs/authentication/api-keys");
                                continue;
                            }
                        }
                    }
                }
                else if (ready.ToLower() == "quit")
                {
                    QuitProgram = true;
                }
                else
                {
                    Console.WriteLine("I'm sorry, but I'm not sure what you were intending to say. Please say \"Quit\" if you want to quit or give me an affirmative answer.");
                }
            }
        }
    }
}
