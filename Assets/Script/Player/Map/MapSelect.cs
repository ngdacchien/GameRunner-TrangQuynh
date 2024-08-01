using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelect : MonoBehaviour
{
    public List<GameObject> maps; // Danh sách các prefab của các bản đồ để sao chép
    private int currentMapIndex = 0; // Chỉ số của bản đồ hiện tại đang được sử dụng
    private bool isCheckClone;
    private GameObject currentMapInstance; // Tham chiếu đến bản đồ hiện tại đã được sao chép

    void Start()
    {
        isCheckClone = false;
        // Sao chép bản đồ đầu tiên từ danh sách
        currentMapInstance = Instantiate(maps[currentMapIndex], transform.position, Quaternion.identity);
    }
    private void Update()
    {
        if (MapTagClone.isTagClone && !isCheckClone)
        {
            CloneNextMap();
            StartCoroutine(ResetCheck());
        }
    }
    // Hàm để sao chép bản đồ tiếp theo từ danh sách
    public void CloneNextMap()
    {
        isCheckClone = true;
        // Tăng chỉ số để lấy bản đồ tiếp theo trong danh sách
        currentMapIndex += 1;

        // Nếu đã đi đến cuối danh sách, quay lại bản đầu tiên
        if (currentMapIndex >= maps.Count)
        {
            currentMapIndex = 0;
        }

        // Hủy bản đồ hiện tại sau 15 giây
        Destroy(currentMapInstance, 15f);

        // Sao chép bản đồ tiếp theo từ danh sách
        currentMapInstance = Instantiate(maps[currentMapIndex], transform.position, Quaternion.identity);
    }
    IEnumerator ResetCheck()
    {
        yield return new WaitForSeconds(0.5f);
        isCheckClone = false;
    }
}
