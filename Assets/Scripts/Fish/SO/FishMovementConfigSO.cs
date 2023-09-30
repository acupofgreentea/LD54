using UnityEngine;

[CreateAssetMenu(menuName = "FishMovementConfig", fileName = "FishMovementConfig")]
public class FishMovementConfigSO : ScriptableObject
{
    [field: SerializeField] public float MoveSpeed {get;  set;} = 1.25f;
    [field: SerializeField] public float RotationSpeed {get;  set;} = 2f;

    [field: SerializeField] public float MoveSpeedAtWarn {get; private set;} = 5f; 
    [field: SerializeField] public float RotationSpeedAtWarn {get; private set;} = 6f;

    [field: SerializeField] public float WarnSpeedFadeDuration {get; private set;} = 1.75f;
}
