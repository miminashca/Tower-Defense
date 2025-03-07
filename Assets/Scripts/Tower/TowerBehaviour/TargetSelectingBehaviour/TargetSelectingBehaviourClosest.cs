using UnityEngine;
using System.Collections.Generic;

public class TargetSelectingBehaviourClosest : TargetSelectingBehaviour
{
    public override List<Entity> GetTargets(List<Entity> targets, Vector3 towerPos)
    {
        List<Entity> chosenTargets = new List<Entity>();
        float relDistance = 100;
        Entity chosen = null;
        
        if (targets!=null && targets.Count > 0)
        {
            foreach (Entity entity in targets)
            {
                if (!entity) continue;
                if (Vector3.Magnitude(towerPos - entity.transform.position) < relDistance)
                {
                    chosen = entity;
                    relDistance = Vector3.Magnitude(towerPos - entity.transform.position);
                }
            }
        }
        chosenTargets.Add(chosen);
        return chosenTargets;
    }
}