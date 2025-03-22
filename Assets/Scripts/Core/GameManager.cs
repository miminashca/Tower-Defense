using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The GameManager orchestrates the main flow of the game by managing waves, shop phases, 
/// and win/lose states. It listens to global events and reacts by spawning waves, opening
/// or closing the shop, and pausing/resuming the game as necessary.
/// </summary>
public class GameManager : Manager<GameManager>
{
    /// <summary>
    /// Indicates whether the player has won the current game session.
    /// </summary>
    [NonSerialized] public bool GameWon = false;
    
    /// <summary>
    /// Indicates whether the player has lost the current game session.
    /// </summary>
    [NonSerialized] public bool GameLost = false;
    
    /// <summary>
    /// Determines if the shop should be opened immediately 
    /// when the Level1 scene is loaded.
    /// </summary>
    public bool OpenShopAtBeginning = false;

    /// <summary>
    /// Subscribes to relevant events (e.g., timer end, shop open/close, win/lose).
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        
        Timer.Instance.OnTimerEnd += GameLoop;
        ShopEventBus.OnShopOpened += GameStateEventBus.PauseGame;
        ShopEventBus.OnShopClosed += GameStateEventBus.ResumeGame;
        
        GameStateEventBus.OnWin += Win;
        GameStateEventBus.OnLose += Lose;
    }

    /// <summary>
    /// Unsubscribes from all previously subscribed events 
    /// when this GameObject is destroyed.
    /// </summary>
    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        Timer.Instance.OnTimerEnd -= GameLoop;
        ShopEventBus.OnShopOpened -= GameStateEventBus.PauseGame;
        ShopEventBus.OnShopClosed -= GameStateEventBus.ResumeGame;
        
        GameStateEventBus.OnWin -= Win;
        GameStateEventBus.OnLose -= Lose;
    }

    protected override void Reload() {}

    /// <summary>
    /// Called each time a new scene is loaded.
    /// it either activates the shop immediately or spawns the first wave
    /// after a short delay, depending on OpenShopAtBeginning.
    /// </summary>
    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        base.OnSceneLoaded(scene, mode);

        if(OpenShopAtBeginning)
        { 
            ServiceLocator.Get<ShopManager>().ActivateShop();
        }
        else
        {
            ServiceLocator.Get<ShopManager>().DeactivateShop();
            Invoke("SpawnWave", 0.5f);
        }
    }

    /// <summary>
    /// Invokes WaveManager to spawn a new wave of enemies.
    /// </summary>
    private void SpawnWave()
    {
        ServiceLocator.Get<WaveManager>().SpawnNewWave();
    }

    /// <summary>
    /// Central game loop logic triggered when the timer ends. 
    /// If the shop is open, it closes the shop and spawns a new wave. Otherwise,
    /// it either opens the shop for the next phase or indicates all waves are completed.
    /// </summary>
    public void GameLoop()
    {
        if (ServiceLocator.Get<ShopManager>().ShopIsOpen)
        {
            ServiceLocator.Get<ShopManager>().DeactivateShop();
            ServiceLocator.Get<WaveManager>().SpawnNewWave();
        }
        else
        {
            if (ServiceLocator.Get<WaveManager>().CurrentWave < ServiceLocator.Get<WaveManager>().WavesData.GetNumberOfWaves())
            {
                ServiceLocator.Get<ShopManager>().ActivateShop();
            }
            else
            {
                WaveEventBus.WavesCompleted();
            }
        }
    }

    /// <summary>
    /// Marks the game as won, sets GameWon to true, and pauses the game state.
    /// </summary>
    private void Win()
    {
        GameWon = true;
        GameStateEventBus.PauseGame();
    }

    /// <summary>
    /// Marks the game as lost, sets GameLost to true, and pauses the game state.
    /// </summary>
    private void Lose()
    {
        GameLost = true;
        GameStateEventBus.PauseGame();
    }
}
