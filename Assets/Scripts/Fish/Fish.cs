using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fish : MonoBehaviour
{
    public FishMovement FishMovement {get; private set;}
    public FishStateController FishStateController {get; private set;}

    public Food CurrentFood {get; set;}

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
        if(CurrentFood.gameObject.GetInstanceID() == food.gameObject.GetInstanceID())
        {
            CurrentFood = null;
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

    private void HandleFoodSpawned(Food food)
    {
        if(CurrentFood == null)
        {
            CurrentFood = food;
        }
        else //if fish has current food and player spawns another food, fish might change the currentFood so some fishes go to other food.
        {
            int randomChance = Random.Range(0, 100);

            if(randomChance >= 80)
            {
                CurrentFood = food;
            }
        }
    }
}
