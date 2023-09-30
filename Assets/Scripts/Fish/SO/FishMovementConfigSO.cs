using UnityEngine;

[CreateAssetMenu(menuName = "FishMovementConfig", fileName = "FishMovementConfig")]
public class FishMovementConfigSO : ScriptableObject
{
    [field: SerializeField] public float MoveSpeed {get;  set;} = 1.25f;
    [field: SerializeField] public float RotationSpeed {get;  set;} = 2f;
    [field: SerializeField] public float MinMoveSpeed {get;  set;} = 0.5f;
    [field: SerializeField] public float MinRotationSpeed {get;  set;} = 1f;

    [field: SerializeField] public float MoveSpeedAtWarn {get; private set;} = 5f; 
    [field: SerializeField] public float RotationSpeedAtWarn {get; private set;} = 6f;

    [field: SerializeField] public float MinMoveSpeedAtWarn {get; private set;} = 1f; 
    [field: SerializeField] public float MinRotationSpeedAtWarn {get; private set;} = 2f;

    [field: SerializeField] public float WarnSpeedFadeDuration {get; private set;} = 1.75f;
    [field: SerializeField] public float WarnPushMultiplier {get; private set;} = 5f;
    [field: SerializeField] public float MoveSpeedReduceAmountOnGrowth {get; private set;} = 5f;
    [field: SerializeField] public float RotationSpeedReduceAmountOnGrowth {get; private set;} = 5f;

    [field: SerializeField] public float MoveSpeedAtWarnReduceAmountOnGrowth {get; private set;} = 5f;
    [field: SerializeField] public float RotationSpeedAtWarnReduceAmountOnGrowth {get; private set;} = 5f;
}
