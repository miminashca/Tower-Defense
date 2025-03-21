using System;
using UnityEngine;

/// <summary>
/// Bullet that is being shot by the tower.
/// Follows a target until reaching it, or until lifetime ends.
/// </summary>
public class Bullet : MonoBehaviour
{
    public event Action<Enemy> OnBulletReachedTarget;
    public float BulletSpeed = 20;
    public float LifeTimeInSeconds = 3;
    
    [NonSerialized] public GameObject Target;
    private float timer = 0;
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer>=LifeTimeInSeconds) Destroy(gameObject);
    }
    private void FixedUpdate()
    {
        if(!Target) return;
        
        if (TimeScaler.Instance.TimeScaleForGameObjects != 0f)
            gameObject.GetComponent<Rigidbody>().linearVelocity =
                Vector3.Normalize(Target.transform.position - gameObject.transform.position) * BulletSpeed;
        else gameObject.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
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