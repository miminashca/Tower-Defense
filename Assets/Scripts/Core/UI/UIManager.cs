using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The UIManager class handles displaying and updating various user interface elements,
/// such as wave information, money count, countdown timer, enemy count at the target,
/// and final game state messages (win/lose).
/// </summary>
public class UIManager : MonoBehaviour
{
    /// <summary>
    /// Text component displaying the current wave number.
    /// </summary>
    [SerializeField] private TextMeshProUGUI waveText;
    
    /// <summary>
    /// Text component showing the player's current money amount.
    /// </summary>
    [SerializeField] private TextMeshProUGUI moneyText;
    
    /// <summary>
    /// Text component representing the remaining time, updated each frame.
    /// </summary>
    [SerializeField] private TextMeshProUGUI timerText;
    
    /// <summary>
    /// Text component displaying how many enemies have reached the goal
    /// compared to the allowed maximum.
    /// </summary>
    [SerializeField] private TextMeshProUGUI targetEnemyCounterText;
    
    /// <summary>
    /// Text component for the final game state message ("YOU LOST!" or "YOU WON!").
    /// </summary>
    [SerializeField] private TextMeshProUGUI finalGameStateMessage;
    
    /// <summary>
    /// A static reference to this UIManager instance, ensuring a single instance
    /// can be accessed project-wide.
    /// </summary>
    public static UIManager Instance { get; private set; }

    /// <summary>
    /// Sets up the UIManager singleton, subscribes to necessary events, 
    /// and ensures it persists across scene loads unless destroyed by a duplicate instance.
    /// </summary>
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        WaveEventBus.OnWaveStart += UpdateWaveText;
        EconomyEventBus.OnMoneyAmountChange += UpdateMoneyText;
        EnemyEventBus.OnUpdateEnemyCountAtTarget += UpdateEnemyCountText;
        
        GameStateEventBus.OnLose += ShowLoseGameState;
        GameStateEventBus.OnWin += ShowWinGameState;
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Called when a new scene is fully loaded. If the loaded scene is "Level1",
    /// hides the final game state panel and updates the UI elements to reflect 
    /// current wave, money, and enemy count.
    /// </summary>
    /// <param name="scene">Information about the loaded scene.</param>
    /// <param name="mode">Specifies how the scene was loaded (e.g., Single or Additive).</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level1")
        {
            finalGameStateMessage.gameObject.transform.parent.gameObject.SetActive(false);

            UpdateWaveText(WaveManager.Instance.CurrentWave);
            UpdateMoneyText();
            UpdateEnemyCountText();
        }
    }

    /// <summary>
    /// Unity's standard Update callback. Called once per frame.
    /// Continually updates the timer text on the UI.
    /// </summary>
    private void Update()
    { 
        UpdateTimer();
    }

    /// <summary>
    /// Updates the displayed money amount by fetching current value from the EconomyManager.
    /// </summary>
    private void UpdateMoneyText()
    {
        moneyText.text = EconomyManager.Instance.GetMoney().ToString();
    }

    /// <summary>
    /// Updates the timer text in mm:ss format, turning red when only a few seconds remain.
    /// </summary>
    private void UpdateTimer()
    {
        string seconds;
        if(Timer.Instance.GetTimeLeft() >= 10) 
            seconds = Timer.Instance.GetTimeLeft().ToString();
        else 
            seconds = "0" + Timer.Instance.GetTimeLeft();
        
        timerText.text = "00:" + seconds;
        
        if(Timer.Instance.GetTimeLeft() <= 3) 
            timerText.color = Color.red;
        else
            timerText.color = Color.black;
    }
    
    /// <summary>
    /// Updates the wave text to show the current wave number out of the total.
    /// </summary>
    /// <param name="waveNum">The current wave index/number.</param>
    private void UpdateWaveText(int waveNum)
    {
        waveText.text = waveNum + "/" + WaveManager.Instance.WavesData.GetNumberOfWaves();
    }
    
    /// <summary>
    /// Updates the text showing how many enemies have reached the goal versus the allowed maximum.
    /// Caps the displayed number if it exceeds the maximum.
    /// </summary>
    private void UpdateEnemyCountText()
    {
        int x = EnemyManager.Instance.GetEnemiesAtGoal();
        int y = EnemyManager.Instance.GetMaxEnemiesAtGoalAllowed();
        if (x > y) x = y;
        
        targetEnemyCounterText.text = x + "/" + y;
    }

    /// <summary>
    /// Reveals a red "YOU LOST!" game state message on the screen.
    /// </summary>
    private void ShowLoseGameState()
    {
        finalGameStateMessage.gameObject.transform.parent.gameObject.SetActive(true);
        finalGameStateMessage.text = "YOU LOST!";
        finalGameStateMessage.color = Color.red;
        Debug.Log("GAME LOST!");
    }

    /// <summary>
    /// Reveals a cyan "YOU WON!" game state message on the screen.
    /// </summary>
    private void ShowWinGameState()
    {
        finalGameStateMessage.gameObject.transform.parent.gameObject.SetActive(true);
        finalGameStateMessage.text = "YOU WON!";
        finalGameStateMessage.color = Color.cyan;
        Debug.Log("GAME WON!");
    }

    /// <summary>
    /// Unsubscribes from all subscribed events to prevent memory leaks 
    /// when this object is disabled or destroyed.
    /// </summary>
    private void OnDisable()
    {
        EconomyEventBus.OnMoneyAmountChange -= UpdateMoneyText;
        WaveEventBus.OnWaveStart -= UpdateWaveText;
        EnemyEventBus.OnUpdateEnemyCountAtTarget -= UpdateEnemyCountText;
        GameStateEventBus.OnLose -= ShowLoseGameState;
        GameStateEventBus.OnWin -= ShowWinGameState;
        
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Restarts the game by sending a global restart event via GameStateEventBus.
    /// </summary>
    public void Restart()
    {
        GameStateEventBus.Restart();
    }
}
