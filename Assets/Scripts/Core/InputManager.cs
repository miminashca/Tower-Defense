using System;
using UnityEngine;

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
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        sceneCamera = Camera.main;
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

}