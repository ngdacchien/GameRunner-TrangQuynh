using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Player Settings")]
    public Transform _Player;
    public SplineComputer _SplineComputer;
    public SplineFollower _SplineFollower;
    public static float speed;
    public static float swerveSpeed;
    public static float swerveRange;
    private static float numberMove;
    public bool _isMoving = false;
    public static bool _isReplay;


    [Header("Swipe Mobile")]
    //Swipe
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private Vector2 currentPosition;
    private bool stopTouch = false;

    public float swipeRange = 30;
    public float tabRange = 10;

    public static bool swipeLeft = false;
    public static bool swipeRight = false;
    public static bool swipeUp = false;
    public static bool swipeDown = false;
    public static bool swipeTab = false;
    private Vector2 Distance;
    //endSwipe

    public void SwipeMobile()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    stopTouch = false;
                    break;

                case TouchPhase.Moved:
                    currentPosition = touch.position;
                    Distance = currentPosition - startTouchPosition;

                    if (!stopTouch)
                    {
                        if (Distance.x < -swipeRange)
                        {
                            swipeLeft = true;
                            stopTouch = true;
                        }
                        else if (Distance.x > swipeRange)
                        {
                            swipeRight = true;
                            stopTouch = true;
                        }
                        else if (Distance.y < -swipeRange)
                        {
                            swipeDown = true;
                            stopTouch = true;
                        }
                        else if (Distance.y > swipeRange)
                        {
                            swipeUp = true;
                            stopTouch = true;
                        }

                        // New conditions for diagonal swipes
                        else if (Distance.x < -swipeRange && Distance.y < -swipeRange)
                        {
                            swipeDown = true;
                            //swipeDownLeft = true;
                            stopTouch = true;
                        }
                        else if (Distance.x > swipeRange && Distance.y < -swipeRange)
                        {
                            swipeDown = true;
                            //swipeDownRight = true;
                            stopTouch = true;
                        }
                        else if (Distance.x < -swipeRange && Distance.y > swipeRange)
                        {
                            swipeUp = true;
                            //swipeUpLeft = true;
                            stopTouch = true;
                        }
                        else if (Distance.x > swipeRange && Distance.y > swipeRange)
                        {
                            swipeUp = true;
                            //swipeUpRight = true;
                            stopTouch = true;
                        }
                    }
                    break;

                case TouchPhase.Ended:
                    if (!stopTouch)
                    {
                        endTouchPosition = touch.position;
                        Distance = endTouchPosition - startTouchPosition;
                        if (Mathf.Abs(Distance.x) < tabRange && Mathf.Abs(Distance.y) < tabRange)
                        {
                            swipeTab = true;
                        }
                    }
                    stopTouch = false;
                    break;
            }
        }
    }
    void Start()
    {
        _Player.localPosition = Vector3.zero;
        _SplineFollower.follow = false;
        speed = 0;
        swerveSpeed = 5;
        swerveRange = 1.5f;
        numberMove = 0;
        _isReplay = false;
    }

    void Update()
    {
        SwipeMobile();

        PlayerMoving();
        //StopMovement();
        if (!GameController.isGameStarted || GameController.gameOver) { return; }
        if (GameController.isGameOn && !GameController.isPause && !PlayerManagers.isSlow && !PlayerManagers.isSpecial && !PlayerManagers.isDead && !PlayerManagers.isSlow)
        {
            speed = 8;
        }
        else if (GameController.isGameOn && !GameController.isPause && PlayerManagers.isSpecial)
        {
            speed = 16;
        }
        else if (GameController.isGameOn && !GameController.isPause && PlayerManagers.isSlow)
        {
            speed = 5;
        }
        else if (GameController.isGameOn && !GameController.isPause && PlayerManagers.isDead)
        {
            speed = 0;
            //StopMovement();
        }
        else if (!GameController.isGameStarted) { speed = 0; }
        else { speed = 0; }


        // Cập nhật followSpeed mỗi frame
        _SplineFollower.followSpeed = speed;

        if (_isMoving)
        {

            if (Input.GetKey(KeyCode.LeftArrow) || swipeLeft) // Sử dụng phím mũi tên trái để di chuyển sang trái
            {
                numberMove -= 1.5f;
                swipeLeft = false;
            }
            if (Input.GetKey(KeyCode.RightArrow) || swipeRight)
            {
                numberMove += 1.5f;
                swipeRight = false;
            }
            numberMove = Mathf.Clamp(numberMove, -swerveRange, swerveRange);
            SwerveMovement();
        }
    }

    public void SwerveMovement()
    {
        var playerPosition = _Player.localPosition;
        var targetPositionX = Mathf.Lerp(playerPosition.x, numberMove, Time.deltaTime * swerveSpeed);
        targetPositionX = Mathf.Clamp(targetPositionX, -swerveRange, swerveRange);
        var newPosition = new Vector3(targetPositionX, playerPosition.y, playerPosition.z);
        _Player.localPosition = newPosition;
    }

    public void PlayerMoving()
    {
        //speed += 0.01f; 
        //if (Input.GetKeyDown(KeyCode.Space) || swipeTab)
        //{
            GameController.isGameStarted = true;
            _SplineFollower.follow = true;
            _isMoving = true;
            swipeTab = false;
        //}
    }
    public void StopMovement()
    {
        if (PlayerManagers.isDead)
        {
            _isMoving = false;
            _SplineFollower.follow = false;

        }

    }
}