using UnityEngine;

[CreateAssetMenu(menuName = "Data/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float HealthPoints = 2;
    public float Speed = 2;
    public int CarriedMoney = 0;
    
    public GameObject Model;
}
