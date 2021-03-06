using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RoadTripPlannerProject
{
    public class UserInput
    {
        /// <summary>
        /// Lists of positive and negative inputs created to allow for greater flexibility in accepable inputs
        /// </summary>
        private static string[] PosInputOptArr = { "yes", "yep", "yea", "yup", "hell yeah", "heck yeah", "y", "yeah" };
        public static List<string> PosInputOpt = new List<string>(PosInputOptArr);
        private static string[] NegInputOptArr = { "nah", "no", "nope" };
        public static List<string> NegInputOpt = new List<string>(NegInputOptArr);

        /// <summary>
        /// Reads input and evaluates to see if it is either a positive response or "quit". If it meets either condition, the string is returned. If it matches neither, it will continue to loop until either condition is met.
        /// </summary>
        /// <returns>String matching a positive response or the word "quit"</returns>
        public static string ReadyToStartInput()
        {
            string Ready = "";
            while (Ready.ToLower() != "quit" || PosInputOpt.Contains(Ready.ToLower()))
            {
                Ready = Console.ReadLine();
                if (Ready.ToLower() == "quit" || PosInputOpt.Contains(Ready.ToLower()))
                {
                    return Ready;
                }
                else
                {
                    Console.WriteLine("I'm sorry, but I'm not sure what you were intending to say. Please say \"Quit\" if you want to quit or give an affirmative answer.");
                    continue;
                }
            }
            return Ready.ToLower();
        }

        /// <summary>
        /// Reads input and uses RegEx to make sure that ApiKey entered matches Google's ApiKey norms (alphanumeric and dashes). If input does not match criteria, it will continue to loop until the input matches the RegEx test.
        /// </summary>
        /// <returns>String that matches Google's ApiKey norms</returns>
        public static string ApiKeyInput()
        {
            string ApiKey = "";
            string RgKey = "[^-a-zA-Z\\d]";
            while (ApiKey.ToLower() != "quit" || !Regex.Match(ApiKey, RgKey).Success)
            {
                Console.WriteLine("Sorry to bug you, but can you give me your API Key?");
                ApiKey = Console.ReadLine();
                RgKey = "[^-a-zA-Z\\d]";
                if (!Regex.Match(ApiKey, RgKey).Success)
                {
                    return ApiKey;
                }
                else if (ApiKey.ToLower() == "quit")
                {
                    return ApiKey;
                }
                else
                {
                    Console.WriteLine("The API Key that you entered is incorrect. Google's API keys only contain alphanumeric values and dashes. Please try typing the API key again. For more information, read here: https://cloud.google.com/docs/authentication/api-keys");
                    continue;
                }
            }
            return ApiKey;
        }

        /// <summary>
        /// Confirms that the locations that have already been input are correct before calling the Directions API. If either location is incorrect, the user can update them.
        /// </summary>
        /// <param name="currLoc">The origin location object</param>
        /// <param name="destLoc">The destination location object</param>
        /// <param name="apiKey">The API Key inherited from previous input</param>
        /// <returns>Both the origin and destination location, whether they have been updated or not</returns>
        public static (LocationWithGeoCode, LocationWithGeoCode) ConfirmLocationsInput(LocationWithGeoCode currLoc, LocationWithGeoCode destLoc, string apiKey)
        {
            string confirmLocations = "";
            bool Confirmed = false;
            while (Confirmed == false)
            {
                Confirmed = false;
                Console.WriteLine($"Thank you! Just to confirm, you are currently in {currLoc.Location} and driving to {destLoc.Location}, right? If this is incorrect, please type \"No\"");
                confirmLocations = Console.ReadLine();
                if (PosInputOpt.Contains(confirmLocations.ToLower()))
                {
                    Confirmed = true;
                    Console.WriteLine("Thank you for confirming! Please give me just a second as I calculate your route.");
                    return (currLoc, destLoc);
                }
                else if (NegInputOpt.Contains(confirmLocations.ToLower()))
                {
                    string[] OriginArr = { "origin", "first", "1st", "1", "start" };
                    List<string> Origin = new List<string>(OriginArr);
                    string[] DestArr = { "destination", "second", "2nd", "2", "end" };
                    List<string> Destination = new List<string>(DestArr);

                    Console.WriteLine("It looks like you found an error in one of the locations that I just mentioned. Please indicate which location is incorrect, so that we can go ahead and get it fixed.");
                    Console.WriteLine("If it is the first location (Origin), please indicate that by stating that it was the first or the origin. If it is the second location (Destination), please indicate that by stating that it was the second or the destination.");
                    string IncorrectLocation = Console.ReadLine();

                    if (Origin.Contains(IncorrectLocation.ToLower()))
                    {
                        Console.WriteLine("Thank you for letting me know that the origin for your trip is incorrect. Please go ahead and enter the correct location below.");
                        string CorrectedOrigin = Console.ReadLine();
                        // Makes new API call to override the current origin location
                        currLoc = ApiCalls.CallGeoCodeApi(CorrectedOrigin, apiKey);
                        continue;
                    }
                    else if (Destination.Contains(IncorrectLocation.ToLower()))
                    {
                        Console.WriteLine("Thank you for letting me know that the destination for your trip is incorrect. Please go ahead and enter the correct location below.");
                        string CorrectedDest = Console.ReadLine();
                        // Makes new API call to override the current destination location
                        destLoc = ApiCalls.CallGeoCodeApi(CorrectedDest, apiKey);
                        continue;
                    }
                    else if (IncorrectLocation.ToLower() == "quit")
                    {
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("I wasn't able to determine which location was incorrect from your input. You are about to be re-directed to where you can confirm locations or change them again.");
                        continue;
                    }
                }
                else if (confirmLocations.ToLower() == "quit")
                {
                    Confirmed = true;
                    return (currLoc, destLoc);
                }
                else
                {
                    Console.WriteLine("I'm sorry, but I couldn't determine what you meant. Please give me a more direct positive or negative response.");
                    continue;
                }
            }
            return (currLoc, destLoc);
        }

        public static double DrivingTodayInput(IEnumerable<PolyLineCoordinates> points, double distance)
        {
            double DrivingStillTodayDouble = 0;
            bool LessThanDistance = false;
            while (LessThanDistance == false)
            {
                bool isDouble = false;
                while (isDouble == false)
                {
                    string drivingStillToday = Console.ReadLine();
                    isDouble = double.TryParse(drivingStillToday, out DrivingStillTodayDouble);
                    if (isDouble == false)
                    {
                        Console.WriteLine("Sorry, please enter only numbers. For example, if you mean 50 miles, please enter \"50\".");
                        continue;
                    }
                }
                if (DrivingStillTodayDouble == 0)
                {
                    Console.WriteLine("You entered 0, but that is unfortunately an invalid answer. Please enter any number greater than 0.");
                }
                else if (DrivingStillTodayDouble > distance)
                {
                    Console.WriteLine($"The number that you entered is larger than the distance for the route that you are driving. Please enter a number less than {Math.Round(distance, 1)}");
                    continue;
                }
                else if (NumericExtensions.HaversineDistance(points.ToList().ElementAt(0), points.ToList().ElementAt(1), NumericExtensions.DistanceUnit.Miles) > DrivingStillTodayDouble)
                {
                    Console.WriteLine("The distance that you selected is too small. Please enter another distance.");
                }
                else
                {
                    LessThanDistance = true;
                }
            }
            return DrivingStillTodayDouble;
        }

    }
}
