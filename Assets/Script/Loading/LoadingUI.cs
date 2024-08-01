using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] private Text loadingCount;
    [SerializeField] private Scrollbar loadingScrollbar;
    private float loadingProgress;
    private bool isComplete;

    public void UpdateUI(float progress)
    {
        loadingProgress = progress;
        if (loadingCount)
        {
            loadingCount.text = $"Loading... {(loadingProgress * 100).ToString("f0")}%";
        }

        if (loadingScrollbar)
        {
            loadingScrollbar.size = loadingProgress;
        }
    }

    public bool IsComplete()
    {
        return isComplete;
    }

    public void SetComplete(bool value)
    {
        isComplete = value;
    }
}