using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float maxTime;
    public float minTime;
    public UIManager UIM;

    private bool timerEnabled = false;
    private float timeRemaining;

    // Start is called before the first frame update
    void Start()
    {
        ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerEnabled)
        {
            timeRemaining -= Time.deltaTime;
            Mathf.Clamp(timeRemaining, minTime, maxTime);
            // Update our UI as well
            UIM.UpdateTimer(timeRemaining);
        }
    }

    public void ResetTimer()
    {
        timeRemaining = maxTime;
    }

    public void SetTimerEnabled(bool value)
    {
        timerEnabled = value;
    }
}
