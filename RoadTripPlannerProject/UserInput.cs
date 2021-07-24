using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RoadTripPlannerProject
{
    public class UserInput
    {

        private static string[] PosInputOptArr = { "yes", "yep", "yea", "yup", "hell yeah", "heck yeah", "y", "yeah" };
        public static List<string> PosInputOpt = new List<string>(PosInputOptArr);
        private static string[] NegInputOptArr = { "nah", "no", "nope" };
        public static List<string> NegInputOpt = new List<string>(NegInputOptArr);

        public static string ReadyToStart()
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

        public UserInput()
        {
        }
    }
}
