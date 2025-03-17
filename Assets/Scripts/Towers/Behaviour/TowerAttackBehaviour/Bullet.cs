using System;
using UnityEngine;
public class Bullet : MonoBehaviour
{
    public float lifeTimeInSeconds = 3;
    private float timer = 0;
    public event Action <Enemy> OnBulletReachedTarget;

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer>=lifeTimeInSeconds) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            OnBulletReachedTarget?.Invoke(enemy);
            Destroy(gameObject);
        }
    }
}