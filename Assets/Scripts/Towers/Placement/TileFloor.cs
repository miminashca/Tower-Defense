using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The TileFloor class manages a grid-like floor where towers can be placed. 
/// It keeps track of occupied cells, provides visual feedback for tower placement, 
/// and updates the grid's appearance based on occupancy status.
/// </summary>
public class TileFloor : MonoBehaviour
{
    /// <summary>
    /// A prefab used to visualize the current cell under the mouse when placing a tower.
    /// </summary>
    public GameObject GridCellPointerPrefab;
    
    /// <summary>
    /// The material applied to the grid cell pointer if the cell is occupied.
    /// </summary>
    public Material OccupiedCellMaterial;
    
    /// <summary>
    /// The material applied to the grid cell pointer if the cell is unoccupied.
    /// </summary>
    public Material UnoccupiedCellMaterial;
    
    /// <summary>
    /// An instance of the cell pointer GameObject that follows the tower during placement.
    /// </summary>
    private GameObject gridCellPointer;
    
    /// <summary>
    /// A dictionary tracking occupied cells, mapping cell positions to the occupying tower GameObject.
    /// </summary>
    private Dictionary<Vector3, GameObject> occupiedCells = new Dictionary<Vector3, GameObject>();

    /// <summary>
    /// Subscribes to tower events related to placement,
    /// and initializes data structures.
    /// </summary>
    private void Awake()
    {
        TowerEventBus.OnTowerStartDrag += StartPlacement;
        TowerEventBus.OnTowerEndDrag += EndPlacement;
        TowerEventBus.OnTowerRemoved += UnoccupyCellOfTower;
        TowerEventBus.OnTowerMovedToSnappedPosition += UpdateCellColour;
    }

    /// <summary>
    /// Unsubscribes from tower events when this TileFloor is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        TowerEventBus.OnTowerStartDrag -= StartPlacement;
        TowerEventBus.OnTowerEndDrag -= EndPlacement;
        TowerEventBus.OnTowerRemoved -= UnoccupyCellOfTower;
        TowerEventBus.OnTowerMovedToSnappedPosition -= UpdateCellColour;
    }

    /// <summary>
    /// Hides the grid visualization on Start.
    /// </summary>
    private void Start()
    {
        ActivateGrid(false);
    }
    
    /// <summary>
    /// Checks whether a given grid cell position is occupied by a tower.
    /// </summary>
    /// <param name="cell">The grid cell position to check.</param>
    /// <returns>True if the cell is occupied; otherwise, false.</returns>
    public bool CellIsOccupied(Vector3Int cell)
    {
        return occupiedCells.ContainsKey(cell);
    }

    /// <summary>
    /// Marks a specified cell as occupied by a particular GameObject (e.g., a tower).
    /// </summary>
    /// <param name="cellPosition">The grid cell position to occupy.</param>
    /// <param name="occupant">The GameObject occupying the cell.</param>
    public void OccupyCell(Vector3Int cellPosition, GameObject occupant)
    {
        occupiedCells[cellPosition] = occupant;
    }

    /// <summary>
    /// Unmarks a specified cell, making it available for tower placement again.
    /// </summary>
    /// <param name="cellPosition">The grid cell position to free.</param>
    public void UnoccupyCell(Vector3Int cellPosition)
    {
        occupiedCells.Remove(cellPosition);
    }

    /// <summary>
    /// Unoccupies the cell corresponding to the given tower's position,
    /// used when a tower is removed from the scene.
    /// </summary>
    /// <param name="tower">The tower being removed.</param>
    private void UnoccupyCellOfTower(Tower tower)
    {
        Vector3 position = tower.gameObject.transform.position;
        UnoccupyCell(new Vector3Int((int)position.x, (int)position.y, (int)position.z));
    }

    /// <summary>
    /// Toggles the visibility of the grid floor.
    /// </summary>
    /// <param name="activate">If true, shows the grid; if false, hides it.</param>
    private void ActivateGrid(bool activate)
    {
        gameObject.GetComponent<MeshRenderer>().enabled = activate;
    }

    /// <summary>
    /// Called when a tower starts being dragged. Instantiates a cell pointer
    /// and enables the grid visualization for placement feedback.
    /// </summary>
    /// <param name="obj">The tower GameObject that is being dragged.</param>
    private void StartPlacement(GameObject obj)
    {
        if (!gridCellPointer)
        {
            gridCellPointer = Instantiate(GridCellPointerPrefab, obj.transform);
            gridCellPointer.transform.localPosition = new Vector3(0,0.01f,0);
        }
        ActivateGrid(true);
    }

    /// <summary>
    /// Called when a tower finishes being dragged. Destroys the cell pointer
    /// and disables the grid visualization.
    /// </summary>
    /// <param name="obj">The tower GameObject that finished dragging.</param>
    private void EndPlacement(GameObject obj)
    {
        if(gridCellPointer) Destroy(gridCellPointer.gameObject);
        ActivateGrid(false);
    }

    /// <summary>
    /// Updates the cell pointer's material based on whether the snapped position
    /// is already occupied, providing visual feedback for valid or invalid placement.
    /// </summary>
    /// <param name="currentSnappedPosition">The grid cell position to color-code.</param>
    private void UpdateCellColour(Vector3Int currentSnappedPosition)
    {
        if (gridCellPointer)
        {
            gridCellPointer.GetComponent<Renderer>().material =
                CellIsOccupied(currentSnappedPosition) ? OccupiedCellMaterial : UnoccupiedCellMaterial;
        }
    }
}
