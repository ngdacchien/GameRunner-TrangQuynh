using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class FollowerCamera : MonoBehaviour
//{
//    public Transform target; // Đối tượng được theo dõi

//    public LayerMask groundLayer; // Các lớp để xem xét cho va chạm với mặt đất
//    private float distanceFromGround = 0f; // Khoảng cách giữa camera và mặt đất

//    private bool isJumping = false; // Biến để kiểm tra trạng thái nhảy
//    private float jumpForce; // Lực đẩy của camera khi nhân vật nhảy
//    private float currentJumpHeight; // Chiều cao hiện tại của nhảy

//    private Vector3 startPosition; // Vị trí ban đầu của camera
//    private float jumpPeakHeight = 2f; // Chiều cao tối đa của nhảy

//    void LateUpdate()
//    {
//        if (target != null)
//        {
//            // Lấy vị trí của đối tượng được theo dõi
//            Vector3 targetPosition = target.position;
//            // Nếu nhân vật đang nhảy, áp dụng lực đẩy lên camera
//            if (isJumping || PlayerManagers.isJump2 || PlayerManagers.isJump)
//            {
//                currentJumpHeight += jumpForce * Time.deltaTime;
//                currentJumpHeight = Mathf.Clamp(currentJumpHeight, 0f, 2f); // Giới hạn chiều cao nhảy

//                Vector3 newPosition = new Vector3(targetPosition.x, targetPosition.y + currentJumpHeight, targetPosition.z);
//                transform.position = newPosition;

//                // Nếu đã đạt đến chiều cao tối đa, kết thúc nhảy
//                if (currentJumpHeight >= 2f)
//                {
//                    isJumping = false;
//                }
//            }
//            else
//            {
//                // Thực hiện raycast để điều chỉnh chiều cao của camera
//                RaycastHit hit;
//                if (Physics.Raycast(targetPosition + Vector3.up * 10f, Vector3.down, out hit, Mathf.Infinity, groundLayer))
//                {
//                    // Tính toán vị trí mới với một lớp nhỏ trên mặt đất
//                    Vector3 newPosition = hit.point + Vector3.up * distanceFromGround;

//                    // Đặt vị trí của camera vào vị trí mới tính toán
//                    transform.position = newPosition;
//                }
//                else
//                {
//                    // Nếu không có mặt đất được va chạm (hiếm khi xảy ra), quay lại theo dõi vị trí của đối tượng mục tiêu
//                    transform.position = targetPosition;
//                }
//            }

//            //transform.position = target.position;
//            transform.rotation = target.rotation;
//        }
//    }
//}

public class FollowerCamera : MonoBehaviour
{
    public Transform target; // Đối tượng được theo dõi
    public LayerMask groundLayer; // Các lớp để xem xét cho va chạm với mặt đất

    private bool isJumping = false; // Biến để kiểm tra trạng thái nhảy

    private float jumpDuration = 1f; // Thời gian (giây) camera nâng lên và hạ xuống
    private float currentJumpTime = 0f; // Thời gian hiện tại đã trôi qua trong quá trình nhảy

    private Vector3 startPosition; // Vị trí ban đầu của camera
    private float jumpPeakHeight = 1.2f; // Chiều cao tối đa của nhảy

    private float groundCheckDistance = 10f; // Khoảng cách từ mặt đất để xác định vị trí của camera

    private float descendDuration = 0.5f; // Thời gian hạ dần của camera
    private float currentDescendTime = 0f; // Thời gian hiện tại đã trôi qua trong quá trình hạ dần

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position;

            if (isJumping)
            {
                currentJumpTime += Time.deltaTime;

                // Tính toán phần trăm hoàn thành của quá trình nhảy
                float jumpProgress = currentJumpTime / jumpDuration;

                // Sử dụng hàm SmoothStep để di chuyển mượt mà
                float smoothStepProgress = Mathf.SmoothStep(0f, 1f, jumpProgress);

                // Lerp từ vị trí ban đầu đến vị trí cao nhất của nhảy, sau đó lerp từ đó đến vị trí cuối cùng
                float currentJumpHeight = Mathf.Lerp(0f, jumpPeakHeight, smoothStepProgress);
                Vector3 newPosition = new Vector3(targetPosition.x, targetPosition.y + currentJumpHeight, targetPosition.z);

                // Đặt vị trí của camera vào vị trí mới tính toán
                transform.position = newPosition;

                // Kết thúc nhảy khi hoàn thành
                if (currentJumpTime >= jumpDuration)
                {
                    isJumping = false;
                    currentJumpTime = 0f;

                    // Bắt đầu hạ dần camera xuống
                    currentDescendTime = 0f;
                }
            }
            else if (currentDescendTime < descendDuration)
            {
                currentDescendTime += Time.deltaTime;

                // Tính toán phần trăm hoàn thành của quá trình hạ dần
                float descendProgress = currentDescendTime / descendDuration;

                // Lerp từ vị trí cao nhất của nhảy xuống vị trí ban đầu
                float currentDescendHeight = Mathf.Lerp(jumpPeakHeight, 0f, descendProgress);
                Vector3 newPosition = new Vector3(targetPosition.x, targetPosition.y + currentDescendHeight, targetPosition.z);

                // Đặt vị trí của camera vào vị trí mới tính toán
                transform.position = newPosition;

                // Đặt lại vị trí khi hoàn thành hạ dần
                if (currentDescendTime >= descendDuration)
                {
                    transform.position = targetPosition;
                }
            }
            else
            {
                // Thực hiện raycast để điều chỉnh chiều cao của camera
                RaycastHit hit;
                if (Physics.Raycast(targetPosition + Vector3.up * groundCheckDistance, Vector3.down, out hit, Mathf.Infinity, groundLayer))
                {
                    Vector3 newPosition = hit.point;
                    transform.position = newPosition;
                }
                else
                {
                    transform.position = targetPosition;
                }
            }

            transform.rotation = target.rotation;
        }
    }

    public void StartJump(float duration)
    {
        isJumping = true;
        jumpDuration = duration;
        currentJumpTime = 0f;
    }
}
