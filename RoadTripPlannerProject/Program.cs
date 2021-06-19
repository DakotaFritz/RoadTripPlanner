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
                string locationInput = Console.ReadLine();
                CurrentLocation currentLocation = new CurrentLocation(CurrentLocation.CallApi(locationInput).Location, CurrentLocation.CallApi(locationInput).Longitude, CurrentLocation.CallApi(locationInput).Latitude);
                Console.WriteLine($"Your current location is {currentLocation.Location} and the coordinates are {currentLocation.Longitude}, {currentLocation.Latitude}");
            }
            else
            {
                Console.WriteLine("That's okay! Please type \"I'm ready\" when you are ready to begin.");
            }
        }
    }
}
