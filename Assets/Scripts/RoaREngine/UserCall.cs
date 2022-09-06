using RoaREngine;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class UserCall : MonoBehaviour
{
    public string[] CueNames;
    public RoarContainerSO[] Containers;
    public float hSliderValue = 0.0f;
    public int bankIndex;
    public float crossFadeParam;

    private void OnGUI()
    {
        hSliderValue = GUI.HorizontalSlider(new Rect(25, 25, 100, 30), hSliderValue, 0f, 1f);
        GUILayout.BeginArea(new Rect(Screen.width / 4 - 100, Screen.height / 4, 200, Screen.height));
        if (GUILayout.Button("Play"))
        {
            RoarManager.CallPlay("Music", false);
            RoarManager.CallPlay("Music1", false);
        }
        if (GUILayout.Button("ChangeScene"))
        {
            SceneManager.LoadScene(1);
        }
        if (GUILayout.Button("Pause"))
        {
            RoarManager.CallPause("Music");
            Debug.Log("Pause");
        }
        if (GUILayout.Button("Resume"))
        {
            RoarManager.CallResume("Music");
            Debug.Log("Resume");
        }
        if (GUILayout.Button("Stop"))
        {
            RoarManager.CallStop("Music");
            Debug.Log("Stop");
        }
        if (GUILayout.Button("StopAll"))
        {
            RoarManager.CallStopAll();
            Debug.Log("StopAll");
        }
        if (GUILayout.Button("PauseAll"))
        {
            RoarManager.CallPauseAll();
            Debug.Log("PauseAll");
        }
        if (GUILayout.Button("ResumeAll"))
        {
            RoarManager.CallResumeAll();
            Debug.Log("ResumeAll");
        }
        if (GUILayout.Button("Add Container"))
        {
            RoarManager.CallAddContainer(Containers[0]);
            Debug.Log("Add Container");
        }
        if (GUILayout.Button("Remove Container"))
        {
            RoarManager.CallRemoveContainer(Containers[0]);
            Debug.Log("Remove Container");
        }
        if (GUILayout.Button("Get Container"))
        {
            RoarContainerSO container = RoarManager.CallGetContainer(CueNames[0]);
            Debug.Log("Get Container" + container);
        }
        if (GUILayout.Button("Get Containers"))
        {
            RoarManager.CallGetContainers();
            Debug.Log("Get Containers");
        }
        if (GUILayout.Button("Get Number Containers"))
        {
            RoarManager.CallGetNumberContainers();
            Debug.Log("Get Number Containers");
        }
        if (GUILayout.Button("Change Sequence Mode"))
        {
            RoarManager.CallChangeSequenceMode(CueNames[0], AudioSequenceMode.Sequential);
            Debug.Log("Change Sequence Mode");
        }
        if (GUILayout.Button("Get AudioSource"))
        {
            RoarManager.CallGetAudioSource(CueNames[0]);
            Debug.Log("Get AudioSource");
        }
        if (GUILayout.Button("Get Number AudioSources"))
        {
            RoarManager.CallGetNumberAudioSources();
            Debug.Log("Get Number AudioSources");
        }
        if (GUILayout.Button("Add Effect"))
        {
            RoarManager.CallAddEffect(CueNames[0], EffectType.Chorus);
            Debug.Log("Add Effect");
        }
        if (GUILayout.Button("Get AudioSource Effect"))
        {
            //RoaRManager.CallGetAudioSourceEffect(CueNames[0], AudioChorusFilter);
            //manager.GetAudioSourceEffect<AudioDistortionFilter>("Music");
            Debug.Log("Get AudioSource Effect");
        }
        if (GUILayout.Button("Set Bank Index"))
        {
            RoarManager.CallSetBankIndex(CueNames[0], bankIndex);
            Debug.Log("Set Bank Index");
        }
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(Screen.width / 2 - 100, Screen.height / 4, 200, Screen.height));
        if (GUILayout.Button("Fade"))
        {
            RoarManager.CallFade(CueNames[0], 3f, 0f);
            Debug.Log("Fade");
        }
        if (GUILayout.Button("CrossFade By Parameter"))
        {
            RoarManager.CallCrossFadeByParameter(CueNames, hSliderValue);
            Debug.Log("CrossFade By Parameter");
        }
        if (GUILayout.Button("Add Measure Event"))
        {
            RoarManager.CallAddMeasureEvent(CueNames[0], MeasureEvent);
            Debug.Log("Add Measure Event");
        }
        if (GUILayout.Button("Remove Measure Event"))
        {
            RoarManager.CallRemoveMeasureEvent(CueNames[0], MeasureEvent);
            Debug.Log("Remove Measure Event");
        }
        if (GUILayout.Button("Add Timed Event"))
        {
            RoarManager.CallAddTimedEvent(CueNames[0], TimedEvent);
            Debug.Log("Add Timed Event");
        }
        if (GUILayout.Button("Remove Timed Event"))
        {
            RoarManager.CallRemoveTimedEvent(CueNames[0], TimedEvent);
            Debug.Log("Remove Timed Event");
        }
        if (GUILayout.Button("Add Play Event"))
        {
            RoarManager.CallAddPlayEvent(CueNames[0], PlayEvent);
            Debug.Log("Add Play Event");
        }
        if (GUILayout.Button("Remove Play Event"))
        {
            RoarManager.CallRemovePlayEvent(CueNames[0], PlayEvent);
            Debug.Log("Remove Play Event");
        }
        if (GUILayout.Button("Add Pause Event"))
        {
            RoarManager.CallAddPauseEvent(CueNames[0], PauseEvent);
            Debug.Log("Add Pause Event");
        }
        if (GUILayout.Button("Remove Pause Event"))
        {
            RoarManager.CallRemovePauseEvent(CueNames[0], PauseEvent);
            Debug.Log("Remove Pause Event");
        }
        if (GUILayout.Button("Add Resume Event"))
        {
            RoarManager.CallAddResumeEvent(CueNames[0], ResumeEvent);
            Debug.Log("Add Resume Event");
        }
        if (GUILayout.Button("Remove Resume Event"))
        {
            RoarManager.CallRemoveResumeEvent(CueNames[0], ResumeEvent);
            Debug.Log("Remove Resume Event");
        }
        if (GUILayout.Button("Add Stop Event"))
        {
            RoarManager.CallAddStopEvent(CueNames[0], StopEvent);
            Debug.Log("Add Stop Event");
        }
        if (GUILayout.Button("Remove Stop Event"))
        {
            RoarManager.CallRemoveStopEvent(CueNames[0], StopEvent);
            Debug.Log("Remove Stop Event");
        }
        if (GUILayout.Button("Add Finished Event"))
        {
            RoarManager.CallAddFinishedEvent(CueNames[0], FinishedEvent);
            Debug.Log("Add Finished Event");
        }
        if (GUILayout.Button("Remove Finished Event"))
        {
            RoarManager.CallRemoveFinishedEvent(CueNames[0], FinishedEvent);
            Debug.Log("Remove Finished Event");
        }
        GUILayout.EndArea();

    }

    private void MeasureEvent()
    {
        Debug.Log("Measure Event");
    }

    private void TimedEvent()
    {
        Debug.Log("Timed Event");
    }

    void FinishedEvent()
    {
        Debug.Log("Finished Event");
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
