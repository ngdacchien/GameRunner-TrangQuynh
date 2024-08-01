using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    void Update()
    {
       OnTriggerEnterCheck();
    }
    private void OnTriggerEnterCheck()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.1f); 
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Bode"))
            {
                Destroy(gameObject);
                Destroy(collider.gameObject);
            }
        }
    }
}
