using System.Collections.Generic;
using UnityEngine;

public abstract class TargetSelectingBehaviour : MonoBehaviour
{
    public virtual Entity GetTarget(List<Entity> targets)
    {
        return null;
    }
}
