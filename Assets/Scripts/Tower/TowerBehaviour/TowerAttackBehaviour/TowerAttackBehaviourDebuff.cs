using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerAttackBehaviourDebuff : TowerAttackBehaviour
{
    private List<Entity> debuffedTargets = null;
    public override void Attack(Entity target, int impact)
    {
        if (!debuffedTargets.Contains(target))
        {
            //entity gets debuffed + sets isDebuffed true for the impact time
            debuffedTargets.Add(target);
            //target.getdebuff;
            StartCoroutine(RemoveDebuffedTargetAfterTime(target, 3f));
        }
    }

    private IEnumerator RemoveDebuffedTargetAfterTime(Entity target, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (debuffedTargets.Contains(target)) debuffedTargets.Remove(target);
    }
}
