using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public float TValue = 60;
    public TextMeshProUGUI TText;

    void Update()
    {
        if (TValue > 0)
        {
            TValue -= Time.deltaTime;
        }
        else
        {
            TValue = 0;
        }
        DisplayT(TValue);
    }

    void DisplayT(float TDisplayed)
    {
        if (TDisplayed < 0)
        {
            TDisplayed = 0;
        }
        float mins = Mathf.FloorToInt(TDisplayed / 60);
        float secs = Mathf.FloorToInt(TDisplayed % 60);
        TText.text = string.Format("{0:00}:{1:00}", mins, secs);
    }
}