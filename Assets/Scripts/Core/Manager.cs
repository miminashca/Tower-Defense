using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Manager<T> : MonoBehaviour where T : Manager<T>
{
    protected virtual void Awake()
    {
        ServiceLocator.Register<T>(this as T);
        
        GameStateEventBus.OnReloadManagers += Reload;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    protected virtual void OnDestroy()
    {
        GameStateEventBus.OnReloadManagers -= Reload;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Register this Manager in Service locator.
    /// </summary>
    protected abstract void Reload();

    protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Bootstrap") return;
    }
}