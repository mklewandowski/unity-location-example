# unity-location-example
Example project showing integration of location services in Unity. This project does the following:
- press the Update button to start the location service and get the current device latitude and longitude. This is saved internally as the current location.
- press the Set Start button to take the current location recorded in the previous step and store it as the start location.
- press the Distance button to calculate the distance in meters between the start location and current location.

## Running Locally
Use the following steps to run locally:
1. Clone this repo
2. Open repo folder using Unity 2021.3.3f.1
3. Install Text Mesh Pro

## Platform Support
This repo has been tested for use on the following platforms:
- Android

## Android Settings
To get permission to access location services on Android, the following is required:
- ACCESS_FINE_LOCATION permission
- ACCESS_COURSE_LOCATION permission (might not be needed for this example, included anyway)

## iOS Settings
To get permission to access the location services on iOS, the following is required:
- WTD

## Development Tools
- Created using Unity 2021.3.3f.1
- Code edited using Visual Studio Code.

## Credits
GPS in Unity code based on this tutorial:
https://nosuchstudio.medium.com/how-to-access-gps-location-in-unity-521f1371a7e3

## Useful Links
Unity Location Services Overview:
https://docs.unity3d.com/ScriptReference/LocationService.html

Unity Location Services Start:
https://docs.unity3d.com/ScriptReference/LocationService.Start.html

Latitude/Longitude Distance Calculations:
https://www.meridianoutpost.com/resources/etools/calculators/calculator-latitude-longitude-distance.php?

