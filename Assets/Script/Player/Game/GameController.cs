using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private Server server;
    public GameObject SpecialTime;
    public Text specialTime;
    private bool isSpecialTime;
    private float playedSeconds;
    public static bool gameOver;
    public GameObject gameOverMenu;
    //public GameObject gameOverPopup;
    public GameObject gamePauseMenu;
    public GameObject gameOnMenu;
    private bool checkCoins;
    public Transform playerScore;
    public static int numberCoins;
    public Text coinsText;
    public Text gameOverCoin;
    public static int numberCoinPremium;
    public Text coinPremiumText;
    //public Text highestText;
    public static int numberScore;
    public Text scoreText;
    public Text totalScore;
    public static int totalNumber;

    public Text gameOverScore;
    public static bool isGameStarted;
    //public GameObject startText;
    public static bool clickOver;
    public static bool isPause;
    public static bool isReplay;
    public static bool isGameOn;
    public static bool isPoint;
    //public GameObject Point;
    public GameData gameData;

    private float elapsedTime; // Thời gian đã chơi
    private float currentSpeed; // Tốc độ hiện tại của người chơi

    private float startScore; // Điểm số khi bắt đầu tính toán với tốc độ hiện tại
    private float startTime; // Thời gian bắt đầu tính toán với tốc độ hiện tại

    
    void Start()
    {
        Time.timeScale = 1;
        playedSeconds = 0;
        isSpecialTime = false;
        SpecialTime.SetActive(false);
        checkCoins = false;
        gameOver = false;   
        clickOver = false;
        isPause = false;
        isGameOn = true;
        isReplay = false;
        isGameStarted = false;
        isPoint = false;
        //Point.SetActive(false);
        numberCoins = 0;
        numberCoinPremium = 0;
        numberScore = 0;
        gameData = SaveSystem.Load();
    }
    void Update()
    {
        
        server = FindObjectOfType<Server>();

        
         if (PlayerManagers.specialDuration < 0)
        {
            PlayerManagers.specialDuration = 0;
            specialTime.color = Color.red; // Đặt màu chữ thành màu đỏ
        }
        else
        {
            specialTime.color = Color.white; // Đặt màu chữ về mặc định
        }

        if (PlayerManagers.isSpecial == true)
        {
            isSpecialTime = true;
            SpecialTime.SetActive(true);
            specialTime.text = PlayerManagers.specialDuration.ToString("F2");
        }
        else
        {
            isSpecialTime = false;
            SpecialTime.SetActive(false);
        }
        UpdateSpecialTime(PlayerManagers.specialDuration, PlayerManagers.isSpecial);
        UpdateUI();
        // Kiểm tra điều kiện game over
        if (gameOver)
        {
            // Thực hiện các hành động khi game over
            Time.timeScale = 0;
            gameOnMenu.SetActive(false);
            gameOverMenu.SetActive(true);
            
            //if(!clickOver){
            //    gameOverPopup.SetActive(true);
            //}
            
            GameOver();
        }
        else
        {
            // Đảm bảo rằng thời gian tiếp tục chạy khi không game over hoặc pause
            Time.timeScale = 1;
        }

    }
    void UpdateSpecialTime(float duration, bool isSpecial)
    {
        if (duration < 0)
        {
            duration = 0;
            specialTime.color = Color.red; // Đặt màu chữ thành màu đỏ
        }
        else
        {
            specialTime.color = Color.white; // Đặt màu chữ về mặc định
        }

        if (isSpecial)
        {
            isSpecialTime = true;
            SpecialTime.SetActive(true);
            specialTime.text = duration.ToString("F2");
        }
        else
        {
            isSpecialTime = false;
            SpecialTime.SetActive(false);
        }
    }
    void UpdateUI()
    {
        coinsText.text = "" + numberCoins;
        gameOverCoin.text = "+ " + numberCoins;
        coinPremiumText.text = "" + numberCoinPremium;
        scoreText.text = numberScore + "";
        gameOverScore.text = numberScore + "";
        totalNumber = numberScore + numberCoins;
        totalScore.text = totalNumber.ToString();
        //if (clickOver == true)
        //{
        //    gameOverPopup.SetActive(false);
        //}

        // Hiển thị/ẩn menu tạm dừng dựa trên trạng thái của isPause
        gamePauseMenu.SetActive(isPause);

        if (isReplay == true)
        {
            gameOver = false;
            gameOnMenu.SetActive(true);
            gameOverMenu.SetActive(false);
            //gameOverPopup.SetActive(false);
        }
        if (isGameStarted == false) { return; }
        else if (isGameStarted == true)
        {
            //Destroy(startText);
            //Vận tốc *thời gian
            if (!gameOver && !isPause)
            {
                playedSeconds += Time.deltaTime * Movement.speed / 5;

                // Tính số giây đã chơi
                //float playedSeconds = Time.timeSinceLevelLoad;

                // Tính điểm dựa trên số giây và tốc độ, với tỷ lệ tốc độ
                //float scoreBasedOnSpeed = playedSeconds * Movement.speed * 5;
                float scoreBasedOnSpeed = playedSeconds * 20;
                // Chia điểm số dựa trên tỷ lệ tốc độ
                numberScore = Mathf.FloorToInt(scoreBasedOnSpeed);
            }
        }

        
    }
    public void OnClickPause()
    {
        isPause = true;
    }
    public void OnClickContinue()
    {
        isPause = false;
    }
    public void OnClickReplay()
    {
        isReplay = true;
        Movement._isReplay = true;
        PlayerManagers.isDead = false;

        SceneManager.LoadScene("Game_Spline");

    }
    public void OnClickMainScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainScene");
        isGameStarted = false;
    }
    public void OnLeaderboard()
    {
        Time.timeScale = 1;
        isGameStarted = false;
        SceneManager.LoadScene("Leaderboard");
    }
    public void OnClickClose()
    {
        clickOver = true;
        
    }
    public void GameOver()
    {
        if (totalNumber > gameData.highestScore)
        {
            gameData.highestScore = totalNumber;
        }

        int exCoins = gameData.totalCoins;
        gameData.CoinsEx = (int)exCoins;
        gameData.CoinsNew = (int)numberCoins;

        if (!checkCoins)
        {
            int total = exCoins + (int)numberCoins;
            gameData.totalCoins = (int)total;
            gameData.totalCoinsPremium += (int)numberCoinPremium;
            isPoint = true;
            //Point.SetActive(true);
            //highestText.text = gameData.highestScore.ToString();
            SaveSystem.Save(gameData);
            string deviceId = gameData.device_id;
            int coinlast = gameData.totalCoins;
            int highestScore = gameData.highestScore;

            StartCoroutine(server.UpdateCoin(deviceId, coinlast));
            StartCoroutine(server.UpdateScore(deviceId, highestScore));

            checkCoins = true;
        }
        Time.timeScale = 1;
    }
}
