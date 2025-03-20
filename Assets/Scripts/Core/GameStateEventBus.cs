using System;

/// <summary>
/// The GameStateEventBus class defines global events for restarting the game, 
/// reloading managers, pausing/resuming gameplay, and handling the end of a game session (win/lose).
/// Other components subscribe to these events to react accordingly without direct inter-dependencies.
/// </summary>
public static class GameStateEventBus
{
    /// <summary>
    /// Fired when a full restart of the game is requested (e.g., returning to initial state).
    /// </summary>
    public static event Action OnRestart;
    
    /// <summary>
    /// Invokes the OnRestart event, signaling that the game should fully restart.
    /// </summary>
    public static void Restart()
    {
        OnRestart?.Invoke();
    }
    
    /// <summary>
    /// Fired when managers should be reset or reloaded (e.g., wave counters, currency, etc.).
    /// </summary>
    public static event Action OnReloadManagers;

    /// <summary>
    /// Invokes the OnReloadManagers event, requesting all managers to re-initialize or reset.
    /// </summary>
    public static void ReloadManagers()
    {
        OnReloadManagers?.Invoke();
    }
    
    /// <summary>
    /// Fired when the game should be paused (e.g., disabling movement or time progression).
    /// </summary>
    public static event Action OnPauseGame;
    
    /// <summary>
    /// Invokes the OnPauseGame event, signaling a global pause.
    /// </summary>
    public static void PauseGame()
    {
        OnPauseGame?.Invoke();
    }
    
    /// <summary>
    /// Fired when the game should resume from a paused state.
    /// </summary>
    public static event Action OnResumeGame;

    /// <summary>
    /// Invokes the OnResumeGame event, indicating gameplay should continue.
    /// </summary>
    public static void ResumeGame()
    {
        OnResumeGame?.Invoke();
    }
    
    /// <summary>
    /// Fired when the game ends, regardless of win or lose outcome.
    /// </summary>
    public static event Action OnGameEnd;

    /// <summary>
    /// Fired when the game is lost.
    /// </summary>
    public static event Action OnLose;
    
    /// <summary>
    /// Invokes both the OnLose event (signaling a lost game) and OnGameEnd event (signaling game completion).
    /// </summary>
    public static void Lose()
    {
        OnLose?.Invoke();
        OnGameEnd?.Invoke();
    }
    
    /// <summary>
    /// Fired when the game is won.
    /// </summary>
    public static event Action OnWin;

    /// <summary>
    /// Invokes both the OnWin event (signaling a won game) and OnGameEnd event (signaling game completion).
    /// </summary>
    public static void Win()
    {
        OnWin?.Invoke();
        OnGameEnd?.Invoke();
    }
}
