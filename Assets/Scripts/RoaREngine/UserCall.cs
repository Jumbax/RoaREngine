using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoaREngine;

public class UserCall : MonoBehaviour
{
    private bool Play;
    public RoaRManager manager;

    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            if (!Play)
            {
                manager.Play("RoaRTest");
                Play = true;
            }
        }
        if (Input.GetKey(KeyCode.F))
        {
            Play = false;
        }
    }
}
