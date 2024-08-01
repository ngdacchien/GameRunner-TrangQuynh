using System.Collections;
using UnityEngine;

public class CameraLuong : MonoBehaviour
{
    public Transform target;
    private float trailDistance = 5.0f;
    private float heightOffset = 0.45f;
    private float cameraDelay = 0.01f;
    private bool isInTriggerWithPlayer = false;

    void Update()
    {
        Vector3 followPos = target.position - target.forward * trailDistance;
        followPos.y += heightOffset;

        if (isInTriggerWithPlayer)
        {
            // Sử dụng Vector3.Lerp để di chuyển mượt mà từ vị trí hiện tại đến followPos
            transform.position = Vector3.Lerp(transform.position, followPos, cameraDelay);
        }

        transform.LookAt(target.transform);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInTriggerWithPlayer = true;
            Invoke("ChangeCameraDelay", 1.09f);
        }
    }

    void ChangeCameraDelay()
    {
        cameraDelay = 1.0f; // Thay đổi giá trị của cameraDelay
        Debug.Log("Camera delay changed to 1.0f");
    }
}
