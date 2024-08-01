using UnityEngine;

public class Banana : MonoBehaviour
{
    private float rotationSpeed = 100f; // Tốc độ xoay của vật thể


    void Update()
    {
        // Lấy góc quay hiện tại của vật thể
        Vector3 rotation = transform.rotation.eulerAngles;

        // Cập nhật góc quay trục Y theo tốc độ xoay và thời gian
        rotation.y += rotationSpeed * Time.deltaTime;

        // Đặt lại góc quay của vật thể
        transform.rotation = Quaternion.Euler(rotation);

    }

}