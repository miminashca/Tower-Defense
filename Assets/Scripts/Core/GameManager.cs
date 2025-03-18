using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool gameWon = false;
    public bool gameLost = false;
    public bool OpenShopAtBeginning = false;
    
    private float fixedDeltaTime;
    
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        
        this.fixedDeltaTime = Time.fixedDeltaTime;

        Timer.Instance.OnTimerEnd += GameLoop;
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        EventBus.OnWin += Win;
        EventBus.OnLose += Lose;
    }
    private void OnDestroy()
    {
        Timer.Instance.OnTimerEnd -= GameLoop;
        
        SceneManager.sceneLoaded -= OnSceneLoaded;
        EventBus.OnWin -= Win;
        EventBus.OnLose -= Lose;
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
        EventBus.ResetGame();
        //Debug.Log("Scene Loaded: " + scene.name);
    }

    private void Win()
    {
        gameWon = true;
    }
    private void Lose()
    {
        gameLost = true;
    }
    
    private void Update()
    { 
        // Time.timeScale = 0.5f; 
        // Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
    }
}

