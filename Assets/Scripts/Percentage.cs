using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
//using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class Percentage : MonoBehaviour
{
    private Timer timer;
    public TextMeshProUGUI TText;

    void Start()
    {
        timer = GetComponent<Timer>();
    }

    void Update()
    {
        float TValue = timer.TValue;
        if (TValue == 0.0f)
        { 
            float percentage = ColourChanger.counter;
            percentage = (percentage / 111) *100;
            TText.text = percentage.ToString("F2")+ "%";
        }
    }
}
