using UnityEngine;

public class Coin : MonoBehaviour
{
    public float LifeTimeInSeconds = 3;
    private float timer = 0;
    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        if(timer>=LifeTimeInSeconds) Destroy(gameObject);
    }
}
