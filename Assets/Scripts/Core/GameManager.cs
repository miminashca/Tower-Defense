using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static TileFloor tileFloor { get; private set; }
    
    private int moneyEarned = 30;
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
        //EventBus.OnShopClosed += WaveManager.Instance.SpawnNewWave;
        
        SceneManager.sceneLoaded += OnSceneLoaded;
        EventBus.OnMoneySpent += SpendMoney;
        EventBus.OnMoneyEarned += AddMoney;
        EventBus.OnWin += Win;
        EventBus.OnLose += Lose;
    }
    private void OnDestroy()
    {
        Timer.Instance.OnTimerEnd -= GameLoop;
        //EventBus.OnShopClosed -= WaveManager.Instance.SpawnNewWave;
        
        SceneManager.sceneLoaded -= OnSceneLoaded;
        EventBus.OnMoneySpent -= SpendMoney;
        EventBus.OnMoneyEarned -= AddMoney;
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
        Debug.Log("enter game loop");
        
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
        //Debug.Log("Scene Loaded: " + scene.name);
        tileFloor = FindFirstObjectByType<TileFloor>(FindObjectsInactive.Include);
    }
    
    private void AddMoney(int amount)
    {
        moneyEarned += amount;
    }
    public int GetMoney()
    {
        return moneyEarned;
    }
    private void SpendMoney(int amount)
    {
        if(amount>GetMoney()) return;
        moneyEarned -= amount;
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

