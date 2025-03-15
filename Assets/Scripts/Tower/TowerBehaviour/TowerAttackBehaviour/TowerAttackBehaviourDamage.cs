using UnityEngine;

public class TowerAttackBehaviourDamage : TowerAttackBehaviour
{
    protected override void DealImpact(Entity target)
    {
        base.DealImpact(target);
        target.GetDamage(impact);
        //Debug.Log($"Entity receives {impact} damage! Entities remaining health: {target.GetHP()}");
    }
}
