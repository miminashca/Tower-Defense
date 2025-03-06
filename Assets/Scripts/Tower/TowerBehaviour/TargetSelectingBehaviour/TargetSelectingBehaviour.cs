using System.Collections.Generic;
using UnityEngine;

public abstract class TargetSelectingBehaviour : MonoBehaviour
{
    public abstract Entity GetTarget(List<Entity> targets);
}
