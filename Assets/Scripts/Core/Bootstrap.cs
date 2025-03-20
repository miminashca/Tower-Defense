using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The Bootstrap class is responsible for setting up or resetting the game
/// at startup and on demand. It loads the initial gameplay scene (Level1) in an additive manner,
/// and provides a method to unload and reload that scene when a restart event is triggered.
/// </summary>
public class Bootstrap : MonoBehaviour
{
    /// <summary>
    /// Subscribes to the global restart event upon object creation.
    /// </summary>
    private void Awake()
    {
        GameStateEventBus.OnRestart += Reload;
    }

    /// <summary>
    /// Unsubscribes from the global restart event when the object is destroyed,
    /// preventing potential memory leaks.
    /// </summary>
    private void OnDestroy()
    {
        GameStateEventBus.OnRestart -= Reload;
    }

    /// <summary>
    /// Called by Unity after Awake. Invokes the ReloadManagers event and then
    /// loads the Level1 scene additively, keeping this Bootstrap scene active in parallel.
    /// </summary>
    private void Start()
    {
        GameStateEventBus.ReloadManagers();
        SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
    }

    /// <summary>
    /// Called when the OnRestart event is raised. Unloads the current Level1 scene,
    /// triggers ReloadManagers, and then loads Level1 again additively to effectively restart the gameplay.
    /// </summary>
    private void Reload()
    {
        SceneManager.UnloadSceneAsync("Level1");
        GameStateEventBus.ReloadManagers();
        SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
    }
}