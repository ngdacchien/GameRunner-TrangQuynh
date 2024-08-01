using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

public class Your_Top : MonoBehaviour
{
    private GameData gameData;
    // URL của script PHP trên máy chủ của bạn
    string url = "your_score.php";

    // Device ID của người chơi
    string deviceId;

    // Mảng các đối tượng Text để hiển thị thông tin người chơi
    public Text[] playerTexts;
    public Text rank;
    public Text displayname;
    public Text score;
    void Start()
    {
        gameData = SaveSystem.Load();
        deviceId = SystemInfo.deviceUniqueIdentifier;
        gameData.device_id = deviceId;
        SaveSystem.Save(gameData);
        Debug.Log(deviceId);
        StartCoroutine(GetYourInfo());
    }
    IEnumerator GetYourInfo()
    {
        // Tạo yêu cầu POST
        WWWForm form = new WWWForm();
        form.AddField("device_id", deviceId);

        // Gửi yêu cầu và đợi phản hồi từ máy chủ
        using (UnityWebRequest www = UnityWebRequest.Post(Server.baseUrl + url, form))
        {
            yield return www.SendWebRequest();

            // Kiểm tra xem có lỗi không
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                string jsonResponse = www.downloadHandler.text;

                YourInfo yourInfo = JsonUtility.FromJson<YourInfo>(jsonResponse);
                if (yourInfo != null)
                {
                    // Assign values to Text objects
                    rank.text = yourInfo.rank.ToString();
                    displayname.text = string.IsNullOrEmpty(yourInfo.display_name) ? "N/A" : yourInfo.display_name;
                    score.text = yourInfo.score_number.ToString();
                }
                else
                {
                    // Handle the case when yourInfo is null (e.g., display a default message)
                    Debug.LogError("Failed to retrieve your information.");
                }
                rank.text = yourInfo.rank.ToString();
                displayname.text = yourInfo.display_name;
                score.text = yourInfo.score_number.ToString();
            }
        }
    }
}
// Lớp đại diện cho thông tin của một người chơi
[System.Serializable]
public class YourInfo
{
    public int rank;
    public string display_name;
    public int score_number;
    
}