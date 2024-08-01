using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTagClone : MonoBehaviour
{
    public static bool isTagClone;
    private void Start()
    {
        isTagClone = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra nếu đối tượng va chạm có tag là "Player"
        if (other.CompareTag("Player")&& !isTagClone)
        {
            isTagClone = true;
            StartCoroutine(ResetTag());
        }
    }
    IEnumerator ResetTag()
    {
        yield return new WaitForSeconds(0.5f);
        isTagClone = false ;
    }
}
