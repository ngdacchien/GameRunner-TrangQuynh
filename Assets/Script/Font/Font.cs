using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Font : MonoBehaviour
{
    public Text textComponent;
    //public AudioSource audioSource;

    private string fullText;
    private bool textDisplayed = false; // Biến để kiểm tra xem chữ đã hiển thị xong hay chưa

    void Start()
    {
        StartCoroutine(ShowText());
    }

    private IEnumerator ShowText()
    {
        fullText = textComponent.text;

        for (int i = 0; i <= fullText.Length; i++)
        {
            textComponent.text = fullText.Substring(0, i);

            // Play sound
            if (i < fullText.Length)
            {
                //PlaySound("BanPhim");
                yield return new WaitForSeconds(0.007f);
            }
        }

        textDisplayed = true; // Đánh dấu là chữ đã hiển thị xong

        // Chờ thêm một khoảng thời gian bằng tổng thời gian hiển thị fulltext
        yield return new WaitForSeconds(fullText.Length * 0.007f);

        // Tắt âm thanh nếu chữ đã hiển thị xong
        //if (textDisplayed)
        //{
        //    audioSource.Stop();
        //}

        // Sau khi chờ xong, bạn có thể thực hiện các hành động khác ở đây
        // Ví dụ: gọi một phương thức hoặc thực hiện một hành động nào đó
    }

    //public void PlaySound(string audioClipName)
    //{
    //    AudioClip audioClip = Resources.Load<AudioClip>("Audio/" + audioClipName);
    //    if (audioClip != null)
    //    {
    //        audioSource.PlayOneShot(audioClip);
    //    }
    //}
}