using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A ScriptableObject containing configuration data for the in-game shop, 
/// such as its open duration and the available towers for purchase.
/// </summary>
[CreateAssetMenu(menuName = "Data/ShopData")]
public class ShopData : ScriptableObject
{
    /// <summary>
    /// How long (in seconds) the shop remains open in a given phase.
    /// </summary>
    public int ShopDuration = 10;
    
    /// <summary>
    /// A list of TowerData entries representing the different towers 
    /// available for the player to purchase.
    /// </summary>
    public List<TowerData> TowersToBuy;
}