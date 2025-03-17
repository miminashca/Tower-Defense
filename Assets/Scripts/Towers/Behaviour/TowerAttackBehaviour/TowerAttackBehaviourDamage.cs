using UnityEngine;

public class TowerAttackBehaviourDamage : TowerAttackBehaviour
{
    protected override void DealImpact(Enemy target)
    {
        base.DealImpact(target);
        target.GetDamage(impact);
    }
}
