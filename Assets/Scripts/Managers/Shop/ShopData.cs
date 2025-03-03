using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/ShopData")]
public class ShopData : ScriptableObject
{
    public int shopDuration = 10;
    public List<TowerData> towersToBuy;
}
