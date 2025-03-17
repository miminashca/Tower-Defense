using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlacementSystem : MonoBehaviour
{
    public bool wasAlreadyPlaced = false;
    
    [SerializeField] private LayerMask placementLayerMask;
    private Grid grid;
    private Vector3 initialPosition;
    private Vector3Int lastGridPos = Vector3Int.zero; // Track the last grid position to reduce redundant updates
    private bool placementInProgress = false;
    private void Start()
    {
        if (!GetComponentInParent<Grid>())
        {
            Debug.Log("No grid in Placement System!!!");
            return;
        }
        grid = transform.parent.GetComponentInParent<Grid>();
        
        Vector3Int currentGridPos = grid.WorldToCell(transform.position);
        Vector3 snappedPosition = grid.GetCellCenterWorld(currentGridPos);
        snappedPosition.y = transform.position.y;
        transform.position = snappedPosition;
        
        if(wasAlreadyPlaced) GameManager.tileFloor.OccupyCell(new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z), gameObject);
    }

    private void OnEnable()
    {
        EventBus.OnTowerStartDrag += StartPlacement;
        EventBus.OnTowerEndDrag += EndPlacement;
    }
    private void OnDisable()
    {
        EventBus.OnTowerStartDrag -= StartPlacement;
        EventBus.OnTowerEndDrag -= EndPlacement;
    }
    
    public void StartPlacement(GameObject obj)
    {
        if(obj != gameObject) return;
        
        initialPosition =  transform.position;
        GameManager.tileFloor.UnoccupyCell(new Vector3Int((int)initialPosition.x, (int)initialPosition.y, (int)initialPosition.z));

        placementInProgress = true;
    }
    public void EndPlacement(GameObject obj)
    {
        placementInProgress = false;
        
        if(obj != gameObject) return;
        
        if (GameManager.tileFloor.CellIsOccupied(new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z)))
        {
            if (!wasAlreadyPlaced)
            {
                Destroy(gameObject);
                return;
            }
            transform.position = initialPosition;
        }
        GameManager.tileFloor.OccupyCell(new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z), gameObject);
        wasAlreadyPlaced = true;
    }
    public void FixedUpdate()
    {
        if (!grid || !placementInProgress) return;
        
        Vector3 mousePos = GameManager.inputManager.GetSelectedMapPosition(placementLayerMask);
        Vector3Int currentGridPos = grid.WorldToCell(mousePos);

        if (currentGridPos != lastGridPos)
        {
            lastGridPos = currentGridPos;
            Vector3 snappedPosition = grid.GetCellCenterWorld(currentGridPos);
            snappedPosition.y = transform.position.y;
            transform.position = snappedPosition;
            
            EventBus.TowerMovedToSnappedPosition(new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z));
        }
    }

}
