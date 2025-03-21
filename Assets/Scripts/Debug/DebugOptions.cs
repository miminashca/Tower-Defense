using UnityEngine;

/// <summary>
/// Provides in-Inspector toggles for various debug options (instant kill, infinite money, etc.)
/// and also methods to trigger immediate game over or game win. 
/// The toggles automatically update static fields in OnValidate().
/// </summary>
public class DebugOptions : MonoBehaviour
{
    /// <summary>
    /// Inspector-exposed booleans for enabling or disabling certain debug behaviors.
    /// </summary>
    [Header("Debug Toggles")]
    public bool instantKillToggle;
    public bool infiniteMoneyToggle;
    public bool invincibleBaseToggle;

    /// <summary>
    /// Static properties reflecting the current debug states. 
    /// Other classes can check these to modify game behavior accordingly.
    /// </summary>
    public static bool InstantKill { get; private set; }
    public static bool InfiniteMoney { get; private set; }
    public static bool InvincibleBase { get; private set; }

    /// <summary>
    /// Called whenever any serialized fields change in the Inspector. 
    /// Synchronizes the public toggles with the static debug flags.
    /// </summary>
    private void OnValidate()
    {
        InstantKill = instantKillToggle;
        InfiniteMoney = infiniteMoneyToggle;
        InvincibleBase = invincibleBaseToggle;
    }

    /// <summary>
    /// Forces an immediate game over state by invoking the Lose event.
    /// This method is also exposed to the custom editor button for quick testing.
    /// </summary>
    public void TriggerImmediateGameOver()
    {
        GameStateEventBus.Lose();
    }

    /// <summary>
    /// Forces an immediate game win state by invoking the Win event.
    /// This method is also exposed to the custom editor button for quick testing.
    /// </summary>
    public void TriggerImmediateGameWin()
    {
        GameStateEventBus.Win();
    }
}