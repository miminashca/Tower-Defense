using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{ 
    private Camera sceneCamera;
    private Vector3 lastPosition;

    public static InputManager Instance { get; private set; }
    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Level1")
        {
            sceneCamera = Camera.main;
        }
    }
    
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
    public bool CheckPressOnGameObject(GameObject target)
    {
        if (sceneCamera)
        {
            Ray ray = sceneCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.gameObject == target) return true;
            }
        }
        return false;
    }
}