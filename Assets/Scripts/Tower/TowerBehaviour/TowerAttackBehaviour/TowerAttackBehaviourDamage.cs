using UnityEngine;

public class TowerAttackBehaviourDamage : TowerAttackBehaviour
{
    public override void Attack(Entity target, int impact)
    {
        target.GetDamage(impact);
    }
}
