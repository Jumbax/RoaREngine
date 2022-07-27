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
    public string[] CueNames;
    public float hSliderValue = 0.0f;

    private void OnGUI()
    {
        hSliderValue = GUI.HorizontalSlider(new Rect(25, 25, 100, 30), hSliderValue, 0f, 1f);
    }

    private void Start()
    {
        manager.Play(CueNames[0]);
        manager.Play(CueNames[1], delay: manager.GetAudioSource(CueNames[0]).clip.length);
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            if (!Play)
            {
                Play = true;
            }
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            if (!Stop)
            {
                manager.Stop(CueNames[1]);
                Stop = true;
            }
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            if (!Pause)
            {
                manager.Pause(CueNames[0]);
                Pause = true;
            }
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            if (!Resume)
            {
                manager.Resume(CueNames[0]);
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
                manager.GetAudioSourceEffect<AudioChorusFilter>(CueNames[0]).depth = 1f;
                //manager.GetAudioSource(CueName).volume = 0f;
                X = true;
            }
        }
        if (Input.GetKey(KeyCode.C))
        {
            if (!C)
            {
                manager.AddEffect(CueNames[0], EffectType.Distortion);
                C = true;
            }
        }

    }
}
