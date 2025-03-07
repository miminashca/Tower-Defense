using System.Collections.Generic;
using UnityEngine;

public abstract class TargetSelectingBehaviour
{
    public abstract List<Entity> GetTargets(List<Entity> targets, Vector3 towerPos);
}
