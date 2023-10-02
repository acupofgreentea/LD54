using Lean.Pool;
using UnityEngine;

public class MiniWhaleGrowthController : FishGrowthController
{
    protected override void Update()
    {
        if(HasReachedMature)
        {
            if(!HasEatenRecently)
                return;
        
            if(Time.time >= lastEatTime + fishGrowthConfigSO.FullnessDuration)
            {
                FindFish();
            }
        }
        else
        {
            base.Update();
        }
    }

    private void FindFish()
    {
        SetFood();
        if(CurrentFood == null)
            return;

        fish.FishStateController.ChangeState(FishState.MoveToTarget);
        fish.FishMovement.TargetPosition = CurrentFood.GameObject.transform.position;
        OnCurrentFoodSet?.Invoke();
        HasEatenRecently = false;
    }

    protected override void SetFood()
    {
        if(HasReachedMature)
        {
            CurrentFood = TestAquarium.Instance.GetNearesEatable(transform.position);
        }
        else
            base.SetFood();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!HasReachedMature)
            return;
        
        if(HasEatenRecently)
            return;
        
        if(!other.TryGetComponent(out FishHealth _fish))
            return;
        
        _fish.Die(false);
        HandleFoodEaten();
        CurrentFood = null;
        LeanPool.Despawn(_fish);
    }
} 