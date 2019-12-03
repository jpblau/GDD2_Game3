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
            IsTimeOver();
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

    /// <summary>
    /// Checks to see if the timer is currently less than 0 seconds left. If so, 
    /// let's stop counting down, reset our timer, and tell the UIM to present the score screen
    /// </summary>
    public void IsTimeOver()
    {
        // First, let's check to see if the timer has hit 0
        if (timeRemaining <= 0.0f)
        {
            SetTimerEnabled(false);
            ResetTimer();
            UIM.GameOver();
        }
    }
}
