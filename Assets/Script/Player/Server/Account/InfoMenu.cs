using System;
using System.Data;
using System.Net.Http;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Windows;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System.Linq;
//using UnityEditor.TerrainTools;

public class InfoMenu : MonoBehaviour
{
    private Server server;
    private AsyncOperation asyncs;
    string deviceId;
    public InputField inputDisplayName;
    public Text display_name;
    public Text coin;
    public Text score;
    public Text create_at;

    private GameData gameData;
    public bool isInfoMenu;
    public GameObject infoMenu;
    public bool isEditMenu;
    public GameObject editMenu;
    public bool isDeleteMenu;
    public GameObject deleteMenu;

    private bool isSubmittingNickname = false;

    public bool isInformation;
    public GameObject informationMenu;
    public Text informationText;
    public bool checkRename;
    public bool isExit;
    public GameObject exitgame;


    void Start()
    {
        checkRename = false;
        isInfoMenu = true;
        infoMenu.SetActive(true);
        isEditMenu = false;
        editMenu.SetActive(false);
        isDeleteMenu = false;
        deleteMenu.SetActive(false);
        isInformation = false;
        informationMenu.SetActive(false);
        isExit = false;
        exitgame.SetActive(false);
        gameData = SaveSystem.Load();
        server = FindObjectOfType<Server>();
        deviceId = gameData.device_id;
        display_name.text = gameData.display_name;
        coin.text = gameData.totalCoins.ToString();
        score.text = gameData.highestScore.ToString();
        create_at.text = gameData.create_time;
    }

    public void ExitGameClick()
    {
        SceneManager.LoadScene("MainScene");
        //Application.Quit();
    }
    public void OpenMenuEditInfo()
    {
        isInfoMenu = false;
        infoMenu.SetActive(false);
        isEditMenu = true;
        editMenu.SetActive(true);
    }
    public void CloseMenuEditInfo()
    {
        isInfoMenu = true;
        infoMenu.SetActive(true);
        isEditMenu = false;
        editMenu.SetActive(false);
    }
    public void OnUpdateUser()
    {
        string value = inputDisplayName.text;
        bool isNicknameValid = value.Length >= 3 && value.Length <= 20 && !IsRestrictedNickname(value);

        inputDisplayName.onValueChanged.AddListener(ValidateNicknameLength);

        if (isNicknameValid)
        {
            inputDisplayName.onValueChanged.RemoveListener(ValidateNicknameLength);
            StartCoroutine(server.UpdateUser(deviceId, value));
            isSubmittingNickname = true;
            Debug.Log("SubmitNickname called");
            
            informationMenu.SetActive(true);
            informationText.text = "Bạn đã đổi tên thành công !";

            string nickname = inputDisplayName.text;
            gameData.display_name = nickname;
            SaveSystem.Save(gameData);

            isSubmittingNickname = false;
            checkRename = true;
        }
        else
        {
            // Gỡ bỏ sự kiện onValueChanged
            inputDisplayName.onValueChanged.RemoveListener(ValidateNicknameLength);
            // Hiển thị menu thông báo lỗi
            informationMenu.SetActive(true);
            informationText.text = "Nickname không hợp lệ! \nNickname không thể chứa chức danh đặc biệt( như  \"admin\", \"cloud\", \"player\",...) và phải chứa ít nhất 3 kí tự, không quá 20 kí tự.";
        }
    }
    private void ValidateNicknameLength(string value)
    {
        bool isNicknameValid = value.Length >= 3 && value.Length <= 20 && !IsRestrictedNickname(value);
    }

    private bool IsRestrictedNickname(string value)
    {
        string[] restrictedKeywords = { "Admin", "admin", "cloud", "Cloud", "Player", "player"};

        foreach (string keyword in restrictedKeywords)
        {
            if (value.Contains(keyword))
            {
                return true;
            }
        }
        return false;
    }

    public void CloseInformationMenu()
    {
        if (checkRename==true)
        {
            SceneManager.LoadScene("Account");
        }
        isInformation = false;
        informationMenu.SetActive(false);
        
    }


    public void UnloadSceneCoroutine()
    {
         SceneManager.LoadScene("MainScene");
    }

    public void DeleteAccount()
    {
        deviceId = SystemInfo.deviceUniqueIdentifier;
        ResetData();
        StartCoroutine(server.DeleteUser(deviceId));
        isExit = true;
        exitgame.SetActive(true);
    }
    public void Delete()
    {
        isInfoMenu = false;
        infoMenu.SetActive(false);
        isDeleteMenu = true;
        deleteMenu.SetActive(true);
    }
    public void CloseDelete()
    {
        isDeleteMenu = false;
        deleteMenu.SetActive(false);
        isInfoMenu = true;
        infoMenu.SetActive(true);
    }

    private void ResetData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.DeleteKey(SaveSystem.DataKey);
        gameData = new GameData();
        gameData.display_name = "";
        gameData.device_id = "";
        gameData.create_time = "";

        gameData.totalCoins = 0;
        gameData.totalCoinsPremium = 0;
        gameData.highestScore = 0;

        gameData.leverUnlocked = new bool[5];
        gameData.leverUnlocked[0] = true;
        gameData.charUnlocked = new bool[5];
        gameData.charUnlocked[1] = true;
        gameData.language = 0;
        gameData.qualitySettings = 0;
        gameData = SaveSystem.Load();
    }
}
