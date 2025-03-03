using System;
using UnityEngine;

public class SpriteRotController : MonoBehaviour
{
    private Camera mainCam;
    
    [SerializeField] private bool freezeXZAxis = false;

    protected void Start()
    {
        if(Camera.main) mainCam = Camera.main;
        else
        {
            Debug.LogWarning("No main Camera!");
        }
    }

    protected void LateUpdate()
    {
        if (mainCam)
        {
            if(freezeXZAxis) transform.rotation = Quaternion.Euler(0f, mainCam.transform.rotation.eulerAngles.y, 0f);
            else
            {
                transform.rotation = mainCam.transform.rotation;
            }
        }
    }
}