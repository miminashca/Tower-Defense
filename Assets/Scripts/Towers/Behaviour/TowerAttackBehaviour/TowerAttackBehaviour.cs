public abstract class TowerAttackBehaviour
{
    private Enemy target;
    private Bullet bullet;
    protected float impact;
    
    public void Attack(Enemy pTarget, float pImpact, Bullet pBullet)
    {
        target = pTarget;
        impact = pImpact;
        bullet = pBullet;
        bullet.OnBulletReachedTarget += DealImpact;
    }

    protected virtual void DealImpact(Enemy pTarget)
    {
        bullet.OnBulletReachedTarget -= DealImpact;
        if(pTarget != target) return;
    }
}
