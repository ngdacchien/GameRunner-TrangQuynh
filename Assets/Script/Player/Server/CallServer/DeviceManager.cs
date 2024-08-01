using UnityEngine;
using System;
using System.Runtime.InteropServices;
using UnityEngine.Networking;
using System.Collections;

public class DeviceManager : MonoBehaviour
{
    private GameData gameData;
    private Server server;
    string device_id;

    private void Start()
    {
        gameData = SaveSystem.Load();
        device_id = SystemInfo.deviceUniqueIdentifier;
        gameData.device_id = device_id;
        SaveSystem.Save(gameData);
        server = FindObjectOfType<Server>();
        StartCoroutine(server.LogUser(device_id));
        SaveSystem.Save(gameData);
    }
}
