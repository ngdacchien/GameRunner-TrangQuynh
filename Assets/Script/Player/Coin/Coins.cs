using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Coins : MonoBehaviour
{
    public GameObject coin;
    //private bool isHidden;
    //private void Start()
    //{
    //    isHidden = false;
    //}
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(50 * Time.deltaTime, 0, 0);
        //StartCoroutine(Hidden());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "PetRiding")
        {
            FindObjectOfType<AudioManagers>().PlaySound("PickCoin");
            GameController.numberCoins += 100;
            //isHidden= true;
            //coin.SetActive(false);
            Destroy(gameObject);
        }
    }
    //private IEnumerator Hidden()
    //{
    //    if (isHidden)
    //    {
    //        yield return new WaitForSeconds(10f);
    //        isHidden = false;
    //        coin.SetActive(true);
    //    }
    //}
}
