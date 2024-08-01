using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;
using static Server;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.SceneManagement;

public class CloseLeader : MonoBehaviour
{
    public void CloseMenu()
    {
        SceneManager.LoadScene("MainScene");
    }
}
