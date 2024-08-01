using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManagers : MonoBehaviour
{
    private FollowerCamera followerCamera; // Tham chiếu đến FollowerCamera
    private CharacterController characterController;
    public GameObject PetRiding;
    public static bool isPetRiding;
    public Animator animator;
    public static bool isSipping;
    public static bool isJump;
    public static bool isJump2;
    public static bool isDead;
    public static bool isChase;
    public static bool isSlow;
    public static float slowNumber;
    public static bool isIntro;
    public static bool introStarted; // Biến để kiểm tra đã kích hoạt Intro chưa
    private float introTimer = 0f; // Biến để tính thời gian đã chạy của Intro

    //public LayerMask groundLayer;
    //public Transform groundCheck;
    //public bool isGrounded;

    public bool isNormal; // Trạng thái animation bình thường
    public static bool isSpecial; // Trạng thái animation đặc biệt
    private bool isSpecialRun; // Trạng thái chạy animation đặc biệt
    public static float specialDuration; // Thời gian sử dụng hiệu ứng đặc biệt
    
    public void OnPetRiding()
    {
        if (isPetRiding == true)
        {
            PetRiding.SetActive(true);
        }
        else if (isPetRiding == false)
        {
            PetRiding.SetActive(false);
        }
    }
    void Start()
    {
        slowNumber = 0;
        isIntro = false;
        introStarted = false;
        isChase = false;
        isSlow = false;
        isDead = false;
        isJump = false;
        isJump2 = false;
        isSipping = false;
        isPetRiding = false;
        PetRiding.SetActive(false);
        isNormal = false;
        isSpecial = false;
        isSpecialRun = false;
        characterController = GetComponent<CharacterController>();
        followerCamera = FindObjectOfType<FollowerCamera>(); // Tìm FollowerCamera trong scene
    }


    void Update()
    {
        OnPetRiding();
        
        if (!GameController.isGameStarted || GameController.gameOver) { return; }

        

        if (!isIntro && GameController.isGameStarted && introStarted==false && SceneManager.GetActiveScene().name == "Game_Spline")
        {
            introStarted = true;
            FindObjectOfType<AudioManagers>().PlaySound("BiChoDuoi");
            StartCoroutine(Intro());
        }
        if (isSlow == true)
        {
            StartCoroutine(Chasing_Dog());
        }
        if (isSpecial)
        {
            StartCoroutine(OnSpecials());
        }
        if (isNormal)
        {
            animator.SetBool("isNormal", true);
            
            if (Input.GetKeyDown(KeyCode.UpArrow) && !isJump2 && !isJump|| Movement.swipeUp && !isJump2 && !isJump)
            {
                StartCoroutine(Jump());
                
            }
            
            if (Input.GetKeyDown(KeyCode.DownArrow) && !isSipping || Movement.swipeDown && !isSipping)
            {
                StartCoroutine(Slipe());
            }
        }
    }
    public IEnumerator Intro()
    {
        isIntro = true;
        animator.SetBool("isIntro", true);
        animator.SetBool("isRunIntro", true);
        animator.SetBool("isIntroJump", false);
        yield return new WaitForSeconds(1f);
        animator.SetBool("isIntro", true);
        animator.SetBool("isRunIntro", false);
        animator.SetBool("isIntroJump", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isIntro", true);
        animator.SetBool("isIntroJump", false);
        animator.SetBool("isRunIntro", true);
        yield return new WaitForSeconds(1f);
        animator.SetBool("isIntro", true);
        animator.SetBool("isRunIntro", false);
        animator.SetBool("isIntroJump", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isIntro", true);
        animator.SetBool("isIntroJump", false);
        animator.SetBool("isRunIntro", true);
        yield return new WaitForSeconds(0.01f);
        isIntro = false;
        animator.SetBool("isIntro", false);
        animator.SetBool("isRunIntro", false);
        isNormal = true;
        animator.SetBool("isGameStarted", true);
    }

    public IEnumerator Chasing_Dog()
    {
        isChase = true;
        animator.SetBool("isChase", true);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isChase", false);
        isChase = false;
        isSlow = false;
    }

    private IEnumerator OnDead()
    {
        isDead = true;
        isNormal = false;
        animator.SetBool("isNormal", false);
        animator.SetBool("isDead", true);
        yield return new WaitForSeconds(1f);
        GameController.gameOver = true;

    }
    public void OnTriggerEnter(Collider onSpecial)
    {
        if (onSpecial.tag == "SpecialItems")
        {
            // Kích hoạt animation đặc biệt khi va chạm với vật có tag "SpecialItems"
            isNormal = false;
            isSpecial = true;
            specialDuration = 10f;
        }
        if (onSpecial.tag == "Obstancle" && isSpecial == false)
        {
            //FindObjectOfType<AudioManager>().PlaySound("GameOver");
            StartCoroutine(OnDead());
        }
        if (onSpecial.tag == "Slow" && isSpecial == false)
        {
            //FindObjectOfType<AudioManager>().PlaySound("GameOver");
            isSlow = true;
        }
    }
    private IEnumerator OnSpecials()
    {
        specialDuration -= Time.deltaTime;
        if (specialDuration <= 0)
        {
            // Kết thúc animation đặc biệt
            isSpecial = true;
            isSpecialRun = false;
            animator.SetBool("isSpecial", true);
            animator.SetBool("isSpecialRun", false);
            isPetRiding = false;
            yield return new WaitForSeconds(0.2f);

            animator.SetBool("isSpecial", false);
            animator.SetBool("isSpecialRun", false);

            isSpecialRun = false;
            isSpecial = false;
            isNormal = true;
            //characterController.center = new Vector3(0, 0.125f, 0);
        }
        else if (isSpecialRun == true)
        {
            // Đang chạy animation đặc biệt
            animator.SetBool("isSpecial", true);
            animator.SetBool("isSpecialRun", true);
            //characterController.center = new Vector3(0, 0.5f, 0);
        }
        else
        {
            // Kích hoạt animation đặc biệt
            isSpecialRun = true;
            animator.SetBool("isSpecial", true);
            animator.SetBool("isSpecialRun", false);
            yield return new WaitForSeconds(0.6f);
            isPetRiding = true;
        }
    }
    public IEnumerator Jump()
    {
        //int randomAction = Random.Range(0, 2);
        //if (randomAction == 0)
        //{
            isJump2 = true;
            animator.SetBool("isJump2", true);
            // Gọi hàm StartJump từ FollowerCamera để bắt đầu nhảy
            if (followerCamera != null)
            {
                followerCamera.StartJump(0.8f); // Chỉ định thời gian nhảy
            }
            FindObjectOfType<AudioManagers>().PlaySound("JumpSound");
            yield return new WaitForSeconds(1.4f);
            animator.SetBool("isJump2", false);
            isJump2 = false;
        //}
        //else
        //{
        //    isJump = true;
        //    animator.SetBool("isJump", true);
        //    FindObjectOfType<AudioManagers>().PlaySound("JumpSound");
        //    yield return new WaitForSeconds(1.3f);
        //    animator.SetBool("isJump", false);
        //    isJump = false;
        //}
        Movement.swipeUp = false;
    }

    public IEnumerator Slipe()
    {
        Movement.swipeDown = false;
        isSipping = true;
        animator.SetBool("isSlipping", true);
        FindObjectOfType<AudioManagers>().PlaySound("SlideSound");
        characterController.center = new Vector3(0, 0.05f, 0);
        characterController.height = 0.1f;
        yield return new WaitForSeconds(1f);

        characterController.center = new Vector3(0, 0.125f, 0);
        characterController.height = 0.25f;
        animator.SetBool("isSlipping", false);
        isSipping = false;
    }
}
