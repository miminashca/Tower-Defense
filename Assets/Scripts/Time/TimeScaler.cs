using System;
using UnityEngine;

public class TimeScaler : MonoBehaviour
{
    public float TimeScale = 1;
    [NonSerialized] public float TimeScaleForGameObjects = 1;
    public static TimeScaler Instance { get; private set; }
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        
        GameStateEventBus.OnPauseGame += SetZeroTimeScale;
        GameStateEventBus.OnResumeGame += ResetTimeScale;
        
        GameStateEventBus.OnReloadManagers += Reload;
    }
    private void Reload()
    {
        ResetTimeScale();
    }
    private void OnDestroy()
    {
        GameStateEventBus.OnPauseGame -= SetZeroTimeScale;
        GameStateEventBus.OnResumeGame -= ResetTimeScale;
        
        GameStateEventBus.OnReloadManagers -= Reload;
    }

    private void Update()
    {
        Time.timeScale = TimeScale;
        
        if (TimeScaleForGameObjects != 0f) ResetTimeScale();
    }
    private void SetZeroTimeScale()
    {
        Debug.Log("set 0 time");
        TimeScaleForGameObjects = 0f; 
    }
    private void ResetTimeScale()
    {
        TimeScaleForGameObjects = TimeScale; 
    }
}
