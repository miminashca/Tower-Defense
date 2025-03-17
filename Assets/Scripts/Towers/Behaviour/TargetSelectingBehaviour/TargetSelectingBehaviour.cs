using System.Collections.Generic;
using UnityEngine;

public abstract class TargetSelectingBehaviour
{
    public abstract List<Enemy> GetTargets(List<Enemy> targets, Vector3 towerPos);
}
