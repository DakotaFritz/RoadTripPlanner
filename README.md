# C# Roadtrip Planner

C# Roadtrip Planner is my project application for my C# course with Code Louisvile in the Summer 2021 semester.

The inspiration for this project came from my passion for travel. My wife and I have been planning a roadtrip through much of Alaska and I thought it would be nice to create an app that could assist with finding places along our routes, as there will be some areas that have no gas or villages for multiple hours at a time.

In it's current form, this app is able to get the approximate location of the user through their current IP address or through user input, depending on the preference of the user. The user is then asked a few questions to determine the best route for the trip and helps find a gas station along the way.

## Instructions:
* **User must say if they are ready to start.**

    * When the app is first loaded, the user will be asked if they are ready to begin. What happens next depends on user input. If the answer is in the list of expected positive responses (Yes, Yep, Yeah, etc.), then the app will proceed to the next step. If the answer is "quit", then the app will stop running. If the answer is neither positive or "quit", then the user will receive the same prompt to get started.

* **User must enter the API key.**

    * Once the user gives an expected positive response, they will be prompted to give their Google API key. Google API keys only contain alphanumerics and dashes, so if the submission contains anything other than those characters, the user will be prompted to submit the API key again.

* **User must confirm that the IP-based location is correct or say that they want to give another location and enter it when prompted.**

    * Once the API key has been submitted, the user will be presented with their location based on their IP address. Due to the API that the IP address comes from being imprecise, the location may be off by a few miles. The user will then be prompted to confirm if their IP-based location is correct or not. If the response is an expected positive response, then the app will continue using the IP-based location as the current location of the user. If the response is a negative response, the user will be prompted to type their current location instead of the IP-based location. If the response is not in either the positive or negative lists, then they will be prompted to try again.

* **User must enter where they are driving to.**

    * Once the IP-based location has been confirmed or a new location has been entered, the user will then be prompted to say where they are planning to drive to.

* **User must confirm or change locations.**

    * At this point, the user will be prompted to confirm their origin and destination locations. If the answer is negative identifying that one or both locations is incorrect, the user will be prompted to identify if it was the origin or destination (or both) that is incorrect. They will then be able to update the location for the one(s) that they identify and reconfirm locations after.

* **User will be presented with directions as long as there are no errors. If there are errors, user will be prompted to re-confirm locations (likely the problem was that they are not driveable locations)**

    * Once both locations have been confirmed, the app will get directions from the origin to the destination. If there is an error (like getting driving directions from Louisville to London or Minneapolis to Moscow), then the user will be prompted to reconfirm the locations and give two locations that are possible to drive between.

* **User will enter a number that represents the number of miles they still want to drive today. If this number is greater than the number from the directions, they'll be prompted to enter a new number.**

    * After the directions have been displayed, the user will be prompted about how far they want to drive today. The expected input is a number of miles (just the number). If anything other than a number is submitted, the user will be prompted to re-submit their input. This number will be used to determine how far along the route the stopping place will be. It will then look for gas stations within a 5 miles radius of that point and read them back to the user.

* **User will be presented with gas stations within a 5 mile radius of their end point for today based on the number of miles they entered. The gas stations will be options that come one by one. A gas station must be selected.**

    * The app will then read off gas stations within the radius of the point x miles along the route. For each one, the user will be asked if they want to go to that one. If the answer is positive, the app will then get directions from the origin to that gas station. If the answer is negative, then it moves to the next gas station. If none of the gas stations are selected, then the app loops back through the list until the user chooses one.**

* **Directions from their current location to the gas station they selected will be retrieved and the app will then create a directions.txt file with the directions information in it.**

## Features:

* **Implement a “master loop” console application where the user can repeatedly enter commands/perform actions, including choosing to exit the program**

    * While loops are used in numerous places, especially to validate user input.

* **Create a dictionary or list, populate it with several values, retrieve at least one value, and use it in your program**

    * Lists are used in multiple classes.

* **Read data from an external file, such as text, JSON, CSV, etc and use that data in your application**

    * Each API response for this project is in JSON format.

* **Implement a regular expression (regex) to ensure a field either a phone number or an email address is always stored and displayed in the same format**

    * RegEx is used to validate the API key that is entered by the user in `ApiKeyInput()` on the `UserInput` class.

* **Connect to an external/3rd party API and read data into your app**

    * This app connects to 4 different APIs.