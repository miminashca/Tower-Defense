using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/ShopData")]
public class ShopData : ScriptableObject
{
    public int ShopDuration = 10;
    public List<TowerData> TowersToBuy;
}
