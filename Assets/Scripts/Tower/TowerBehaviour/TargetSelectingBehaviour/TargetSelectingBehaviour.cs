using System.Collections.Generic;
using UnityEngine;

public abstract class TargetSelectingBehaviour
{
    public abstract Entity GetTarget(List<Entity> targets, Vector3 towerPos);
}
