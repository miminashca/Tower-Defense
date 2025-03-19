using System;
using System.Collections;
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
        }
        
        Timer.Instance.OnTimerEnd += GameLoop;
        ShopEventBus.OnShopOpened += GameStateEventBus.PauseGame;
        ShopEventBus.OnShopClosed += GameStateEventBus.ResumeGame;
        
        GameStateEventBus.OnWin += Win;
        GameStateEventBus.OnLose += Lose;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDestroy()
    {
        Timer.Instance.OnTimerEnd -= GameLoop;
        ShopEventBus.OnShopOpened -= GameStateEventBus.PauseGame;
        ShopEventBus.OnShopClosed -= GameStateEventBus.ResumeGame;
        
        GameStateEventBus.OnWin -= Win;
        GameStateEventBus.OnLose -= Lose;
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level1")
        {
            if(OpenShopAtBeginning) ShopManager.Instance.ActivateShop();
            else
            {
                ShopManager.Instance.DeactivateShop();
                Invoke("SpawnWave", 0.5f);
            }
        }
    }

    private void SpawnWave()
    {
        WaveManager.Instance.SpawnNewWave();
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

