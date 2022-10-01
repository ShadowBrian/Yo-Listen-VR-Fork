using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{

    public static bool gameActive = false;
    public static List<Timer> bakeTimers = new List<Timer>();
    void Awake()
    {
        gameActive = true;
        foreach(var timer in bakeTimers)
        {
            timer.ActivateTimer();
        }
    }
}
