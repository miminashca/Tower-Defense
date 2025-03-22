using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float lifeTimeInSeconds = 3;
    private float timer = 0;
    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        if(timer>=lifeTimeInSeconds) Destroy(gameObject);
    }
}
