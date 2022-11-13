using System.Collections.Generic;
using UnityEngine;

public static class WaitForCache
{
    static Dictionary<float, WaitForSeconds> waitForSecondsDict = new Dictionary<float, WaitForSeconds>();
    public static WaitForSeconds Seconds(float seconds)
    {
        if (waitForSecondsDict.ContainsKey(seconds)) return waitForSecondsDict[seconds];
        var waitForSeconds = new WaitForSeconds(seconds);
        waitForSecondsDict[seconds] = waitForSeconds;
        return waitForSeconds;
    }

    public static void Log()
    {
        foreach (var a in waitForSecondsDict)
        {
            Debug.Log($"{a.Key} {a.Value}");
        }
    }
}
