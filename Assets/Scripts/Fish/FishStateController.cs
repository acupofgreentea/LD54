using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FishStateController : MonoBehaviour
{
    private Fish fish;

    private Dictionary<FishState, StateBase> stateDictionary;
    private StateBase currentState;

    public event UnityAction<Vector3> OnFishWarned;

    public FishStateController Init(Fish fish) 
    {
        this.fish = fish;
        CreateDictionary();
        fish.OnSpawned += HandleOnSpawned;
        fish.OnDespawned += HandleOnDespawned;

        return this;
    }

    private void HandleOnDespawned()
    {
        fish.FishHealth.OnDie -= HandleOnDie;
    }

    private void HandleOnSpawned()
    {
        PlayerActionHandler.OnWarnFishes += HandleWarn;
        fish.FishHealth.OnDie += HandleOnDie;
        ChangeState(FishState.Patrol);
    }

    private void HandleOnDie()
    {
        PlayerActionHandler.OnWarnFishes -= HandleWarn;
        ChangeState(FishState.Empty);
    }

    private void HandleWarn(Vector3 warnPosition)
    {
        ChangeState(FishState.MoveToTarget);
        OnFishWarned?.Invoke(warnPosition);
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
            {FishState.Empty, new EmptyState(fish)},
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
        fish.OnSpawned -= HandleOnSpawned;
        fish.OnDespawned -= HandleOnDespawned;
    }
}