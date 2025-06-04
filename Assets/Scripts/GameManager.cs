using Logitech;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LogitechGSDK.LogiPlayDirtRoadEffect(0, 50);
    }
    void OnApplicationQuit()
    {
        LogitechGSDK.LogiStopDirtRoadEffect(0);
    }

}
