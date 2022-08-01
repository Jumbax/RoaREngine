using RoaREngine;
using UnityEngine;

[ExecuteInEditMode]
public class UserCall : MonoBehaviour
{
    public string[] CueNames;
    public RoaRContainer[] Containers;
    public float hSliderValue = 0.0f;

    private void OnGUI()
    {
        hSliderValue = GUI.HorizontalSlider(new Rect(25, 25, 100, 30), hSliderValue, 0f, 1f);
        GUILayout.BeginArea(new Rect(Screen.width / 4 - 100, Screen.height / 4, 200, Screen.height));
        if (GUILayout.Button("Play"))
        {
            RoaREventChannel.Play?.Invoke(CueNames[0], true);
            Debug.Log("Play");
        }
        if (GUILayout.Button("Pause"))
        {
            RoaREventChannel.Pause?.Invoke(CueNames[0]);
            Debug.Log("Pause");
        }
        if (GUILayout.Button("Resume"))
        {
            RoaREventChannel.Resume?.Invoke(CueNames[0]);
            Debug.Log("Resume");
        }
        if (GUILayout.Button("Stop"))
        {
            RoaREventChannel.Stop?.Invoke(CueNames[0]);
            Debug.Log("Stop");
        }
        if (GUILayout.Button("Add Container"))
        {
            RoaREventChannel.AddContainer?.Invoke(Containers[0]);
            Debug.Log("Add Container");
        }
        if (GUILayout.Button("Remove Container"))
        {
            RoaREventChannel.RemoveContainer?.Invoke(Containers[0]);
            Debug.Log("Remove Container");
        }
        if (GUILayout.Button("Get Container"))
        {
            RoaREventChannel.GetContainer?.Invoke(CueNames[0]);
            Debug.Log("Get Container");
        }
        if (GUILayout.Button("Get Containers"))
        {
            RoaREventChannel.GetContainers?.Invoke();
            Debug.Log("Get Containers");
        }
        if (GUILayout.Button("Get Number Containers"))
        {
            RoaREventChannel.GetNumberContainers?.Invoke();
            Debug.Log("Get Number Containers");
        }
        if (GUILayout.Button("Set Containers"))
        {
            //RoaREventChannel.SetContainer?.Invoke(CueNames[0]);
            Debug.Log("Set Container");
        }
        if (GUILayout.Button("Change Sequence Mode"))
        {
            Debug.Log("test");
        }
        if (GUILayout.Button("Get AudioSource"))
        {
            Debug.Log("test");
        }
        if (GUILayout.Button("Get Number AudioSources"))
        {
            Debug.Log("test");
        }
        if (GUILayout.Button("Add Effect"))
        {
            Debug.Log("test");
        }
        if (GUILayout.Button("Get AudioSource Effect"))
        {
            Debug.Log("test");
        }
        if (GUILayout.Button("Set Bank Index"))
        {
            Debug.Log("test");
        }
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(Screen.width / 2 - 100, Screen.height / 4, 200, Screen.height));
        if (GUILayout.Button("Fade"))
        {
            Debug.Log("test");
        }
        if (GUILayout.Button("CrossFade By Parameter"))
        {
            Debug.Log("test");
        }
        if (GUILayout.Button("Add Measure Event"))
        {
            Debug.Log("test");
        }
        if (GUILayout.Button("Remove Measure Event"))
        {
            Debug.Log("test");
        }
        if (GUILayout.Button("Add Timed Event"))
        {
            Debug.Log("test");
        }
        if (GUILayout.Button("Remove Timed Event"))
        {
            Debug.Log("test");
        }
        if (GUILayout.Button("Add Play Event"))
        {
            Debug.Log("test");
        }
        if (GUILayout.Button("Remove Play Event"))
        {
            Debug.Log("test");
        }
        if (GUILayout.Button("Add Pause Event"))
        {
            Debug.Log("test");
        }
        if (GUILayout.Button("Remove Pause Event"))
        {
            Debug.Log("test");
        }
        if (GUILayout.Button("Add Resume Event"))
        {
            Debug.Log("test");
        }
        if (GUILayout.Button("Remove Resume Event"))
        {
            Debug.Log("test");
        }
        if (GUILayout.Button("Add Stop Event"))
        {
            Debug.Log("test");
        }
        if (GUILayout.Button("Remove Stop Event"))
        {
            Debug.Log("test");
        }
        if (GUILayout.Button("Add Finished Event"))
        {
            Debug.Log("test");
        }
        if (GUILayout.Button("Remove Finished Event"))
        {
            Debug.Log("test");
        }
        GUILayout.EndArea();

    }


    void FinishEvent()
    {
        Debug.Log("Finish  Event");
    }

    void PlayEvent()
    {
        Debug.Log("Play Event");
    }

    void PauseEvent()
    {
        Debug.Log("Pause Event");
    }

    void ResumeEvent()
    {
        Debug.Log("Resume Event");
    }

    void StopEvent()
    {
        Debug.Log("Stop Event");
    }
}
