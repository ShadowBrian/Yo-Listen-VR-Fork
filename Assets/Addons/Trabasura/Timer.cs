using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{

    float startTime;
    float endTime;
    float timeIncrement;

    bool autoIncrement;

    public Timer(float timeIncrement)
    {
        this.timeIncrement = timeIncrement;
        if(TimerManager.gameActive)
        {
            ActivateTimer();
        }
        else{
            TimerManager.bakeTimers.Add(this);
        }
    }

    public float GetTimeRemaining(){
        return endTime-Time.time;
    }

    public void ActivateTimer()
    {
        startTime = Time.time;
        endTime = startTime + timeIncrement;
    }

    public Timer(float timeIncrement, bool autoIncrement)
    {
        this.timeIncrement = timeIncrement;
        if(TimerManager.gameActive)
        {
            ActivateTimer();
        }
        else{
            TimerManager.bakeTimers.Add(this);
        }
        this.autoIncrement = autoIncrement;
    }

    public void ChangeTimer(float change)
    {
        var diff = startTime - change;
        startTime = change;
        endTime += diff;
    }


    public bool Finished()
    {
        if(Time.time > endTime)
        {
            if(autoIncrement)
            {
                RestartTimer();
            }
            return true;
        }
        return false;
        
    }

    public void RestartTimer()
    {
        startTime = Time.time;
        endTime = startTime + timeIncrement;
    }
}
