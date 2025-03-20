using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The GameManager orchestrates the main flow of the game by managing waves, shop phases, 
/// and win/lose states. It listens to global events and reacts by spawning waves, opening
/// or closing the shop, and pausing/resuming the game as necessary.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Indicates whether the player has won the current game session.
    /// Marked NonSerialized so it won't be saved or restored automatically.
    /// </summary>
    [NonSerialized] public bool gameWon = false;
    
    /// <summary>
    /// Indicates whether the player has lost the current game session.
    /// Marked NonSerialized so it won't be saved or restored automatically.
    /// </summary>
    [NonSerialized] public bool gameLost = false;
    
    /// <summary>
    /// Determines if the shop should be opened immediately 
    /// when the Level1 scene is loaded.
    /// </summary>
    public bool OpenShopAtBeginning = false;
    
    /// <summary>
    /// Provides a globally accessible instance of GameManager, 
    /// following a Singleton-like pattern.
    /// </summary>
    public static GameManager Instance { get; private set; }

    /// <summary>
    /// Subscribes to relevant events (e.g., timer end, shop open/close, win/lose)
    /// and ensures this object becomes the active GameManager if not already set.
    /// </summary>
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        
        Timer.Instance.OnTimerEnd += GameLoop;
        ShopEventBus.OnShopOpened += GameStateEventBus.PauseGame;
        ShopEventBus.OnShopClosed += GameStateEventBus.ResumeGame;
        
        GameStateEventBus.OnWin += Win;
        GameStateEventBus.OnLose += Lose;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Unsubscribes from all previously subscribed events 
    /// when this GameObject is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        Timer.Instance.OnTimerEnd -= GameLoop;
        ShopEventBus.OnShopOpened -= GameStateEventBus.PauseGame;
        ShopEventBus.OnShopClosed -= GameStateEventBus.ResumeGame;
        
        GameStateEventBus.OnWin -= Win;
        GameStateEventBus.OnLose -= Lose;
        
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Called each time a new scene is loaded. If the scene is Level1, 
    /// it either activates the shop immediately or spawns the first wave
    /// after a short delay, depending on OpenShopAtBeginning.
    /// </summary>
    /// <param name="scene">Information about the loaded scene.</param>
    /// <param name="mode">Specifies how the scene was loaded (e.g., Single or Additive).</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level1")
        {
            if(OpenShopAtBeginning)
            {
                ShopManager.Instance.ActivateShop();
            }
            else
            {
                ShopManager.Instance.DeactivateShop();
                Invoke("SpawnWave", 0.5f);
            }
        }
    }

    /// <summary>
    /// Invokes WaveManager to spawn a new wave of enemies.
    /// </summary>
    private void SpawnWave()
    {
        WaveManager.Instance.SpawnNewWave();
    }

    /// <summary>
    /// Central game loop logic triggered when the timer ends. 
    /// If the shop is open, it closes the shop and spawns a new wave. Otherwise,
    /// it either opens the shop for the next phase or indicates all waves are completed.
    /// </summary>
    public void GameLoop()
    {
        if (ShopManager.Instance.ShopIsOpen)
        {
            ShopManager.Instance.DeactivateShop();
            WaveManager.Instance.SpawnNewWave();
        }
        else
        {
            if (WaveManager.Instance.CurrentWave < WaveManager.Instance.WavesData.GetNumberOfWaves())
            {
                ShopManager.Instance.ActivateShop();
            }
            else
            {
                WaveEventBus.WavesCompleted();
            }
        }
    }

    /// <summary>
    /// Marks the game as won, sets gameWon to true, and pauses the game state.
    /// </summary>
    private void Win()
    {
        gameWon = true;
        GameStateEventBus.PauseGame();
    }

    /// <summary>
    /// Marks the game as lost, sets gameLost to true, and pauses the game state.
    /// </summary>
    private void Lose()
    {
        gameLost = true;
        GameStateEventBus.PauseGame();
    }
}
