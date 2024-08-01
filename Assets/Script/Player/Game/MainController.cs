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
using System.Net.Mail;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    private Server server;
    string device_id;
    //----------------------//
    private GameData gameData;
    public Text displayname;
    public Text coin;

    public bool isNicknameMenu;
    public GameObject nicknameMenu;
    public bool isMainMenu;
    public GameObject mainMenu;

    public InputField nicknameField;
    private bool isSubmittingNickname = false;
    public bool isSubmitSuccess = false;
    public bool isInformation;
    public GameObject informationMenu;
    public Text informationText;
    public bool isInformationError;
    public GameObject informationMenuError;
    public GameObject Oncharacter;
    public bool isOnCharacter = true;

    public GameObject thongbao;
    public bool isthongbao;
    public Text textthongbao;
    public void EventMenu()
    {
        isthongbao = true;
        thongbao.SetActive(true);
        textthongbao.text = "Chức năng này đang bảo trì.";
    }
    public void CloseEventMenu()
    {
        isthongbao = false;
        thongbao.SetActive(false);
    }

    void Start()
    {
        device_id = SystemInfo.deviceUniqueIdentifier;
        gameData = SaveSystem.Load();
        gameData.device_id = device_id;
        SaveSystem.Save(gameData);
        isNicknameMenu = true;
        nicknameMenu.SetActive(true);
        isMainMenu = false;
        mainMenu.SetActive(false);
        isInformation = false;
        informationMenu.SetActive(false);
        isthongbao = false;
        thongbao.SetActive(false);
        isOnCharacter = true;
        Oncharacter.SetActive(true);
    }

    void Update()
    {
        gameData = SaveSystem.Load();
        server = FindObjectOfType<Server>();
        device_id = SystemInfo.deviceUniqueIdentifier;
        gameData.device_id = device_id;
        displayname.text = gameData.display_name;
        coin.text = gameData.totalCoins.ToString();
        if (isSubmitSuccess == true)
        {
            isOnCharacter = false;
            Oncharacter.SetActive(false);
            informationMenu.SetActive(true);
            informationText.text = gameData.display_name + "";
        }
        if (gameData.display_name.Length < 2)
        {
            isNicknameMenu = true;
            nicknameMenu.SetActive(true);
            isMainMenu = false;
            mainMenu.SetActive(false);
        }
        else
        {
            isNicknameMenu = false;
            nicknameMenu.SetActive(false);
            isMainMenu = true;
            mainMenu.SetActive(true);
        }
    }

    public void SubmitNickname()
    {
        string value = nicknameField.text;
        bool isNicknameValid = value.Length >= 3 && value.Length <= 20 && !IsRestrictedNickname(value);

        // Gắn lại sự kiện onValueChanged
        nicknameField.onValueChanged.AddListener(ValidateNicknameLength);

        if (isNicknameValid)
        {
            // Gỡ bỏ sự kiện onValueChanged
            nicknameField.onValueChanged.RemoveListener(ValidateNicknameLength);


            isSubmittingNickname = true;
            Debug.Log("SubmitNickname called");

            string nickname = nicknameField.text;
            StartCoroutine(server.CreateUser(device_id, nickname));
            gameData.device_id = device_id;
            gameData.display_name = nickname;
            SaveSystem.Save(gameData);

            //SceneManager.LoadScene("MainScene");
            // Trả về dữ liệu ngay lập tức
            isNicknameMenu = false;
            nicknameMenu.SetActive(false);
            isMainMenu = true;
            mainMenu.SetActive(true);
            gameData = SaveSystem.Load();
            isInformation = true;
            isSubmitSuccess = true;
            isSubmittingNickname = false;
        }
        else
        {
            // Gỡ bỏ sự kiện onValueChanged
            nicknameField.onValueChanged.RemoveListener(ValidateNicknameLength);
            // Hiển thị menu thông báo lỗi
            isInformationError = true;
            informationMenuError.SetActive(true);
            //informationText.text = "Nickname không hợp lệ! \nNickname không thể chứa chức danh đặc biệt( như  \"admin\", \"cloud\", \"player\",...) và phải chứa ít nhất 3 kí tự, không quá 20 kí tự.";
        }
    }

    private void ValidateNicknameLength(string value)
    {
        bool isNicknameValid = value.Length >= 3 && value.Length <= 20 && !IsRestrictedNickname(value);
    }

    private bool IsRestrictedNickname(string value)
    {
        string[] restrictedKeywords = { "Admin", "admin", "cloud", "Cloud", "Player", "player" };

        foreach (string keyword in restrictedKeywords)
        {
            if (value.Contains(keyword))
            {
                return true;
            }
        }

        return false;
    }
    public void OnInfo()
    {
        SceneManager.LoadScene("Account");
        //asyncs.allowSceneActivation = false;

    }
    public void OnLeaderboad()
    {
        SceneManager.LoadScene("Leaderboard");
        //asyncs.allowSceneActivation = false;

    }
    public void CloseMenu()
    {
        isOnCharacter = true;
        Oncharacter.SetActive(true);
        isInformation = false;
        informationMenu.SetActive(false);
        isInformationError = false;
        informationMenuError.SetActive(false);
        SceneManager.LoadScene("MainScene");
    }
}
