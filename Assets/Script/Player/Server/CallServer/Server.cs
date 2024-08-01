using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Playables;
using UnityEngine.SocialPlatforms.Impl;

public class Server : MonoBehaviour
{
    private bool isApplicationQuitting = false;
    public static string baseUrl = "http://150.95.112.175/TrangQuynhRunner/";
    string device_id;
    string log_User = "log_user.php";
    string createUserUrl = "create_user.php";
    string updateUserUrl = "update_user.php";
    string updateScoreUrl = "update_score.php";
    string updateCoinUrl = "update_coin.php";
    string deleteUserUrl = "delete_user.php";
    string leaderboard = "post_leaderboard.php";
    string updateHistoryUrl = "update_status.php";
    string note;
    public string test;
    private string lastActiveTimeKey = "lastActiveTime";
    private float offlineThreshold = 120f; // 2 phút
    private float lastActiveTime;
    private GameData gameData;

    private bool isDeviceIdInitialized = false;
    private void Start()
    {
        StartCoroutine(InitializeDeviceId());
        // Load thời gian tương tác cuối cùng từ PlayerPrefs
        lastActiveTime = PlayerPrefs.GetFloat(lastActiveTimeKey, GetCurrentUnixTimestamp());
    }
    private IEnumerator InitializeDeviceId()
    {
        gameData = SaveSystem.Load();
        device_id = gameData.device_id;
        isDeviceIdInitialized = true; 
        yield return null;

    }
    private void Update()
    {
        // Chỉ thực thi các hành động nếu device_id đã được khởi tạo
        if (!isDeviceIdInitialized)
            return;

        // Cập nhật thời gian tương tác cuối cùng mỗi frame
        lastActiveTime = GetCurrentUnixTimestamp();
        PlayerPrefs.SetFloat(lastActiveTimeKey, lastActiveTime);
    }
    private void OnApplicationPause(bool pauseStatus)
    {
        // Chỉ thực thi các hành động nếu device_id đã được khởi tạo
        if (!isDeviceIdInitialized)
            return;

        if (pauseStatus)
        {
            note = "Offline";
            StartCoroutine(UpdateLoginHistory(device_id, note));
            Debug.Log("Offline");
            // Thực hiện các hành động khi người chơi tạm dừng ứng dụng
        }
        else
        {
            note = "Online";
            StartCoroutine(UpdateLoginHistory(device_id, note));
            Debug.Log("Online");
            // Thực hiện các hành động khi người chơi quay lại ứng dụng
        }
    }
    private float GetCurrentUnixTimestamp()
    {
        // Lấy thời gian hiện tại dưới dạng Unix timestamp
        return (float)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
    }
    private void OnApplicationQuit()
    {
        if (!isDeviceIdInitialized)
            return;
        note = "Offline";
        StartCoroutine(UpdateLoginHistory(device_id, note));
        Debug.Log("Quitting application");

    }
    public IEnumerator LogUser(string deviceId)
    {
        WWWForm form = new WWWForm();
        form.AddField("device_id", deviceId);
        form.AddField("display_name", "placeholder"); 

        using (UnityWebRequest www = UnityWebRequest.Post(baseUrl + log_User, form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                string jsonResponse = www.downloadHandler.text;
                test = jsonResponse;
                PlayerData playerData = JsonUtility.FromJson<PlayerData>(jsonResponse);
                gameData.display_name = playerData.display_name;
                gameData.highestScore = playerData.score_number;
                gameData.totalCoins = playerData.total_coin;
                gameData.create_time = playerData.create_at;

                SaveSystem.Save(gameData);
            }
        }
    }

    public IEnumerator CreateUser(string deviceId, string displayName)
    {
        WWWForm form = new WWWForm();
        form.AddField("device_id", deviceId);
        form.AddField("display_name", displayName);

        using (UnityWebRequest www = UnityWebRequest.Post(baseUrl + createUserUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                string jsonResponse = www.downloadHandler.text;
            }
        }
    }
    public IEnumerator UpdateUser(string deviceId, string newDisplayName)
    {
        WWWForm form = new WWWForm();
        form.AddField("device_id", deviceId);
        form.AddField("display_name", newDisplayName);
        using (UnityWebRequest www = UnityWebRequest.Post(baseUrl + updateUserUrl, form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                string jsonResponse = www.downloadHandler.text;
            }
        }
    }

    public IEnumerator UpdateScore(string deviceId, int newScore)
    {
        WWWForm form = new WWWForm();
        form.AddField("device_id", deviceId);
        form.AddField("new_score", newScore);

        using (UnityWebRequest www = UnityWebRequest.Post(baseUrl + updateScoreUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                string jsonResponse = www.downloadHandler.text;
            }
        }
    }

    public IEnumerator UpdateCoin(string deviceId, int newCoin)
    {
        WWWForm form = new WWWForm();
        form.AddField("device_id", deviceId);
        form.AddField("new_coin", newCoin);

        using (UnityWebRequest www = UnityWebRequest.Post(baseUrl + updateCoinUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                string jsonResponse = www.downloadHandler.text;
            }
        }
    }
    public IEnumerator UpdateLoginHistory(string deviceId, string note)
    {
        WWWForm form = new WWWForm();

        form.AddField("device_id", deviceId);
        form.AddField("note", note);

        using (UnityWebRequest www = UnityWebRequest.Post(baseUrl + updateHistoryUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                string jsonResponse = www.downloadHandler.text;
            }
        }
    }




    public IEnumerator DeleteUser(string deviceId)
    {
        WWWForm form = new WWWForm();
        form.AddField("device_id", deviceId);

        using (UnityWebRequest www = UnityWebRequest.Post(baseUrl + deleteUserUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                string jsonResponse = www.downloadHandler.text;
            }
        }
    }
    
    // Lớp để chứa dữ liệu người chơi
    [System.Serializable]
    public class PlayerData
    {
        public string display_name;
        public int score_number;
        public int total_coin;
        public string create_at;
        public string error;
    }
}
