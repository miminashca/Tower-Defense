using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{ 
    private Camera sceneCamera;
    private Vector3 lastPosition;
    public Action UpgradeTowers;
    private void Start()
    {
        sceneCamera = Camera.main;
    }
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.J)) UpgradeTowers?.Invoke();
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
                //Debug.Log("Hit Layer: " + LayerMask.LayerToName(hit.collider.gameObject.layer));
            }
            // else
            // {
            //     Debug.Log("No ground hit detected!");
            // }
        }
        return lastPosition;
    }

}