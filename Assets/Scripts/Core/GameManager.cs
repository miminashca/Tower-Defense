using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static TileFloor tileFloor { get; private set; }
    public static Timer timer { get; private set; }
    
    private int moneyEarned = 30;
    public bool gameWon = false;
    public bool gameLost = false;
    
    private float fixedDeltaTime;
    
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        this.fixedDeltaTime = Time.fixedDeltaTime;
        
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        
        if (!timer) timer = GetComponent<Timer>();
        
        if (!timer) Debug.Log("No Timer in Game manager!!!");

        SceneManager.sceneLoaded += OnSceneLoaded;
        EventBus.OnMoneySpent += SpendMoney;
        EventBus.OnMoneyEarned += AddMoney;
        EventBus.OnWin += Win;
        EventBus.OnLose += Lose;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        EventBus.OnMoneySpent -= SpendMoney;
        EventBus.OnMoneyEarned -= AddMoney;
        EventBus.OnWin -= Win;
        EventBus.OnLose -= Lose;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);
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

