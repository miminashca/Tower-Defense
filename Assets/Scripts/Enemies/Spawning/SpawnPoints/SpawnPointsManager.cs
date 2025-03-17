using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnPointsManager : MonoBehaviour
{
    private int numberOfPoints = 0;
    public List<SpawnPoint> spawnPoints { get; private set; }
    public SpawnPoint spawnPoint;
    public void Init(int num)
    {
        if (num != numberOfPoints)
        {
            spawnPoints = new List<SpawnPoint>(GetComponentsInChildren<SpawnPoint>());
            if (spawnPoints.Count != 0)
            {
                foreach (SpawnPoint p in spawnPoints)
                {
                    Destroy(p.gameObject);
                }
                spawnPoints.Clear();
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

                   //worldPosition += new Vector3(column, 0, -row);

                   SpawnPoint point = Instantiate(spawnPoint, worldPosition, Quaternion.identity, gameObject.transform);
                   spawnPoints.Add(point);
                }
            }
        }
    }
}
