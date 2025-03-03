using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviourClosest : TowerBehaviour
{
    new protected void Start()
    {
        base.Start();
        if (!GetComponent<TargetSelectingBehaviourClosest>())
        {
            targetSelectingBehaviour = gameObject.AddComponent(typeof(TargetSelectingBehaviourClosest)) as TargetSelectingBehaviourClosest;
        }
        else
        {
            targetSelectingBehaviour = GetComponent<TargetSelectingBehaviourClosest>();
        }
    }
}
