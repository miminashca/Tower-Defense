using UnityEngine;
using Image = UnityEngine.UI.Image;

[CreateAssetMenu(menuName = "Data/TowerData")]
public class TowerData : ScriptableObject
{
    public enum Level
    {
        Basic,
        Upgrade1,
        
        Undefined
    }
    public struct TowerStruct
    {
        public readonly int BasicPrice { get; }
        public readonly GameObject Model { get; }
        public readonly int Range { get; }
        public readonly int Impact { get; }
        public readonly float Threshold { get; }
        public readonly Sprite PreviewImage { get; }
        public TowerStruct(int basicPrice, GameObject model, int range, int impact, float threshold, Sprite previewImage)
        {
            BasicPrice = basicPrice;
            Model = model;
            Range = range;
            Impact = impact;
            Threshold = threshold;
            PreviewImage = previewImage;
        }
    }
    
    public TowerBehaviour towerBehaviour;
    
    [SerializeField] private int basicPrice;
    [SerializeField] private Sprite basicUiImage;
    [SerializeField] private GameObject basicModelPrefab;
    [SerializeField] private int basicRange = 1;
    [SerializeField] private int basicImpact = 1;
    [SerializeField] private float basicThreshold = 1f;
    
    [SerializeField] private int upgrade1BasicPrice;
    [SerializeField] private Sprite u1UiImage;
    [SerializeField] private GameObject upgrade1ModelPrefab;
    [SerializeField] private int upgrade1RangeModifier = 1;
    [SerializeField] private int upgrade1ImpactModifier = 1;
    [SerializeField] private float upgrade1ThresholdModifier = 0.7f;
    
    [SerializeField] private Sprite undefUiImage;

    private TowerStruct basicStruct;
    private TowerStruct upgrade1Struct;
    private TowerStruct undefStruct;

    private void OnEnable()
    {
        basicStruct = new TowerStruct(basicPrice, basicModelPrefab, basicRange, basicImpact, basicThreshold, basicUiImage);
        upgrade1Struct = new TowerStruct(upgrade1BasicPrice, upgrade1ModelPrefab, basicRange*upgrade1RangeModifier, basicImpact*upgrade1ImpactModifier, basicThreshold*upgrade1ThresholdModifier, u1UiImage);
        undefStruct = new TowerStruct(0, null, 0, 0, 0, undefUiImage);
    }
    public TowerStruct GetStructAtLevel(Level currentTowerLevel)
    {
        if (currentTowerLevel == Level.Basic)
        {
            return basicStruct;
        }
        if (currentTowerLevel == Level.Upgrade1)
        {
            return upgrade1Struct;
        }
        else return undefStruct;
    }
    
    public enum ImpactType
    {
        Damage,
        Debuff,
        
        Undefined,
    }
    public enum TargetSelectingType
    {
        Closest,
        AOE,
        
        Undefined,
    }

    public ImpactType GetImpactType()
    {
        return ImpactType.Damage;
    }
    public TargetSelectingType GetTargetSelectingType()
    {
        if (towerBehaviour is TowerBehaviourClosest) return TargetSelectingType.Closest;
        else
        {
            return TargetSelectingType.Undefined;
        }
    }
}
