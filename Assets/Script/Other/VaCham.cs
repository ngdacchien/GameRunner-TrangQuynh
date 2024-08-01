using UnityEngine;

public class VaCham : MonoBehaviour
{
    public GameObject chuoi;

    // Khởi tạo đối tượng chuoi là ẩn ban đầu
    private void Start()
    {
        chuoi.SetActive(false);
    }

    // Khi một đối tượng khác va chạm với trigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("VCChuoi"))
        {
            chuoi.SetActive(true);
        }
    }

    // Khi một đối tượng khác rời khỏi trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("VCChuoi"))
        {
            chuoi.SetActive(false);
        }
    }
}