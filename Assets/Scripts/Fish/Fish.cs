using System;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Fish : MonoBehaviour
{
    public FishMovement FishMovement {get; private set;}
    public FishStateController FishStateController {get; private set;}

    public Food CurrentFood {get; set;}

    public event UnityAction OnCurrentFoodSet; 
    public event UnityAction OnCurrentFoodDespawned; 

    void Awake()
    {
        FishMovement = GetComponent<FishMovement>().Init(this);
        FishStateController = GetComponent<FishStateController>().Init(this);
    }

    void Start()
    {
        Food.OnFoodSpawned += HandleFoodSpawned;
        Food.OnFoodDespawned += HandleFoodDespawned;
    }

    private void HandleFoodDespawned(Food food)
    {
        // when food is despawned event will called, then all the fishes will check if the their current food is despawned, checking by getinstance id is faster because of int
        if(CurrentFood != null)
        {
            if(CurrentFood.gameObject.GetInstanceID() == food.gameObject.GetInstanceID())
            {
                CurrentFood = null;
                OnCurrentFoodDespawned?.Invoke();
            }
        }

        if(FoodManager.Instance.TryGetAvailableFood(out Food availableFood))
        {
            CurrentFood = availableFood;
            FishStateController.ChangeState(FishState.MoveToTarget);
        }
        else
            FishStateController.ChangeState(FishState.Patrol);
    }

    void OnDestroy()
    {
        Food.OnFoodSpawned -= HandleFoodSpawned;
        Food.OnFoodDespawned -= HandleFoodDespawned;
    }

    private void HandleFoodSpawned()
    {
        Food _food = FoodManager.Instance.GetClosestFood(transform.position);

        if(_food == null)
        {
            FishStateController.ChangeState(FishState.Patrol);

            Debug.LogError("state changed to patrol. there is no food");
            return;
        }

        CurrentFood = _food;
        FishStateController.ChangeState(FishState.MoveToTarget);
        FishMovement.TargetPosition = CurrentFood.transform.position;
        OnCurrentFoodSet?.Invoke();
    }
}
