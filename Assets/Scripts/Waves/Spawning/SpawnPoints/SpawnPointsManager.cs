using System.Collections.Generic;
using UnityEngine;

public class SpawnPointsManager : MonoBehaviour
{
    private int numberOfPoints = 0;
    public List<SpawnPoint> SpawnPoints { get; private set; }
    [SerializeField] private SpawnPoint spawnPoint;
    public void Init(int num)
    {
        if (num != numberOfPoints)
        {
            SpawnPoints = new List<SpawnPoint>(GetComponentsInChildren<SpawnPoint>());
            if (SpawnPoints.Count != 0)
            {
                foreach (SpawnPoint p in SpawnPoints)
                {
                    Destroy(p.gameObject);
                }
                SpawnPoints.Clear();
            }
            numberOfPoints = num;
            if (numberOfPoints != 0)
            {
                for (int i = 0; i < numberOfPoints; i++)
                {
                   int column = i % 2;            // 0 for left column, 1 for right column
                   int row = i / 2;               // Increments after every two items

                   Vector3Int gridPosition = new Vector3Int(column, 0, row); // Negative Y to go downwards
                   Vector3 worldPosition = GetComponent<Grid>().CellToWorld(gridPosition);

                   SpawnPoint point = Instantiate(spawnPoint, worldPosition, Quaternion.identity, gameObject.transform);
                   SpawnPoints.Add(point);
                }
            }
        }
    }
}
