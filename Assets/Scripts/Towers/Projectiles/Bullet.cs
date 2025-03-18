using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public event Action<Enemy> OnBulletReachedTarget;
    public float LifeTimeInSeconds = 3;
    
    private float timer = 0;
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer>=LifeTimeInSeconds) Destroy(gameObject);
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