using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// A generic base class for all managers in the game. 
/// It automatically registers itself with the Service Locator upon awakening,
/// and provides hooks for reloading and scene-loaded logic.
/// </summary>
/// <typeparam name="T">The concrete manager type inheriting from this class.</typeparam>
public abstract class Manager<T> : MonoBehaviour where T : Manager<T>
{
    /// <summary>
    /// Called by Unity when the script instance is being loaded.
    /// Registers this manager in the Service Locator so it can be accessed globally.
    /// Also subscribes to reload and scene-loaded events.
    /// </summary>
    protected virtual void Awake()
    {
        // Immediately register this manager so other scripts can retrieve it via the Service Locator.
        ServiceLocator.Register<T>(this as T);
        
        GameStateEventBus.OnReloadManagers += Reload;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Called when this manager is being destroyed (e.g., scene unload or application quit).
    /// Unsubscribes from events to prevent dangling references.
    /// </summary>
    protected virtual void OnDestroy()
    {
        GameStateEventBus.OnReloadManagers -= Reload;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// An abstract method to be implemented by derived managers for handling 
    /// any state resets or data clearing upon a reload event. 
    /// Each concrete manager defines its own reload behavior.
    /// </summary>
    protected abstract void Reload();

    /// <summary>
    /// A virtual method called whenever a new scene is loaded. 
    /// By default, it skips additional logic if the newly loaded scene is "Bootstrap".
    /// Derived classes can override or extend this for custom behavior.
    /// </summary>
    /// <param name="scene">Information about the scene that has been loaded.</param>
    /// <param name="mode">Specifies how the scene was loaded (e.g., Single or Additive).</param>
    protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Bootstrap") return;
    }
}
