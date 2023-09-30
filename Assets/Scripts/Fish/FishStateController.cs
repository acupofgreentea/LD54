using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FishStateController : MonoBehaviour
{
    private Fish fish;

    private Dictionary<FishState, StateBase> stateDictionary;
    private StateBase currentState;
    [SerializeField] private float minWarnDistance = 3f;

    public event UnityAction<Vector3> OnFishWarned;

    public FishStateController Init(Fish fish) 
    {
        this.fish = fish;
        CreateDictionary();

        PlayerActionHandler.OnWarnFishes += HandleWarn;
        return this;
    }

    private IEnumerator Start()
    {
        yield return null;
        ChangeState(FishState.Patrol);
    }

    private void HandleWarn(Vector3 foodPosition)
    {
        if(Vector3.Distance(transform.position, foodPosition) >= minWarnDistance)
            return;
        
        foodPosition.z = transform.position.z;
        Vector3 oppositeDirection = (transform.position - foodPosition).normalized;
        oppositeDirection *= 10f;//temp, move to so

        oppositeDirection = TestAquarium.Instance.ClampPosition(oppositeDirection);
        ChangeState(FishState.MoveToTarget);
        OnFishWarned?.Invoke(oppositeDirection);
    }

    void Update()
    {
        currentState?.UpdateState();
    }

    private void CreateDictionary()
    {
        stateDictionary = new()
        {
            {FishState.Patrol, new PatrolState(fish)},
            {FishState.MoveToTarget, new MoveToTargetState(fish)},
        };
    }

    public void ChangeState(FishState nextState)
    {
        currentState?.ExitState();
        currentState = stateDictionary[nextState];
        currentState.EnterState();
    }

    void OnDestroy()
    {
        PlayerActionHandler.OnWarnFishes -= HandleWarn;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, minWarnDistance);
    }
}