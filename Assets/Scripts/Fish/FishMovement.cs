using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FishMovement : MonoBehaviour
{
    [SerializeField] private FishMovementConfigSO fishMovementConfigSO;

    [SerializeField] private Transform model;
    [SerializeField] private float minWarnDistance = 3f;
    private float moveSpeed;
    private float rotationSpeed;

    private float moveSpeedAtWarn;
    private float rotationSpeedAtWarn;

    public Vector3? TargetPosition {get; set;}
    private Fish fish;

    private float defaultMoveSpeed;
    private float defaultRotationSpeed;
    private float defaultMoveSpeedAtWarn;
    private float defaultRotationSpeedAtWarn;

    private Coroutine warnCoroutine;

    private NavMeshAgent agent;

    public bool HasPath => agent.hasPath;
    public FishMovement Init(Fish fish)
    {
        this.fish = fish;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;

        moveSpeed = fishMovementConfigSO.MoveSpeed;
        rotationSpeed = fishMovementConfigSO.RotationSpeed;

        moveSpeedAtWarn = fishMovementConfigSO.MoveSpeedAtWarn;
        rotationSpeedAtWarn = fishMovementConfigSO.RotationSpeedAtWarn;

        defaultMoveSpeed = moveSpeed;
        defaultRotationSpeed = rotationSpeed;

        defaultMoveSpeedAtWarn = moveSpeedAtWarn;
        defaultRotationSpeedAtWarn = rotationSpeedAtWarn;
        
        fish.OnSpawned += HandleOnSpawned;
        fish.OnDespawned += HandleOnDespawned;

        return this;
    }

    private void HandleOnDespawned()
    {
        fish.FishStateController.OnFishWarned -= HandleFishWarned;
        fish.FishGrowthController.OnCurrentFoodSet -= HandleCurrentFoodSet;
        fish.FishGrowthController.OnCurrentFoodDespawned -= HandleCurrentFoodDespawned;
        fish.FishGrowthController.OnFoodEaten -= HandleOnFoodEaten;
        fish.FishHealth.OnDie -= HandleOnDie;
    }

    private void HandleOnSpawned()
    {
        fish.FishStateController.OnFishWarned += HandleFishWarned;
        fish.FishGrowthController.OnCurrentFoodSet += HandleCurrentFoodSet;
        fish.FishGrowthController.OnCurrentFoodDespawned += HandleCurrentFoodDespawned;
        fish.FishGrowthController.OnFoodEaten += HandleOnFoodEaten;
        fish.FishHealth.OnDie += HandleOnDie;

        defaultMoveSpeed = fishMovementConfigSO.MoveSpeed;
        defaultRotationSpeed = fishMovementConfigSO.RotationSpeed;

        moveSpeedAtWarn = defaultMoveSpeedAtWarn;
        rotationSpeedAtWarn = defaultRotationSpeedAtWarn;

        moveSpeed = defaultMoveSpeed;
        rotationSpeed = defaultRotationSpeed;
        
        agent.enabled = true;
    }

    private void HandleOnFoodEaten()
    {
        float newDefaultMoveSpeed = defaultMoveSpeed - fishMovementConfigSO.MoveSpeedReduceAmountOnGrowth;
        defaultMoveSpeed = newDefaultMoveSpeed >= fishMovementConfigSO.MinMoveSpeed ? newDefaultMoveSpeed : defaultMoveSpeed;

        float newdefaultRotationSpeed = defaultRotationSpeed - fishMovementConfigSO.RotationSpeedReduceAmountOnGrowth;
        defaultRotationSpeed = newdefaultRotationSpeed >= fishMovementConfigSO.MinRotationSpeed ? newdefaultRotationSpeed : defaultRotationSpeed;

        float newMoveSpeedAtWarn = moveSpeedAtWarn - fishMovementConfigSO.MoveSpeedAtWarnReduceAmountOnGrowth;
        moveSpeedAtWarn = newMoveSpeedAtWarn >= fishMovementConfigSO.MinMoveSpeedAtWarn ? newMoveSpeedAtWarn : moveSpeedAtWarn;

        float newRotationSpeedAtWarn = defaultRotationSpeedAtWarn - fishMovementConfigSO.RotationSpeedAtWarnReduceAmountOnGrowth;
        rotationSpeedAtWarn = newRotationSpeedAtWarn >= fishMovementConfigSO.MinRotationSpeedAtWarn ? newRotationSpeedAtWarn : rotationSpeedAtWarn;
    }

    private void HandleOnDie()
    {
        agent.enabled = false;
    }

    private void HandleCurrentFoodDespawned()
    {
        moveSpeed = defaultMoveSpeed;
        rotationSpeed = defaultRotationSpeed;
    }

    private void HandleCurrentFoodSet()
    {
        IncreaseSpeed();
    }

    private void HandleFishWarned(Vector3 warnPosition)
    {
        if(Vector3.Distance(transform.position, warnPosition) >= minWarnDistance)
            return;
        
        Vector3 oppositeDirection = (model.transform.position - warnPosition).normalized;
        oppositeDirection *= fishMovementConfigSO.WarnPushMultiplier;

        oppositeDirection = TestAquarium.Instance.ClampPosition(oppositeDirection);
        fish.FishMovement.TargetPosition = oppositeDirection;

        fish.FishMovement.IncreaseSpeed();
    }

    public void IncreaseSpeed()
    {
        if(warnCoroutine != null)
        {
            StopCoroutine(warnCoroutine);
            moveSpeed = defaultMoveSpeed;
            rotationSpeed = defaultRotationSpeed;
        }
        
        moveSpeed = moveSpeedAtWarn;
        rotationSpeed = rotationSpeedAtWarn;

        float elapsedTime = 0f;
        float totalTime = fishMovementConfigSO.WarnSpeedFadeDuration;

        warnCoroutine =  StartCoroutine(Sequence());
        
        IEnumerator Sequence()
        {
            float startMoveSpeed = moveSpeed;
            float startRotationSpeed = rotationSpeed;
            while(elapsedTime <= totalTime)
            {
                elapsedTime += Time.deltaTime;

                moveSpeed = Mathf.Lerp(startMoveSpeed, defaultMoveSpeed, elapsedTime / totalTime);
                rotationSpeed = Mathf.Lerp(startRotationSpeed, defaultRotationSpeed, elapsedTime / totalTime);
                yield return null;
            }

            warnCoroutine = null;
        }
    }

    void Update()
    {
        agent.speed = moveSpeed;
        transform.eulerAngles = Vector3.zero;
    }

    public void MoveToTarget()
    {
        if(TargetPosition == null)
            return;

        agent.SetDestination(TargetPosition.Value);
        //transform.position = Vector3.MoveTowards(transform.position, TargetPosition.Value, moveSpeed * Time.deltaTime);
        Vector3 direction = TargetPosition.Value - transform.position;
        model.rotation = Quaternion.Lerp(model.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
    }

    public bool HasReachedTarget()
    {
        if(TargetPosition == null)
            return false;

        Vector3 fishPosition = transform.position;
        fishPosition.z = 0f; 

        Vector3 targetPosition = TargetPosition.Value;
        targetPosition.z = 0f; 
            
        return Vector3.Distance(fishPosition, targetPosition) <= 0.5f;
    }

    void OnDestroy()
    {
        fish.OnSpawned -= HandleOnSpawned;
        fish.OnDespawned -= HandleOnDespawned;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, minWarnDistance);
    }
}
