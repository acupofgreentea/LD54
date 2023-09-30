using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;
using System.Linq;

public class FoodManager : MonoBehaviour
{
    [SerializeField] private Food foodPrefab;

    [SerializeField] private List<Food> activeFoods = new();

    public static FoodManager Instance;


    private void Awake()
    {
        if(Instance)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        PlayerActionHandler.OnFeed += HandleFeed;
    }

    private void HandleFeed(Vector3 spawnPosition)
    {
        var food = LeanPool.Spawn(foodPrefab, spawnPosition, Random.rotation);
        activeFoods.Add(food);
    }

    public bool TryGetAvailableFood(out Food food)
    {
        food = null;
        if(activeFoods.Count == 0)
            return false;

        food = activeFoods.GetRandom();

        return true;
    }

    public Food GetClosestFood(Vector3 fishPosition)
    {
        return activeFoods.OrderBy(x => Vector3.Distance(x.transform.position, fishPosition)).FirstOrDefault();
    }

    public Vector3? GetFoodPosition()
    {
        if(activeFoods.Count == 0)
            return null;

        return activeFoods.GetRandom().transform.position;
    }

    public void RemoveFood(Food food)
    {
        if(!activeFoods.Contains(food))
            return;

        activeFoods.Remove(food);
    }

    void OnDestroy()
    {
        PlayerActionHandler.OnFeed -= HandleFeed;
    }
}
