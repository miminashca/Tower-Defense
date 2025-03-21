using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The InputManager class handles mouse-based input for selecting map positions 
/// and interacting with GameObjects in the scene. It maintains references to the 
/// main scene camera once Level1 is loaded, providing raycast-based position and 
/// object checks.
/// </summary>
public class InputManager : MonoBehaviour
{
    /// <summary>
    /// A reference to the main camera of the current scene,
    /// used to perform raycasts for input detection.
    /// </summary>
    private Camera sceneCamera;
    
    /// <summary>
    /// Stores the last world position hit by a raycast from the mouse cursor.
    /// </summary>
    private Vector3 lastPosition;

    /// <summary>
    /// A Singleton-like reference to the active InputManager instance.
    /// </summary>
    public static InputManager Instance { get; private set; }

    /// <summary>
    /// Initializes the InputManager Singleton instance and subscribes to the sceneLoaded event.
    /// </summary>
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Unsubscribes from the sceneLoaded event upon destruction to avoid memory leaks.
    /// </summary>
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Called when a new scene is loaded.
    /// it references the main camera for subsequent input raycasts.
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sceneCamera = Camera.main;
    }
    
    /// <summary>
    /// Performs a raycast from the mouse position into the scene, restricted to 
    /// the specified LayerMask. If a hit occurs, saves and returns that position;
    /// otherwise, returns the last known valid position.
    /// </summary>
    /// <param name="mapLayerMask">The layer mask to filter raycast collisions.</param>
    /// <returns>The world position corresponding to the mouse raycast hit.</returns>
    public Vector3 GetSelectedMapPosition(LayerMask mapLayerMask)
    {
        if (sceneCamera)
        {
            Ray ray = sceneCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Ensure raycast only hits objects on the correct layer
            if (Physics.Raycast(ray, out hit, 100, mapLayerMask))
            {
                lastPosition = hit.point;
            }
        }
        return lastPosition;
    }

    /// <summary>
    /// Checks whether the mouse cursor, when clicked, is targeting the specified GameObject.
    /// Performs a raycast from the mouse cursor to see if it hits the provided target object.
    /// </summary>
    /// <param name="target">The GameObject to check for a mouse click.</param>
    /// <returns>True if the raycast hits the target object; otherwise, false.</returns>
    public bool CheckPressOnGameObject(GameObject target)
    {
        if (sceneCamera)
        {
            Ray ray = sceneCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.gameObject == target) 
                    return true;
            }
        }
        return false;
    }
}
