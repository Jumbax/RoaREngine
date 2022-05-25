using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoaREngine;

public class UserCall : MonoBehaviour
{
    private bool Play;
    private bool Stop;
    private bool Pause;
    private bool Resume;
    public RoaRManager manager;
    public string CueName;
    public float FadeTime;

    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            if (!Play)
            {
                manager.Play(CueName, 0f, false, 0f, true);
                Play = true;
            }
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            if (!Stop)
            {
                manager.Stop(CueName);
                Stop = true;
            }
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            if (!Pause)
            {
                manager.Pause(CueName, 1f);
                Pause = true;
            }
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            if (!Resume)
            {
                manager.Resume(CueName, 1f);
                Resume = true;
            }
        }
        if (Input.GetKey(KeyCode.F))
        {
            Play = false;
            Stop = false;
            Pause = false;
            Resume = false;
        }
    }
}
