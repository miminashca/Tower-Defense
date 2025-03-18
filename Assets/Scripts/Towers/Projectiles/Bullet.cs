using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float LifeTimeInSeconds = 3;
    public event Action<Enemy> OnBulletReachedTarget;
    
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