using System;
using UnityEngine;
public class Bullet : MonoBehaviour
{
    public float lifeTimeInSeconds = 3;
    private float timer = 0;
    public event Action <Entity> OnBulletReachedTarget;

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer>=lifeTimeInSeconds) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Entity entity = other.GetComponent<Entity>();
        if (entity != null)
        {
            OnBulletReachedTarget?.Invoke(entity);
            Destroy(gameObject);
        }
    }
}