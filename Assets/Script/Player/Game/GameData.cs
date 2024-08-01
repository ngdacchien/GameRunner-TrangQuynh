using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class GameData
{
    public string device_id;
    public string display_name;
    public string create_time;

    public int CoinsEx;
    public int CoinsNew;
    public int totalCoins;
    public int totalCoinsPremium;
    public int highestScore;

    public bool[] leverUnlocked;
    public bool[] charUnlocked;

    public int language;
    public int qualitySettings;

    public GameData()
    {
        display_name = "";
        device_id = "";
        create_time = "";

        CoinsEx = 0;
        CoinsNew = 0;
        totalCoins = 0;
        totalCoinsPremium = 0;
        highestScore = 0;

        leverUnlocked = new bool[5];
        leverUnlocked[0] = true;
        charUnlocked = new bool[5];
        charUnlocked[1] = true;
        language = 0;
        qualitySettings = 0;


    }

}
