using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerAttackBehaviourDebuff : TowerAttackBehaviour
{
    public override void Attack(Entity target, float impact)
    {
        EventBus.TowerDebuffEntity(target, impact);
    }
}
