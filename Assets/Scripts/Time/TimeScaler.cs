using UnityEngine;

public class TimeScaler : MonoBehaviour
{
    public float TimeScale = 1;
    public static TimeScaler Instance { get; private set; }
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        
        ResetTimeScale();
        GameStateEventBus.OnPauseGame += SetZeroTimeScale;
        GameStateEventBus.OnResumeGame += ResetTimeScale;
    }
    private void OnDestroy()
    {
        GameStateEventBus.OnPauseGame -= SetZeroTimeScale;
        GameStateEventBus.OnResumeGame -= ResetTimeScale;
    }

    private void Update()
    {
        if (Time.timeScale != 0f) ResetTimeScale();
    }
    private void SetZeroTimeScale()
    {
        Time.timeScale = 0f; 
    }
    private void ResetTimeScale()
    {
        Time.timeScale = TimeScale; 
    }
}
