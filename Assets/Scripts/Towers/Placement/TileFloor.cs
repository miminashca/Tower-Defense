using System.Collections.Generic;
using UnityEngine;

public class TileFloor : MonoBehaviour
{
    public GameObject GridCellPointerPrefab;
    public Material OccupiedCellMaterial;
    public Material UnoccupiedCellMaterial;
    
    private GameObject gridCellPointer;
    private Dictionary<Vector3, GameObject> occupiedCells = new Dictionary<Vector3, GameObject>();
    private void Awake()
    {
        TowerEventBus.OnTowerStartDrag += StartPlacement;
        TowerEventBus.OnTowerEndDrag += EndPlacement;
        TowerEventBus.OnTowerRemoved += UnoccupyCellOfTower;
        TowerEventBus.OnTowerMovedToSnappedPosition += UpdateCellColour;
    }
    private void OnDestroy()
    {
        TowerEventBus.OnTowerStartDrag -= StartPlacement;
        TowerEventBus.OnTowerEndDrag -= EndPlacement;
        TowerEventBus.OnTowerRemoved -= UnoccupyCellOfTower;
        TowerEventBus.OnTowerMovedToSnappedPosition -= UpdateCellColour;
    }
    private void Start()
    {
        ActivateGrid(false);
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
    public void UnoccupyCell(Vector3Int cellPosition)
    {
        occupiedCells.Remove(cellPosition);
    }
    private void UnoccupyCellOfTower(Tower tower)
    {
        Vector3 position = tower.gameObject.transform.position;
        UnoccupyCell(new Vector3Int((int)position.x, (int)position.y, (int)position.z));
    }

    private void ActivateGrid(bool activate)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = activate;
    }
    
    private void StartPlacement(GameObject obj)
    {
        if (!gridCellPointer)
        {
            gridCellPointer = Instantiate(GridCellPointerPrefab, obj.transform);
            gridCellPointer.transform.localPosition = new Vector3(0,0.01f,0);
        }
        ActivateGrid(true);
    }
    
    private void EndPlacement(GameObject obj)
    {
        if(gridCellPointer) Destroy(gridCellPointer.gameObject);
        ActivateGrid(false);
    }
    
    private void UpdateCellColour(Vector3Int currentSnappedPosition)
    {
        if (gridCellPointer) gridCellPointer.GetComponent<Renderer>().material = CellIsOccupied(currentSnappedPosition) ? OccupiedCellMaterial : UnoccupiedCellMaterial;
    }
}
