# C# Roadtrip Planner

C# Roadtrip Planner is my project application for my C# course with Code Louisvile in the Summer 2021 semester.

The inspiration for this project came from my passion for travel. My wife and I have been planning a roadtrip through much of Alaska and I thought it would be nice to create an app that could assist with finding places along our routes, as there will be some areas that have no gas or villages for multiple hours at a time.

In it's current form, this app is able to get the approximate location of the user through their current IP address or through user input, depending on the preference of the user. The user is then asked a few questions to determine the best route for the trip and helps find a gas station along the way.

## Instructions:
* **When the app is first loaded, the user will be asked if they are ready to begin. What happens next depends on user input. If the answer is in the list of expected positive responses (Yes, Yep, Yeah, etc.), then the app will proceed to the next step. If the answer is "quit", then the app will stop running. If the answer is neither positive or "quit", then the user will receive the same prompt to get started.**

* **Once the user gives an expected positive response, they will be prompted to give their Google API key. Google API keys only contain alphanumerics and dashes, so if the submission contains anything other than those characters, the user will be prompted to submit the API key again.**

* **Once the API key has been submitted, the user will be presented with their location based on their IP address. Due to the API that the IP address comes from being imprecise, the location may be off by a few miles. The user will then be prompted to confirm if their IP-based location is correct or not. If the response is an expected positive response, then the app will continue using the IP-based location as the current location of the user. If the response is a negative response, the user will be prompted to type their current location instead of the IP-based location. If the response is not in either the positive or negative lists, then they will be prompted to try again.**

* **Once the IP-based location has been confirmed or a new location has been entered, the user will then be prompted to say where they are planning to drive to.

* **At this point, the user will be prompted to confirm their origin and destination locations. If the answer is negative identifying that one or both locations is incorrect, the user will be prompted to identify if it was the origin or destination (or both) that is incorrect. They will then be able to update the location for the one(s) that they identify and reconfirm locations after.**

* **Once both locations have been confirmed, the app will get directions from the origin to the destination. If there is an error (like getting driving directions from Louisville to London or Minneapolis to Moscow), then the user will be prompted to reconfirm the locations and give two locations that are possible to drive between.**

* **After the directions have been displayed, the user will be prompted about how far they want to drive today. The expected input is a number of miles (just the number). If anything other than a number is submitted, the user will be prompted to re-submit their input. This number will be used to determine how far along the route the stopping place will be. It will then look for gas stations within a 5 miles radius of that point and read them back to the user.**

* **The app will then read off gas stations within the radius of the point x miles along the route. For each one, the user will be asked if they want to go to that one. If the answer is positive, the app will then get directions from the origin to that gas station. If the answer is negative, then it moves to the next gas station. If none of the gas stations are selected, then the app loops back through the list until the user chooses one.**

* **The app will then create a directions.txt file with directions information in it.**

## Features:

* **

* **

* **

* **

* **

* **