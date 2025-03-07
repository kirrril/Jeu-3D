using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer instance;

    [SerializeField] TMP_Text timerText;

    [SerializeField] public float time;

    TimeSpan timeSpan;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        time += Time.deltaTime;

        timerText.text = time.ToString();

        timeSpan = TimeSpan.FromSeconds(time);

        timerText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes,  timeSpan.Seconds);
    }





}
