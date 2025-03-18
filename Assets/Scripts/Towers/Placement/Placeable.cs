using UnityEngine;

public class Placeable : MonoBehaviour
{
    public bool WasAlreadyPlaced = false;
    
    [SerializeField] private LayerMask placementLayerMask;
    private Grid grid;
    private Vector3 initialPosition;
    private Vector3Int lastGridPos = Vector3Int.zero; // Track the last grid position to reduce redundant updates
    private bool placementInProgress = false;
    private void Start()
    {
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
        
        if(WasAlreadyPlaced) TileFloor.Instance.OccupyCell(new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z), gameObject);
    }

    private void OnEnable()
    {
        TowerEventBus.OnTowerStartDrag += StartPlacement;
        TowerEventBus.OnTowerEndDrag += EndPlacement;
    }
    private void OnDisable()
    {
        TowerEventBus.OnTowerStartDrag -= StartPlacement;
        TowerEventBus.OnTowerEndDrag -= EndPlacement;
    }
    
    public void StartPlacement(GameObject obj)
    {
        if(obj != gameObject) return;
        
        initialPosition =  transform.position;
        TileFloor.Instance.UnoccupyCell(new Vector3Int((int)initialPosition.x, (int)initialPosition.y, (int)initialPosition.z));

        placementInProgress = true;
    }
    public void EndPlacement(GameObject obj)
    {
        placementInProgress = false;
        
        if(obj != gameObject) return;
        
        if (TileFloor.Instance.CellIsOccupied(new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z)))
        {
            if (!WasAlreadyPlaced)
            {
                Destroy(gameObject);
                return;
            }
            transform.position = initialPosition;
        }
        TileFloor.Instance.OccupyCell(new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z), gameObject);
        WasAlreadyPlaced = true;
    }
    public void FixedUpdate()
    {
        if (!grid || !placementInProgress) return;
        
        Vector3 mousePos = InputManager.Instance.GetSelectedMapPosition(placementLayerMask);
        Vector3Int currentGridPos = grid.WorldToCell(mousePos);

        if (currentGridPos != lastGridPos)
        {
            lastGridPos = currentGridPos;
            Vector3 snappedPosition = grid.GetCellCenterWorld(currentGridPos);
            snappedPosition.y = transform.position.y;
            transform.position = snappedPosition;
            
            TowerEventBus.TowerMovedToSnappedPosition(new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z));
        }
    }

}
