using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays a visual health bar for an enemy by observing HP updates 
/// and adjusting the fill value of a Slider UI element accordingly.
/// </summary>
public class HealthBar : MonoBehaviour
{
    /// <summary>
    /// The Slider UI element representing the enemy's health visually.
    /// </summary>
    private Slider healthSlider;
    
    /// <summary>
    /// A reference to the Enemy component that this health bar is tracking.
    /// </summary>
    private Enemy holder;

    /// <summary>
    /// Subscribes to the OnUpdateEnemyHP event so that this health bar updates 
    /// whenever the enemy's HP changes.
    /// </summary>
    private void Awake()
    {
        EnemyEventBus.OnUpdateEnemyHP += UpdateHealthBar;
    }

    /// <summary>
    /// Unsubscribes from the OnUpdateEnemyHP event to prevent potential errors or leaks 
    /// if this GameObject is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        EnemyEventBus.OnUpdateEnemyHP -= UpdateHealthBar;
    }

    /// <summary>
    /// Called after Awake. Fetches the Slider component in the child hierarchy 
    /// and the Enemy component in the parent, and initializes the slider's value to full (1).
    /// </summary>
    private void Start()
    {
        healthSlider = GetComponentInChildren<Slider>();
        holder = GetComponentInParent<Enemy>();
        healthSlider.value = 1;
    }

    /// <summary>
    /// Updates the slider's fill value to reflect the given enemy's current HP, 
    /// but only if the passed-in enemy is the same as the one this health bar is attached to.
    /// </summary>
    /// <param name="receivingEnemy">The enemy whose HP was updated.</param>
    public void UpdateHealthBar(Enemy receivingEnemy)
    {
        if (receivingEnemy == holder)
        {
            healthSlider.value = receivingEnemy.GetCurrentHP() / receivingEnemy.Data.HealthPoints;
        }
    }
}