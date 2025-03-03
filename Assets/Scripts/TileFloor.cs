using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileFloor : MonoBehaviour
{
    public GameObject gridCellPointerPrefab;
    private GameObject gridCellPointer;
    public Material occupiedCellMaterial;
    public Material unoccupiedCellMaterial;
    private Dictionary<Vector3, GameObject> occupiedCells = new Dictionary<Vector3, GameObject>();
    private void Awake()
    {
        EventBus.OnTowerStartDrag += StartPlacement;
        EventBus.OnTowerEndDrag += EndPlacement;
        EventBus.OnTowerRemoved += UnoccupyCellOfTower;
        EventBus.OnTowerMovedToSnappedPosition += UpdateCellColour;
    }

    private void Start()
    {
        ActivateGrid(false);
    }

    private void OnDestroy()
    {
        EventBus.OnTowerStartDrag -= StartPlacement;
        EventBus.OnTowerEndDrag -= EndPlacement;
        EventBus.OnTowerRemoved -= UnoccupyCellOfTower;
        EventBus.OnTowerMovedToSnappedPosition -= UpdateCellColour;
    }
    public bool CellIsOccupied(Vector3Int cell)
    {
        if (occupiedCells.ContainsKey(cell)) return true;
        return false;
    }
    public void OccupyCell(Vector3Int cellPosition, GameObject occupant)
    {
        occupiedCells[cellPosition] = occupant;
    }

    private void UnoccupyCellOfTower(Tower tower)
    {
        Vector3 position = tower.gameObject.transform.position;
        UnoccupyCell(new Vector3Int((int)position.x, (int)position.y, (int)position.z));
    }
    public void UnoccupyCell(Vector3Int cellPosition)
    {
        occupiedCells.Remove(cellPosition);
    }

    private void ActivateGrid(bool activate)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = activate;
    }
    private void StartPlacement(GameObject obj)
    {
        if (!gridCellPointer)
        {
            gridCellPointer = Instantiate(gridCellPointerPrefab, obj.transform);
            gridCellPointer.transform.localPosition = new Vector3(0,0.01f,0);
        }
        ActivateGrid(true);
    }
    private void EndPlacement(GameObject obj)
    {
        if(gridCellPointer) Destroy(gridCellPointer.gameObject);
        ActivateGrid(false);
    }
    public void UpdateCellColour(Vector3Int currentSnappedPosition)
    {
        if (gridCellPointer) gridCellPointer.GetComponent<Renderer>().material = CellIsOccupied(currentSnappedPosition) ? occupiedCellMaterial : unoccupiedCellMaterial;
    }
}
