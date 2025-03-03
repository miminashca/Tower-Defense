using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityData : ScriptableObject
{
    public enum DataType
    {
        EntityData,
        EnemyData,
    }

    public DataType type = DataType.EntityData;
    public int healthPoints = 2;
    public int speed = 2;
}
