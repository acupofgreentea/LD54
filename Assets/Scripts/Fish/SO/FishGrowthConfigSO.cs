using UnityEngine;

[CreateAssetMenu(menuName = "FishGrowthConfig", fileName = "NewFishGrowthConfig")]
public class FishGrowthConfigSO : ScriptableObject
{
    [field: SerializeField] public float GrowthAmount {get; private set;} = 0.25f;
    [field: SerializeField] public float GrowthAmountAtMature {get; private set;} = 0.05f;
    [field: SerializeField] public float MatureGrowthAmount {get; private set;} = 3f;
    [field: SerializeField] public float GrowthScaleDuration {get; private set;} = 0.35f;
    [field: SerializeField] public float FullnessDuration {get; private set;} = 5f;
    [field: SerializeField] public bool StopEatingFoodAtMature {get; private set;} = false;

    [field: SerializeField] public float RevenueAmount {get; private set;} = 5f;
}
