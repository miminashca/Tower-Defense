using System;
using UnityEngine;

public class EnemyEventBus : MonoBehaviour
{
    public event Action<Vector3> OnStartMoveToPosition;
    public void StartMoveToPosition(Vector3 position)
    {
        OnStartMoveToPosition?.Invoke(position);
    }
}
