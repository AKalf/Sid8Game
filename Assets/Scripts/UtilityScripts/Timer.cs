using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float timer = 0f;
    private bool countTime = false;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (countTime != false)
        {
            timer = timer + Time.deltaTime;
        }
    }


    public void StartTimer()
    {
        countTime = true;

    }

    public void StopAndReset()
    {
        countTime = false;
        timer = 0f;
    }

    public float GetTime()
    {
        return timer;
    }

    public void SetTime(float number)
    {
        timer = number;

    }

}
