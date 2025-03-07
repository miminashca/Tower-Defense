using UnityEngine;

public class TowerAttackBehaviourDamage : TowerAttackBehaviour
{
    public override void Attack(Entity target, float impact)
    {
        target.GetDamage(impact);
        //Debug.Log($"Entity receives {impact} damage! Entities remaining health: {target.GetHP()}");
    }
}
