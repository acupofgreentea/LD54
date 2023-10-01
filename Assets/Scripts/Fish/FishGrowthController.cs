using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class FishGrowthController : MonoBehaviour
{
    [SerializeField] private FishGrowthConfigSO fishGrowthConfigSO;

    private Fish fish;
    private float currentGrowthAmount = 1f;
    private float lastEatTime;

    private float growthAmountPerFood;

    public Food CurrentFood {get; set;}
    public event UnityAction OnCurrentFoodSet; 
    public event UnityAction OnCurrentFoodDespawned; 
    public event UnityAction OnFoodEaten;
    public event UnityAction OnHasBecomeMature;

    public bool HasEatenRecently {get; private set;} = false;
    public bool HasReachedMature => currentGrowthAmount >= fishGrowthConfigSO.MatureGrowthAmount;

    public FishGrowthController Init(Fish fish)
    {
        this.fish = fish;
        fish.OnSpawned += HandleOnSpawned;
        fish.OnDespawned += HandleOnDespawned;
        return this;
    }

    private void Update()
    {
        if(!HasEatenRecently)
            return;
        
        if(Time.time >= lastEatTime + fishGrowthConfigSO.FullnessDuration)
        {
            HasEatenRecently = false;
            Food.OnFoodSpawned += HandleFoodSpawned;
            Food.OnFoodDespawned += HandleFoodDespawned;
        }
    }

    private void HandleOnSpawned()
    {
        Food.OnFoodSpawned += HandleFoodSpawned;
        Food.OnFoodDespawned += HandleFoodDespawned;
        HasEatenRecently = false;
        currentGrowthAmount = 1f;
        transform.localScale = Vector3.one;
        growthAmountPerFood = fishGrowthConfigSO.GrowthAmount;
    }

    private void HandleOnDespawned()
    {
        Food.OnFoodSpawned -= HandleFoodSpawned;
        Food.OnFoodDespawned -= HandleFoodDespawned;
    }

    private void HandleFoodEaten()
    {
        currentGrowthAmount += growthAmountPerFood;

        if(HasReachedMature)
        {
            OnHasBecomeMature?.Invoke();
            growthAmountPerFood = fishGrowthConfigSO.GrowthAmountAtMature;
        }

        Vector3 nextScale  = transform.localScale + Vector3.one * growthAmountPerFood; 
        transform.DOScale(nextScale, fishGrowthConfigSO.GrowthScaleDuration);

        HasEatenRecently = true;
        lastEatTime = Time.time;
        Food.OnFoodSpawned -= HandleFoodSpawned;
        Food.OnFoodDespawned -= HandleFoodDespawned;
        CurrentFood = null;
    }

    private void OnDestroy()
    {
        fish.OnSpawned -= HandleOnSpawned;
        fish.OnDespawned -= HandleOnDespawned;
    }

    public void HandleFoodInteraction()
    {
        OnFoodEaten?.Invoke();
        HandleFoodEaten();
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
            fish.FishStateController.ChangeState(FishState.MoveToTarget);
        }
        else
            fish.FishStateController.ChangeState(FishState.Patrol);
    }

    private void HandleFoodSpawned()
    {
        Food _food = FoodManager.Instance.GetClosestFood(transform.position);

        if(_food == null)
        {
            fish.FishStateController.ChangeState(FishState.Patrol);

            Debug.LogError("state changed to patrol. there is no food");
            return;
        }

        CurrentFood = _food;
        fish.FishStateController.ChangeState(FishState.MoveToTarget);
        fish.FishMovement.TargetPosition = CurrentFood.transform.position;
        OnCurrentFoodSet?.Invoke();
    }

}
