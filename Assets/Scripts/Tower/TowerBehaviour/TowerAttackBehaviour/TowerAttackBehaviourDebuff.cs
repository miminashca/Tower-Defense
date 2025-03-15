using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerAttackBehaviourDebuff : TowerAttackBehaviour
{
    protected override void DealImpact(Entity target)
    {
        base.DealImpact(target);
        EventBus.TowerDebuffEntity(target, impact);
    }
}
