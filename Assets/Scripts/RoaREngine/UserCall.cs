using RoaREngine;
using UnityEngine;

public class UserCall : MonoBehaviour
{
    private bool Play;
    private bool Stop;
    private bool Pause;
    private bool Resume;
    private bool C;
    private bool X;
    public RoaRManager manager;
    public string CueName;
    public float FadeInTime;
    public float FadeOutTime;

    public float hSliderValue = 0.0F;
    
    private void OnGUI()
    {
        //hSliderValue = GUI.HorizontalSlider(new Rect(25, 25, 100, 30), hSliderValue, 0f, 1f);
        //if (manager.SetProperty(CueName) != null)
        //{
        //    manager.SetProperty(CueName).volume = hSliderValue;
        //}
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            if (!Play)
            {
                manager.Play(CueName);
                Play = true;
            }
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            if (!Stop)
            {
                manager.Stop(CueName, 10f);
                Stop = true;
            }
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            if (!Pause)
            {
                manager.Pause(CueName);
                Pause = true;
            }
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            if (!Resume)
            {
                manager.Resume(CueName);
                Resume = true;
            }
        }
        if (Input.GetKey(KeyCode.F))
        {
            Play = false;
            Stop = false;
            Pause = false;
            Resume = false;
            C = false;
            X = false;
        }
        if (Input.GetKey(KeyCode.X))
        {
            if (!X)
            {
                manager.GetAudioSourceEffect<AudioChorusFilter>(CueName).depth = 1f;
                //manager.GetAudioSource(CueName).volume = 0f;
                X = true;
            }
        }
        if (Input.GetKey(KeyCode.C))
        {
            if (!C)
            {
                manager.AddEffect(CueName, EffectType.Distortion);
                C = true;
            }
        }

    }
}
