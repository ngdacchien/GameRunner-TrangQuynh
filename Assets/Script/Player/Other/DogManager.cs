using Dreamteck.Splines;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DogManager : MonoBehaviour
{
    public Transform _Player;
    public SplineComputer _SplineComputer;
    public SplineFollower _SplineFollower;
    public Animator _animation;
    public static float _Dogspeed;
    private bool isCheckIntro;

    public Transform target;
    public LayerMask groundLayer;
    private float groundCheckDistance = 10f;

    private void Start()
    {
        isCheckIntro = false;
    }
    private void Update()
    {
        DogMovement();
        
        if(PlayerManagers.isIntro && !isCheckIntro)
        {
            isCheckIntro = true;
            FindObjectOfType<AudioManagers>().PlaySound("ChoDuoi");
            StartCoroutine(Intro());
        }
        if (PlayerManagers.isSlow)
        {
            StartCoroutine(Dog_Attack());
        }
        if (target != null)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up * groundCheckDistance, Vector3.down, out hit, Mathf.Infinity, groundLayer))
            {
                Vector3 newPosition = hit.point;
                transform.position = newPosition;
            }
            
        }
    }
    
    private IEnumerator Intro()
    {
        _animation.SetBool("isRun", true);
        _animation.SetBool("isAttack", false);
        yield return new WaitForSeconds(1f);
        _animation.SetBool("isRun", false);
        _animation.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.5f);
        _animation.SetBool("isAttack", false);
        _animation.SetBool("isRun", true);
        yield return new WaitForSeconds(1f);
        _animation.SetBool("isRun", false);
        _animation.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.5f);
        _animation.SetBool("isAttack", false);
        _animation.SetBool("isRun", true);
    }
    private IEnumerator Dog_Attack()
    {
        _animation.SetBool("isRun", false);
        _animation.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.5f);
        _animation.SetBool("isAttack", false);
        _animation.SetBool("isRun", true);
    }
    public void DogMovement()
    {
        if(!GameController.isGameStarted)
        {
            _animation.SetBool("isHit", true);
            _animation.SetBool("isAttack", true);
            _animation.SetBool("isRun", false);
        }

        if (GameController.isGameStarted && !PlayerManagers.isIntro && !PlayerManagers.isSlow)
        {
            _animation.SetBool("isHit", false);
            _animation.SetBool("isAttack", false);
            _animation.SetBool("isRun", true);
            
        }
    }
    
}