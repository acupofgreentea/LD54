using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FishMovement : MonoBehaviour
{
    [SerializeField] private FishMovementConfigSO fishMovementConfigSO;

    [SerializeField] private Transform model;
    private float moveSpeed;
    private float rotationSpeed;

    public Vector3? TargetPosition {get; set;}
    private Fish fish;

    private float defaultMoveSpeed;
    private float defaultRotationSpeed;

    private Coroutine warnCoroutine;

    private NavMeshAgent agent;
    public FishMovement Init(Fish fish)
    {
        this.fish = fish;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        moveSpeed = fishMovementConfigSO.MoveSpeed;
        rotationSpeed = fishMovementConfigSO.RotationSpeed;
        defaultMoveSpeed = moveSpeed;
        defaultRotationSpeed = rotationSpeed;

        return this;
    }
    void Start()
    {
        fish.FishStateController.OnFishWarned += HandleFishWarned;
        fish.OnCurrentFoodSet += HandleCurrentFoodSet;
        fish.OnCurrentFoodDespawned += HandleCurrentFoodDespawned;
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

    private void HandleFishWarned(Vector3 oppositeDirection)
    {
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
        
        moveSpeed = fishMovementConfigSO.MoveSpeedAtWarn;
        rotationSpeed = fishMovementConfigSO.RotationSpeedAtWarn;

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
        fish.FishStateController.OnFishWarned -= HandleFishWarned;
        fish.OnCurrentFoodSet -= HandleCurrentFoodSet;
        fish.OnCurrentFoodDespawned -= HandleCurrentFoodDespawned;
    }
}
