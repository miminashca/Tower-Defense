using System.Collections.Generic;
using UnityEngine;

public abstract class TowerAttackBehaviour
{
    private Entity target;
    private Bullet bullet;
    protected float impact;
    
    public void Attack(Entity pTarget, float pImpact, Bullet pBullet)
    {
        target = pTarget;
        impact = pImpact;
        bullet = pBullet;
        bullet.OnBulletReachedTarget += DealImpact;
    }

    protected virtual void DealImpact(Entity pTarget)
    {
        bullet.OnBulletReachedTarget -= DealImpact;
        if(pTarget != target) return;
    }
}
