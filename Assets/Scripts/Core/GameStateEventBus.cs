using System;

public static class GameStateEventBus
{
    public static event Action OnRestart;
    public static void Restart()
    {
        OnRestart?.Invoke();
    }
    
    public static event Action OnReloadManagers;
    public static void ReloadManagers()
    {
        OnReloadManagers?.Invoke();
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
    
    public static event Action OnGameEnd;
    public static event Action OnLose;
    public static void Lose()
    {
        OnLose?.Invoke();
        OnGameEnd?.Invoke();
    }
    
    public static event Action OnWin;
    public static void Win()
    {
        OnWin?.Invoke();
        OnGameEnd?.Invoke();
    }
    
}