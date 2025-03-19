using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [NonSerialized] public bool gameWon = false;
    [NonSerialized] public bool gameLost = false;
    
    public bool OpenShopAtBeginning = false;
    
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        
        Timer.Instance.OnTimerEnd += GameLoop;
        ShopEventBus.OnShopOpened += GameStateEventBus.PauseGame;
        ShopEventBus.OnShopClosed += GameStateEventBus.ResumeGame;
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        GameStateEventBus.OnWin += Win;
        GameStateEventBus.OnLose += Lose;
    }
    private void OnDestroy()
    {
        Timer.Instance.OnTimerEnd -= GameLoop;
        ShopEventBus.OnShopOpened -= GameStateEventBus.PauseGame;
        ShopEventBus.OnShopClosed -= GameStateEventBus.ResumeGame;


        
        SceneManager.sceneLoaded -= OnSceneLoaded;
        GameStateEventBus.OnWin -= Win;
        GameStateEventBus.OnLose -= Lose;
    }
    private void Start()
    { 
        if(OpenShopAtBeginning) ShopManager.Instance.ActivateShop();
        else
        {
            ShopManager.Instance.DeactivateShop();
            WaveManager.Instance.SpawnNewWave();
        }
    }
    public void GameLoop()
    {
        if (ShopManager.Instance.ShopIsOpen)
        {
            ShopManager.Instance.DeactivateShop();
            WaveManager.Instance.SpawnNewWave();
        }
        else
        {
            if (WaveManager.Instance.currentWave < WaveManager.Instance.wavesData.GetNumberOfWaves())
            {
                ShopManager.Instance.ActivateShop();
            }
            else
            {
                WaveEventBus.WavesCompleted();
            }
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameStateEventBus.ResetGame();
        //Debug.Log("Scene Loaded: " + scene.name);
    }

    private void Win()
    {
        gameWon = true;
        GameStateEventBus.PauseGame();
    }
    private void Lose()
    {
        gameLost = true;
        GameStateEventBus.PauseGame();
    }
}

