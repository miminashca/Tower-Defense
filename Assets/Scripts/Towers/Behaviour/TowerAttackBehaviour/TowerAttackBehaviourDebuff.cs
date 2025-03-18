public class TowerAttackBehaviourDebuff : TowerAttackBehaviour
{
    protected override void DealImpact(Enemy target)
    {
        base.DealImpact(target);
        target.GetDebuff(impact);
    }
}
