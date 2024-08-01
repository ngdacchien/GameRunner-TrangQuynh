using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public GameData gameData;
    
    private void Awake()
    {
        gameData = SaveSystem.Load();
    }
    void Start()
    {
        
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        gameData = SaveSystem.Load();
    }
}
