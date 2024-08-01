using System.Collections;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    //private PlayerController playerController;
    private PlayerManagers player;
    private BoxCollider boxCollider;
    public bool isTriggers;

    void Start()
    {
        //playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManagers>();
        boxCollider = GetComponent<BoxCollider>();
        isTriggers = false;
        boxCollider.isTrigger = false;
    }

    void Update()
    {
        if (PlayerManagers.isSpecial)
        {
            boxCollider.isTrigger = false;
        }
        else
        {
            boxCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PetRiding") || other.CompareTag("Player"))
        {
            if (PlayerManagers.isSpecial)
            {
                StatusSpecical();
            }
        } 
        if (other.CompareTag("Dog"))
        {
            StatusSpecical();
        }
    }
    public void StatusSpecical()
    {
        boxCollider.center = new Vector3(0, -1f, 0);
        isTriggers = false;
        boxCollider.isTrigger = false;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PetRiding") || other.CompareTag("Player"))
        {
            boxCollider.center = new Vector3(0, 0f, 0);
            isTriggers = true;
            boxCollider.isTrigger = true;
        }
    }
}