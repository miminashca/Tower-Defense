using UnityEngine;
using System.Collections.Generic;

public class TargetSelectingBehaviourClosest : TargetSelectingBehaviour
{
    public override Entity GetTarget(List<Entity> targets, Vector3 towerPos)
    {
        //List<Entity> chosenTargets = new List<Entity>();
        int relDistance = 100;
        Entity chosen = null;
        
        if (targets!=null && targets.Count > 0)
        {
            foreach (Entity entity in targets)
            {
                if (!entity) continue;
                if (Vector3.Magnitude(towerPos - entity.transform.position) < relDistance)
                    chosen = entity;
            }
        }
        //chosenTargets.Add(chosen);
        return chosen;
    }
}