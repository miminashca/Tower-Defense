using UnityEngine;
using System.Collections.Generic;

public class TargetSelectingBehaviourClosest : TargetSelectingBehaviour
{
    public override List<Enemy> GetTargets(List<Enemy> targets, Vector3 towerPos)
    {
        List<Enemy> chosenTargets = new List<Enemy>();
        float relDistance = 100;
        Enemy chosen = null;
        
        if (targets!=null && targets.Count > 0)
        {
            foreach (Enemy entity in targets)
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