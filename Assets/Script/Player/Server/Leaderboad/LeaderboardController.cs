using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;

public class LeaderboardController : MonoBehaviour
{
    string url = "post_leaderboard.php";
    private int ranks_to_get = 100; // Số lượng hạng cần lấy
    public Transform leaderboardParent; // Cha của các mục bảng xếp hạng trong Hierarchy
    public GameObject leaderboardEntryPrefab; // Prefab cho mục bảng xếp hạng
    public Color firstPlaceColor = Color.red; // Màu cho người chơi xếp hạng 1
    public Color otherPlaceColor = Color.white; // Màu cho những người chơi khác

    IEnumerator GetPlayerInfo()
    {
        WWWForm form = new WWWForm();
        form.AddField("ranks_to_get", ranks_to_get);

        using (UnityWebRequest www = UnityWebRequest.Post(Server.baseUrl + url, form))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                string jsonResponse = www.downloadHandler.text;
                PlayerInfoList playerInfoList = JsonUtility.FromJson<PlayerInfoList>(jsonResponse);
                if (!string.IsNullOrEmpty(playerInfoList.error))
                {
                    Debug.LogError(playerInfoList.error);
                }
                else
                {
                    foreach (Transform child in leaderboardParent)
                    {
                        Destroy(child.gameObject);
                    }

                    for (int i = 0; i < playerInfoList.players.Count; i++)
                    {
                        PlayerInfo playerInfo = playerInfoList.players[i];
                        GameObject entry = Instantiate(leaderboardEntryPrefab, leaderboardParent);
                        Text[] texts = entry.GetComponentsInChildren<Text>();

                        texts[0].text = (i + 1).ToString()+ ". " + playerInfo.display_name;
                        texts[1].text = playerInfo.score_number.ToString();

                        // Đổi màu chữ dựa trên hạng của người chơi
                        Color textColor = (i == 0) ? firstPlaceColor : otherPlaceColor;
                        texts[0].color = textColor;
                        texts[1].color = textColor;
                    }
                }
            }
        }
    }

    void Start()
    {
        StartCoroutine(GetPlayerInfo());
    }

    [System.Serializable]
    public class PlayerInfo
    {
        public string display_name;
        public int score_number;
    }

    [System.Serializable]
    public class PlayerInfoList
    {
        public List<PlayerInfo> players;
        public string error;
    }
}
