using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class FishGrowthController : MonoBehaviour
{
    [SerializeField] protected FishGrowthConfigSO fishGrowthConfigSO;

    protected Fish fish;
    private float currentGrowthAmount = 1f;
    protected float lastEatTime;

    private float growthAmountPerFood;

    public IEatable CurrentFood {get; set;}
    public  UnityAction OnCurrentFoodSet; 
    public event UnityAction OnCurrentFoodDespawned; 
    public event UnityAction OnFoodEaten;
    public event UnityAction OnHasBecomeMature;

    public bool HasEatenRecently {get; protected set;} = false;
    public bool HasReachedMature => currentGrowthAmount >= fishGrowthConfigSO.MatureGrowthAmount;
    public bool HasStoppedEatingFood => HasReachedMature && fishGrowthConfigSO.StopEatingFoodAtMature;

    protected bool isCurrentlyMature = false;

    public FishGrowthController Init(Fish fish)
    {
        this.fish = fish;
        fish.OnSpawned += HandleOnSpawned;
        fish.OnDespawned += HandleOnDespawned;
        return this;
    }

    protected virtual void Update()
    {
        if(HasReachedMature && fishGrowthConfigSO.StopEatingFoodAtMature)
            return;
        
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

    protected void HandleFoodEaten()
    {
        currentGrowthAmount += growthAmountPerFood;

        if(HasReachedMature && !isCurrentlyMature)
        {
            isCurrentlyMature = true;
            OnHasBecomeMature?.Invoke();
            growthAmountPerFood = fishGrowthConfigSO.GrowthAmountAtMature;
        }

        Vector3 nextScale  = transform.localScale + Vector3.one * growthAmountPerFood; 
        transform.DOScale(nextScale, fishGrowthConfigSO.GrowthScaleDuration);

        HasEatenRecently = true;
        lastEatTime = Time.time;
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
        Food.OnFoodSpawned -= HandleFoodSpawned;
        Food.OnFoodDespawned -= HandleFoodDespawned;
    }

    private void HandleFoodDespawned(Food food)
    {
        // when food is despawned event will called, then all the fishes will check if the their current food is despawned, checking by getinstance id is faster because of int
        if(CurrentFood != null)
        {
            if(CurrentFood.GameObject.GetInstanceID() == food.gameObject.GetInstanceID())
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

    protected virtual void SetFood()
    {
        Food _food = FoodManager.Instance.GetClosestFood(transform.position);

        if(_food == null)
        {
            fish.FishStateController.ChangeState(FishState.Patrol);

            Debug.LogError("state changed to patrol. there is no food");
            return;
        }

        CurrentFood = _food;
    }


    protected void HandleFoodSpawned()
    {
        SetFood();
        fish.FishStateController.ChangeState(FishState.MoveToTarget);
        fish.FishMovement.TargetPosition = CurrentFood.GameObject.transform.position;
        OnCurrentFoodSet?.Invoke();
    }

}
