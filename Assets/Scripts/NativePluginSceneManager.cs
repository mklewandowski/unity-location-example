using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
#if PLATFORM_ANDROID
using UnityEngine.Android;
#endif
using TMPro;

public class NativePluginSceneManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI LocationText;
    [SerializeField]
    TextMeshProUGUI StartLocationText;
    [SerializeField]
    TextMeshProUGUI DistanceText;
    [SerializeField]
    TextMeshProUGUI StatusText;

    GameObject dialog = null;
    bool locationIsReady = false;
    bool locationGrantedAndroid = false;

    double startLat = 0;
    double startLong = 0;
    double currLat = 0;
    double currLong = 0;

    double[] thresholds5m = {5, 10, 15, 20, 25, 30, 35, 40, 45, 50};
    double[] thresholds2m = {2, 4, 6, 8, 10, 12, 14, 16, 18, 20};
    int currentThreshold = 0;
    bool startThresholds = false;
    bool use5mThreshold = true;

    // Start is called before the first frame update
    void Start()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            dialog = new GameObject();
        }
        else
        {
            locationGrantedAndroid = true;
            locationIsReady = NativeGPSPlugin.StartLocation();
        }

#elif PLATFORM_IOS
        locationIsReady = NativeGPSPlugin.StartLocation();
#endif
    }

    private void Update()
    {
        if (locationIsReady)
        {
            // retrieves the device's current location
            double lat = NativeGPSPlugin.GetLatitude();
            double lon = NativeGPSPlugin.GetLongitude();
            string loc = "CURRENT LOCATION\nlatitude: " + lat + "\nlongitude: " + lon;
            currLat = lat;
            currLong = lon;
            Debug.Log(loc);
            LocationText.text = loc;

            if (startThresholds && currentThreshold < thresholds5m.Length)
            {
                double dist = DistanceBetweenPointsInMeters(startLat, startLong, currLat, currLong);
                if ((use5mThreshold && dist > thresholds5m[currentThreshold]) || (!use5mThreshold && dist > thresholds2m[currentThreshold]))
                {
                    StatusText.text = StatusText.text + "Threshold " + (currentThreshold + 1) + " passed.\n";
                    currentThreshold++;
                }
            }
        }
    }

    void OnGUI ()
    {
        #if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            // The user denied permission to use the fineLocation.
            // Display a message explaining why you need it with Yes/No buttons.
            // If the user says yes then present the request again
            // Display a dialog here.
            dialog.AddComponent<PermissionsRationaleDialog>();
            return;
        }
        else if (dialog != null)
        {
            if (!locationGrantedAndroid)
            {
                locationGrantedAndroid = true;
                locationIsReady = NativeGPSPlugin.StartLocation();
            }

            Destroy(dialog);
        }
        #endif
    }

    public void SetStartLocation()
    {
        startLat = currLat;
        startLong = currLong;
        // retrieves the device's current location
        string loc = "START LOCATION\nlatitude: " + startLat + "\nlongitude: " + startLong;
        StartLocationText.text = loc;
        currentThreshold = 0;
        startThresholds = true;
        StatusText.text = "STATUS\n";
    }

    public void ComputeDistance()
    {
        double dist = DistanceBetweenPointsInMeters(startLat, startLong, currLat, currLong);

        DistanceText.text = "START LOCATION\nlatitude: " + startLat + "\nlongitude: " + startLong + "\nCURRENT LOCATION\nlatitude: " + currLat + "\nlongitude: " + currLong + "\nDISTANCE\n" + dist + " m";
    }

    public void ToggleThreshold()
    {
        use5mThreshold = !use5mThreshold;
        if (use5mThreshold)
            StatusText.text = "STATUS\nusing 5m threshold";
        else
            StatusText.text = "STATUS\nusing 2m threshold";
    }

    public static double DistanceBetweenPointsInMeters(double lat1, double lon1, double lat2, double lon2)
    {
        double rlat1 = System.Math.PI * lat1 / 180;
        double rlat2 = System.Math.PI * lat2 / 180;
        double theta = lon1 - lon2;
        double rtheta = Mathf.PI * theta / 180;
        double dist =
            System.Math.Sin(rlat1) * System.Math.Sin(rlat2) + System.Math.Cos(rlat1) *
            System.Math.Cos(rlat2) * System.Math.Cos(rtheta);
        dist = System.Math.Acos(dist);
        dist = dist * 180 / System.Math.PI;

        // 60 is the number of minutes in a degree
        // 1.1515 is the number of statute miles in a nautical mile
        // One nautical mile is the length of one minute of latitude at the equator
        // this gives us the distance in miles
        double distInMiles = dist * 60 * 1.1515;
        // 1.609344 is the number of kilometres in a mile
        // 1000 is the number of metres in a kilometre
        double distInMeters = distInMiles * 1.609344 * 1000;
        return distInMeters;
    }

    // https://www.inchcalculator.com/convert/degree-to-radian/

    // Utility function for
    // converting degrees to radians
    double toRadians(double degree)
    {
        double one_deg = (System.Math.PI) / 180;
        return (one_deg * degree);
    }

    double DistanceBetweenPointsInMeters2(double lat1, double long1, double lat2, double long2)
    {
        // Convert the latitudes
        // and longitudes
        // from degree to radians.
        lat1 = toRadians(lat1);
        long1 = toRadians(long1);
        lat2 = toRadians(lat2);
        long2 = toRadians(long2);

        // Haversine Formula
        double dlong = long2 - long1;
        double dlat = lat2 - lat1;

        double ans = System.Math.Pow(System.Math.Sin(dlat / 2), 2) +
                            System.Math.Cos(lat1) * System.Math.Cos(lat2) *
                            System.Math.Pow(System.Math.Sin(dlong / 2), 2);

        ans = 2 * System.Math.Asin(System.Math.Sqrt(ans));

        // Radius of Earth in
        // Kilometers, R = 6371
        // Use R = 3956 for miles
        double R = 6371;

        // Calculate the result
        ans = ans * R * 1000;
        print(ans);
        return ans;
    }

}
