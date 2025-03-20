using UnityEngine;

/// <summary>
/// A ScriptableObject containing configuration data for a tower, including 
/// cost, model, range, and impact values at various upgrade levels. This class 
/// also defines the tower's attack behavior type (e.g., damage or debuff) and 
/// target selection strategy (closest or AOE).
/// </summary>
[CreateAssetMenu(menuName = "Data/TowerData")]
public class TowerData : ScriptableObject
{
    /// <summary>
    /// Enumerates the available upgrade levels for a tower.
    /// </summary>
    public enum Level
    {
        Basic,
        Upgrade1,
        
        Undefined
    }
    
    /// <summary>
    /// A structure holding the tower's stats and model for a specific level, 
    /// such as its price, visual model, range, impact, firing threshold, 
    /// and a UI preview image.
    /// </summary>
    public struct TowerStruct
    {
        /// <summary>
        /// The cost of this tower level in basic currency.
        /// </summary>
        public readonly int BasicPrice;
        
        /// <summary>
        /// The 3D model prefab for this tower level.
        /// </summary>
        public readonly GameObject Model;
        
        /// <summary>
        /// The effective range of this tower level (how far it can target or affect enemies).
        /// </summary>
        public readonly float Range;
        
        /// <summary>
        /// The impact or damage value for this tower level.
        /// </summary>
        public readonly float Impact;
        
        /// <summary>
        /// The threshold (e.g., rate of fire) that determines how frequently this tower attacks.
        /// </summary>
        public readonly float Threshold;
        
        /// <summary>
        /// The sprite displayed in the UI to represent this tower level.
        /// </summary>
        public readonly Sprite PreviewImage;
        
        /// <summary>
        /// Constructs a new TowerStruct with the specified stats and references.
        /// </summary>
        /// <param name="basicPrice">Cost of the tower level.</param>
        /// <param name="model">The 3D model prefab.</param>
        /// <param name="range">The range value.</param>
        /// <param name="impact">The impact (damage) value.</param>
        /// <param name="threshold">The firing threshold or attack speed factor.</param>
        /// <param name="previewImage">The UI sprite representing this level.</param>
        public TowerStruct(int basicPrice, GameObject model, float range, float impact, float threshold, Sprite previewImage)
        {
            BasicPrice = basicPrice;
            Model = model;
            Range = range;
            Impact = impact;
            Threshold = threshold;
            PreviewImage = previewImage;
        }
    }

    /// <summary>
    /// Specifies the type of impact this tower inflicts (damage or debuff).
    /// </summary>
    public enum ImpactType
    {
        Damage,
        Debuff
    }
    
    /// <summary>
    /// Defines how this tower selects its targets (closest or area-of-effect).
    /// </summary>
    public enum TargetSelectingType
    {
        Closest,
        AOE
    }

    /// <summary>
    /// The tower's chosen attack behavior type (damage or debuff).
    /// </summary>
    public ImpactType towerAttackBehaviourType;
    
    /// <summary>
    /// The tower's chosen target selection type (closest or AOE).
    /// </summary>
    public TargetSelectingType towerTargetSelectingType;
    
    /// <summary>
    /// The base cost for the tower at its Basic level.
    /// </summary>
    [SerializeField] private int basicPrice;
    
    /// <summary>
    /// The UI sprite used to represent the tower at its Basic level.
    /// </summary>
    [SerializeField] private Sprite basicUiImage;
    
    /// <summary>
    /// The model prefab for the tower's Basic level.
    /// </summary>
    [SerializeField] private GameObject basicModelPrefab;
    
    /// <summary>
    /// The range value for the tower at its Basic level.
    /// </summary>
    [SerializeField] private float basicRange = 1;
    
    /// <summary>
    /// The impact (damage) value for the tower at its Basic level.
    /// </summary>
    [SerializeField] private float basicImpact = 1;
    
    /// <summary>
    /// The firing threshold (attack rate) for the tower at its Basic level.
    /// </summary>
    [SerializeField] private float basicThreshold = 1f;
    
    /// <summary>
    /// The base cost for the tower at its Upgrade1 level.
    /// </summary>
    [SerializeField] private int upgrade1BasicPrice;
    
    /// <summary>
    /// The UI sprite used to represent the tower at its Upgrade1 level.
    /// </summary>
    [SerializeField] private Sprite u1UiImage;
    
    /// <summary>
    /// The model prefab for the tower's Upgrade1 level.
    /// </summary>
    [SerializeField] private GameObject upgrade1ModelPrefab;
    
    /// <summary>
    /// A multiplier applied to the tower's Basic range for the Upgrade1 level.
    /// </summary>
    [SerializeField] private float upgrade1RangeModifier = 1;
    
    /// <summary>
    /// A multiplier applied to the tower's Basic impact for the Upgrade1 level.
    /// </summary>
    [SerializeField] private float upgrade1ImpactModifier = 1;
    
    /// <summary>
    /// A multiplier applied to the tower's Basic threshold for the Upgrade1 level 
    /// (a value less than 1 speeds up the firing rate).
    /// </summary>
    [SerializeField] private float upgrade1ThresholdModifier = 0.7f;
    
    /// <summary>
    /// The UI sprite displayed if a requested level is undefined.
    /// </summary>
    [SerializeField] private Sprite undefUiImage;

    /// <summary>
    /// The cached TowerStruct for the Basic level.
    /// </summary>
    private TowerStruct basicStruct;
    
    /// <summary>
    /// The cached TowerStruct for the Upgrade1 level.
    /// </summary>
    private TowerStruct upgrade1Struct;
    
    /// <summary>
    /// The fallback TowerStruct if the requested level is Undefined.
    /// </summary>
    private TowerStruct undefStruct;

    /// <summary>
    /// Called when this ScriptableObject is enabled. Initializes the TowerStruct 
    /// instances for each defined level (Basic, Upgrade1) and the fallback (Undefined).
    /// </summary>
    private void OnEnable()
    {
        basicStruct = new TowerStruct(
            basicPrice, 
            basicModelPrefab, 
            basicRange, 
            basicImpact, 
            basicThreshold, 
            basicUiImage
        );
        
        upgrade1Struct = new TowerStruct(
            upgrade1BasicPrice, 
            upgrade1ModelPrefab, 
            basicRange * upgrade1RangeModifier, 
            basicImpact * upgrade1ImpactModifier, 
            basicThreshold * upgrade1ThresholdModifier, 
            u1UiImage
        );
        
        undefStruct = new TowerStruct(
            0, 
            null, 
            0, 
            0, 
            0, 
            undefUiImage
        );
    }

    /// <summary>
    /// Retrieves the TowerStruct for the specified tower level (Basic, Upgrade1, or Undefined).
    /// </summary>
    /// <param name="currentTowerLevel">The level of the tower.</param>
    /// <returns>The corresponding TowerStruct for that level.</returns>
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
        else 
        {
            return undefStruct;
        }
    }

    /// <summary>
    /// Returns the configured ImpactType for this tower, 
    /// indicating whether it deals damage or applies a debuff.
    /// </summary>
    /// <returns>The tower's ImpactType enum value.</returns>
    public ImpactType GetImpactType()
    {
        return towerAttackBehaviourType;
    }

    /// <summary>
    /// Returns the configured TargetSelectingType for this tower, 
    /// indicating how it selects targets (closest or AOE).
    /// </summary>
    /// <returns>The tower's TargetSelectingType enum value.</returns>
    public TargetSelectingType GetTargetSelectingType()
    {
        return towerTargetSelectingType;
    }
}
