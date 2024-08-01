using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    private AsyncOperation asyncs;
    public UnityEvent<float> OnLoading;
    private LoadingUI loadingUI;

    private void Start()
    {
        loadingUI = GetComponent<LoadingUI>();

        StartCoroutine(LoadSceneCoroutine());
    }

    private IEnumerator LoadSceneCoroutine()
    {
        asyncs = SceneManager.LoadSceneAsync("DataHolder", LoadSceneMode.Additive);
        asyncs.allowSceneActivation = false;

        while (!loadingUI.IsComplete())
        {
            loadingUI.UpdateUI(asyncs.progress);

            if (asyncs.progress >= 0.2f && asyncs.progress < 1f)
            {
                asyncs.allowSceneActivation = true;
            }

            if (asyncs.progress >= 1f && asyncs.allowSceneActivation)
            {
                loadingUI.SetComplete(true);
                yield return new WaitForSeconds(2f);
                SceneManager.LoadScene("MainScene");
            }

            yield return null;
        }
    }
}