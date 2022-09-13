# unity-location-example
Example project showing integration of location services in Unity. This project does the following.

SampleScene:
- Press the Update button to start the location service and get the current device latitude and longitude. This is saved internally as the current location.
- Press the Set Start button to take the current location recorded in the previous step and store it as the start location.
- Press the Distance button to calculate the distance in meters between the start location and current location.

NativePluginScene:
- In Update function the current device latitude and longitude is retrieved. This is saved internally as the current location.
- Press the Set Start button to take the current location recorded in the previous step and store it as the start location.
- Press the Distance button to calculate the distance in meters between the start location and current location.
- When user walks and distance thresholds are passed, the threshold number is shown at the bottom under STATUS.
- Press the Toggle Threshold button to toggle between 1 and 5 m theshold increments.

## Running Locally
Use the following steps to run locally:
1. Clone this repo
2. Open repo folder using Unity 2021.3.3f.1
3. Install Text Mesh Pro

## Platform Support
This repo has been tested for use on the following platforms:
- Android
- iOS

## Android Settings
To get permission to access location services on Android, the following is required:
- ACCESS_FINE_LOCATION permission

## iOS Settings
To get permission to access the location services on iOS, the following is required:
- set the location usage description in the build settings.

## Development Tools
- Created using Unity 2021.3.3f.1
- Code edited using Visual Studio Code.

## Credits
GPS in Unity code in SampleScene based on this tutorial:
https://nosuchstudio.medium.com/how-to-access-gps-location-in-unity-521f1371a7e3

GPS in Unity code in NativePluginScene based on this tutorial:
https://kulwik.medium.com/unity-native-gps-plugin-ios-and-android-11469d86190c

And uses this plugin:
https://assetstore.unity.com/packages/tools/localization/native-gps-plugin-ios-android-216027

Distance calculation based on this:
https://www.geeksforgeeks.org/program-distance-two-points-earth/



## Useful Links
Unity Location Services Overview:
https://docs.unity3d.com/ScriptReference/LocationService.html

Unity Location Services Start:
https://docs.unity3d.com/ScriptReference/LocationService.Start.html

Latitude/Longitude Distance Calculations:
https://www.meridianoutpost.com/resources/etools/calculators/calculator-latitude-longitude-distance.php?

