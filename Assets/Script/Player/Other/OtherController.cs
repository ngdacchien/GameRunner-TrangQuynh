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

public class OtherController : MonoBehaviour
{
    public bool isNicknameMenu;
    public GameObject nicknameMenu;
    public bool isMainMenu;
    public GameObject mainMenu;

    public void EventMenu()
    {

    }
    void Start()
    {
        isNicknameMenu = true;
        nicknameMenu.SetActive(true);
        isMainMenu = false;
        mainMenu.SetActive(false);
    }

    void Update()
    {
        
    }

    

}
