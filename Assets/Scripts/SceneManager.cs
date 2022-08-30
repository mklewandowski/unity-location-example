using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI DebugText;
    [SerializeField]
    TextMeshProUGUI LocationText;
    [SerializeField]
    TextMeshProUGUI StartLocationText;
    [SerializeField]
    TextMeshProUGUI DistanceText;

    float startLat = 0;
    float startLong = 0;
    float currLat = 0;
    float currLong = 0;

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        // No permission handling needed in Editor
#elif UNITY_ANDROID
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.CoarseLocation))
        {
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.CoarseLocation);
        }

        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.FineLocation))
        {
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.FineLocation);
        }
#elif UNITY_IOS

#endif
        StartCoroutine(StartLocationService());
    }

    IEnumerator StartLocationService()
    {
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Location not enabled");
            DebugText.text = DebugText.text + "Location not enabled\n";
            yield break;
        }

        // Starts the location service. We can pass in values for the accuracy and update distance.
        // public void Start(float desiredAccuracyInMeters, float updateDistanceInMeters);
        Input.location.Start(.5f, .5f);

        // Waits until the location service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            DebugText.text = DebugText.text + "Timed out\n";
            yield break;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            DebugText.text = DebugText.text + "Unable to determine device location\n";
            yield break;
        }
        else
        {
            // retrieves the device's current location
            string loc = "Location:\nlatitude: " + Input.location.lastData.latitude + "\nlongitude: " + Input.location.lastData.longitude;
            currLat = Input.location.lastData.latitude;
            currLong = Input.location.lastData.longitude;
            Debug.Log(loc);
            LocationText.text = loc;
        }

        // Stops the location service if there is no need to query location updates continuously.
        Input.location.Stop();
    }

    public void UpdateLocation()
    {
        LocationText.text = "";
        StartCoroutine(StartLocationService());
    }

    public void SetStartLocation()
    {
        startLat = currLat;
        startLong = currLong;
        // retrieves the device's current location
        string loc = "Start Location:\nlatitude: " + startLat + "\nlongitude: " + startLong;
        StartLocationText.text = loc;
    }

    public void ComputeDistance()
    {
        double dist = DistanceBetweenPointsInMeters((double)startLat, (double)startLong, (double)currLat, (double)currLong);
        DistanceText.text = "Distance: " + dist + " m";
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
        dist = dist * 60 * 1.1515;
        return dist * 1.609344 * 1000;
    }
}
