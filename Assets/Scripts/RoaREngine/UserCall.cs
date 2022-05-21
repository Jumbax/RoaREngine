using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoaREngine;

public class UserCall : MonoBehaviour
{
    private bool Play;
    private bool Stop;
    public RoaRManager manager;
    public string CueName;

    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            if (!Play)
            {
                manager.PlayWithFade(CueName, 5f);
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
        if (Input.GetKey(KeyCode.F))
        {
            Play = false;
            Stop = false;
        }
    }
}
