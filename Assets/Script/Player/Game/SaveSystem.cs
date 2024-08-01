using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public const string DataKey = "GameData";

    public static void Save(GameData data)
    {
        string jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(DataKey, jsonData);
        PlayerPrefs.Save();
    }

    public static GameData Load()
    {
        if (PlayerPrefs.HasKey(DataKey))
        {
            string jsonData = PlayerPrefs.GetString(DataKey);
            return JsonUtility.FromJson<GameData>(jsonData);
        }
        else
        {
            GameData emptyData = new GameData();
            Save(emptyData);
            return emptyData;
        }
    }
}
//public static class SaveSystem
//{

//    public static void Save(GameData data)
//    {
//        string path = Application.persistentDataPath + "/data.qnd";
//        BinaryFormatter formatter = new BinaryFormatter();
//        FileStream fs = new FileStream(GetPath(), FileMode.Create);
//        formatter.Serialize(fs, data);
//        fs.Close();
//    }
//    public static GameData Load()
//    {
//        if (!File.Exists(GetPath()))
//        {
//            GameData emptyData = new GameData();
//            Save(emptyData);
//            return emptyData;
//        }

//        BinaryFormatter formatter = new BinaryFormatter();
//        FileStream fs = new FileStream(GetPath(), FileMode.Open);
//        GameData data = formatter.Deserialize(fs) as GameData;
//        fs.Close();
//        return data;
//    }
//    private static string GetPath()
//    {
//        return Application.persistentDataPath + "/data.qnd";
//    }
//}