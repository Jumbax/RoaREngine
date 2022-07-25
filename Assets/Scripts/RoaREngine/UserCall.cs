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
    public string CueName2;
    public string[] CueNames;
    public float[] params1;
    public float[] params2;
    public float[] params3;
    public float[][] pparams = new float[3][];
    public float hSliderValue = 0.0f;

    public RoaRContainer container;

    private void Start()
    {
        //pparams[0] = params1;
        //pparams[1] = params2;
        //pparams[2] = params3;
        //foreach (var item in CueNames)
        //{
        //    manager.Play(item, delay:5f);
        //}
        
    }

    private void OnGUI()
    {
        hSliderValue = GUI.HorizontalSlider(new Rect(25, 25, 100, 30), hSliderValue, 0f, 1f);
    }

    void Update()
    {
        //manager.CrossFadeByParameter(CueNames, pparams, hSliderValue);
        if (Input.GetKey(KeyCode.Alpha1))
        {
            if (!Play)
            {
                //manager.Play(CueName);
                manager.Play(CueName);
                manager.Play(CueName2, delay: manager.GetAudioSource(CueName).clip.length);
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
