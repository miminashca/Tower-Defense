using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    private void Awake()
    {
        GameStateEventBus.OnRestart += Reload;
    }
    private void OnDestroy()
    {
        GameStateEventBus.OnRestart -= Reload;
    }

    private void Start()
    {
        GameStateEventBus.ReloadManagers();
        SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
    }

    private void Reload()
    {
        SceneManager.UnloadSceneAsync("Level1");
        GameStateEventBus.ReloadManagers();
        SceneManager.LoadScene("Level1", LoadSceneMode.Additive);
    }
}