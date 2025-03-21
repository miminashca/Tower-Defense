using UnityEngine;

/// <summary>
/// The Placeable class allows an object (e.g., a tower) to be placed on a grid.
/// It snaps the object's position to grid cells and manages occupying or unoccupying those cells.
/// </summary>
public class Placeable : MonoBehaviour
{
    /// <summary>
    /// Indicates whether the object was already placed on the grid. 
    /// If true, the cell is considered occupied at startup.
    /// </summary>
    public bool WasAlreadyPlaced = false;
    
    /// <summary>
    /// The layer mask used to ensure the raycast for placement only hits valid ground or grid layers.
    /// </summary>
    [SerializeField] private LayerMask placementLayerMask;
    
    /// <summary>
    /// A reference to the grid component in the object's parent, used to snap the object's position.
    /// </summary>
    private Grid grid;
    
    /// <summary>
    /// Stores the initial position of the object before the user starts dragging it.
    /// </summary>
    private Vector3 initialPosition;
    
    /// <summary>
    /// Caches the last known grid cell position to avoid unnecessary position updates.
    /// </summary>
    private Vector3Int lastGridPos = Vector3Int.zero;
    
    /// <summary>
    /// A flag indicating whether the placement process (dragging) is currently in progress.
    /// </summary>
    private bool placementInProgress = false;

    private TileFloor tileFloor;   

    /// <summary>
    /// Initializes the object's position on the grid at startup, and marks the cell occupied if it was already placed.
    /// </summary>
    private void Start()
    {
        tileFloor = FindFirstObjectByType<TileFloor>();
        
        if (!GetComponentInParent<Grid>())
        {
            Debug.Log("No grid in Placeable Parent!!!");
            return;
        }
        grid = transform.parent.GetComponentInParent<Grid>();
        
        Vector3Int currentGridPos = grid.WorldToCell(transform.position);
        Vector3 snappedPosition = grid.GetCellCenterWorld(currentGridPos);
        snappedPosition.y = transform.position.y;
        transform.position = snappedPosition;
        
        if(!tileFloor) return;
        if (WasAlreadyPlaced)
        {
            tileFloor.OccupyCell(
                new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z), 
                gameObject
            );
        }
    }

    /// <summary>
    /// Subscribes to tower drag events when the object becomes enabled.
    /// </summary>
    private void OnEnable()
    {
        TowerEventBus.OnTowerStartDrag += StartPlacement;
        TowerEventBus.OnTowerEndDrag += EndPlacement;
    }

    /// <summary>
    /// Unsubscribes from tower drag events when the object is disabled.
    /// </summary>
    private void OnDisable()
    {
        TowerEventBus.OnTowerStartDrag -= StartPlacement;
        TowerEventBus.OnTowerEndDrag -= EndPlacement;
    }
    
    /// <summary>
    /// Called when dragging begins on this object. Stores the initial position and unoccupies its current cell.
    /// </summary>
    /// <param name="obj">The GameObject being dragged (must match this object to proceed).</param>
    public void StartPlacement(GameObject obj)
    {
        if(obj != gameObject || !tileFloor) return;
        
        initialPosition = transform.position;
        tileFloor.UnoccupyCell(
            new Vector3Int((int)initialPosition.x, (int)initialPosition.y, (int)initialPosition.z)
        );

        placementInProgress = true;
    }

    /// <summary>
    /// Called when dragging ends on this object. If the new position is occupied and the object was not 
    /// previously placed, it is destroyed. Otherwise, it reverts to the initial position or occupies the new cell.
    /// </summary>
    /// <param name="obj">The GameObject that stopped being dragged (must match this object to proceed).</param>
    public void EndPlacement(GameObject obj)
    {
        placementInProgress = false;
        
        if(obj != gameObject) return;
        
        Vector3Int newPosition = new Vector3Int(
            (int)transform.position.x, 
            (int)transform.position.y, 
            (int)transform.position.z
        );
        WasAlreadyPlaced = true;
        
        if(!tileFloor) return;
        if (tileFloor.CellIsOccupied(newPosition))
        {
            if (!WasAlreadyPlaced)
            {
                Destroy(gameObject);
                return;
            }
            transform.position = initialPosition;
        }

        tileFloor.OccupyCell(newPosition, gameObject);
    }

    /// <summary>
    /// Updates the object's snapped position on the grid while placement is in progress, 
    /// using raycast data from the InputReader.
    /// </summary>
    public void Update()
    {
        if (!grid || !placementInProgress) return;
        
        Vector3 mousePos = InputReader.Instance.GetSelectedMapPosition(placementLayerMask);
        Vector3Int currentGridPos = grid.WorldToCell(mousePos);

        if (currentGridPos != lastGridPos)
        {
            lastGridPos = currentGridPos;
            Vector3 snappedPosition = grid.GetCellCenterWorld(currentGridPos);
            snappedPosition.y = transform.position.y;
            transform.position = snappedPosition;
            
            TowerEventBus.TowerMovedToSnappedPosition(
                new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z)
            );
        }
    }
}
