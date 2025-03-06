using System.Collections.Generic;
using UnityEngine;

public abstract class TowerAttackBehaviour : MonoBehaviour
{
    public abstract void Attack(Entity target, int impact);
}
