using System;

public static class EventBus
{
    public static event Action OnResetGame;
    public static void ResetGame()
    {
        OnResetGame?.Invoke();
    }
    
    public static event Action OnPauseGame;
    public static void PauseGame()
    {
        OnPauseGame?.Invoke();
    }
    public static event Action OnResumeGame;
    public static void ResumeGame()
    {
        OnResumeGame?.Invoke();
    }
    
    public static event Action OnLose;
    public static event Action OnWin;
    public static void Lose()
    {
        OnLose?.Invoke();
    }
    
    public static void Win()
    {
        OnWin?.Invoke();
    }
    
}