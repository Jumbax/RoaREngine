using RoaREngine;
using System;
using UnityEngine;

[ExecuteInEditMode]
public class UserCall : MonoBehaviour
{
    public string[] CueNames;
    public RoaRContainer[] Containers;
    public float hSliderValue = 0.0f;
    public int bankIndex;
    public float crossFadeParam;
    
    private void OnGUI()
    {
        hSliderValue = GUI.HorizontalSlider(new Rect(25, 25, 100, 30), hSliderValue, 0f, 1f);
        GUILayout.BeginArea(new Rect(Screen.width / 4 - 100, Screen.height / 4, 200, Screen.height));
        if (GUILayout.Button("Play"))
        {
            RoaRManager.CallPlay(CueNames[0]);
            Debug.Log("Play");
        }
        if (GUILayout.Button("Pause"))
        {
            RoaRManager.CallPause(CueNames[0]);
            Debug.Log("Pause");
        }
        if (GUILayout.Button("Resume"))
        {
            RoaRManager.CallResume(CueNames[0]);
            Debug.Log("Resume");
        }
        if (GUILayout.Button("Stop"))
        {
            RoaRManager.CallStop(CueNames[0]);
            Debug.Log("Stop");
        }
        if (GUILayout.Button("Add Container"))
        {
            RoaRManager.CallAddContainer(Containers[0]);
            Debug.Log("Add Container");
        }
        if (GUILayout.Button("Remove Container"))
        {
            RoaRManager.CallRemoveContainer(Containers[0]);
            Debug.Log("Remove Container");
        }
        if (GUILayout.Button("Get Container"))
        {
            RoaRManager.CallGetContainer(CueNames[0]);
            Debug.Log("Get Container");
        }
        if (GUILayout.Button("Get Containers"))
        {
            RoaRManager.CallGetContainers();
            Debug.Log("Get Containers");
        }
        if (GUILayout.Button("Get Number Containers"))
        {
            RoaRManager.CallGetNumberContainers();
            Debug.Log("Get Number Containers");
        }
        if (GUILayout.Button("Change Sequence Mode"))
        {
            RoaRManager.CallChangeSequenceMode(CueNames[0], AudioSequenceMode.Sequential);
            Debug.Log("Change Sequence Mode");
        }
        if (GUILayout.Button("Get AudioSource"))
        {
            RoaRManager.CallGetAudioSource(CueNames[0]);
            Debug.Log("Get AudioSource");
        }
        if (GUILayout.Button("Get Number AudioSources"))
        {
            RoaRManager.CallGetNumberAudioSources();
            Debug.Log("Get Number AudioSources");
        }
        if (GUILayout.Button("Add Effect"))
        {
            RoaRManager.CallAddEffect(CueNames[0], EffectType.Chorus);
            Debug.Log("Add Effect");
        }
        if (GUILayout.Button("Get AudioSource Effect"))
        {
            //RoaRManager.CallGetAudioSourceEffect(CueNames[0], AudioChorusFilter);
            Debug.Log("Get AudioSource Effect");
        }
        if (GUILayout.Button("Set Bank Index"))
        {
            RoaRManager.CallSetBankIndex(CueNames[0], bankIndex);
            Debug.Log("Set Bank Index");
        }
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(Screen.width / 2 - 100, Screen.height / 4, 200, Screen.height));
        if (GUILayout.Button("Fade"))
        {
            RoaRManager.CallFade(CueNames[0], 3f, 0f);
            //Debug.Log("Fade");
        }
        if (GUILayout.Button("CrossFade By Parameter"))
        {
            RoaRManager.CallCrossFadeByParameter(CueNames, hSliderValue);
            Debug.Log("CrossFade By Parameter");
        }
        if (GUILayout.Button("Add Measure Event"))
        {
            RoaRManager.CallAddMeasureEvent(CueNames[0], MeasureEvent);
            Debug.Log("Add Measure Event");
        }
        if (GUILayout.Button("Remove Measure Event"))
        {
            RoaRManager.CallRemoveMeasureEvent(CueNames[0], MeasureEvent);
            Debug.Log("Remove Measure Event");
        }
        if (GUILayout.Button("Add Timed Event"))
        {
            RoaRManager.CallAddTimedEvent(CueNames[0], TimedEvent);
            Debug.Log("Add Timed Event");
        }
        if (GUILayout.Button("Remove Timed Event"))
        {
            RoaRManager.CallRemoveTimedEvent(CueNames[0], TimedEvent);
            Debug.Log("Add Timed Event");
        }
        if (GUILayout.Button("Add Play Event"))
        {
            RoaRManager.CallAddPlayEvent(CueNames[0], PlayEvent);
            Debug.Log("Add Play Event");
        }
        if (GUILayout.Button("Remove Play Event"))
        {
            RoaRManager.CallRemovePlayEvent(CueNames[0], PlayEvent);
            Debug.Log("Remove Play Event");
        }
        if (GUILayout.Button("Add Pause Event"))
        {
            RoaRManager.CallAddPauseEvent(CueNames[0], PauseEvent);
            Debug.Log("Add Pause Event");
        }
        if (GUILayout.Button("Remove Pause Event"))
        {
            RoaRManager.CallRemovePauseEvent(CueNames[0], PauseEvent);
            Debug.Log("Remove Pause Event");
        }
        if (GUILayout.Button("Add Resume Event"))
        {
            RoaRManager.CallAddResumeEvent(CueNames[0], ResumeEvent);
            Debug.Log("Add Resume Event");
        }
        if (GUILayout.Button("Remove Resume Event"))
        {
            RoaRManager.CallRemoveResumeEvent(CueNames[0], ResumeEvent);
            Debug.Log("Remove Resume Event");
        }
        if (GUILayout.Button("Add Stop Event"))
        {
            RoaRManager.CallAddStopEvent(CueNames[0], StopEvent);
            Debug.Log("Add Stop Event");
        }
        if (GUILayout.Button("Remove Stop Event"))
        {
            RoaRManager.CallRemoveStopEvent(CueNames[0], StopEvent);
            Debug.Log("Remove Stop Event");
        }
        if (GUILayout.Button("Add Finished Event"))
        {
            RoaRManager.CallAddFinishedEvent(CueNames[0], FinishedEvent);
            Debug.Log("Add Finished Event");
        }
        if (GUILayout.Button("Remove Finished Event"))
        {
            RoaRManager.CallRemoveFinishedEvent(CueNames[0], FinishedEvent);
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
