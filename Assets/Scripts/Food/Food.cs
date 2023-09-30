using System.Collections;
using Lean.Pool;
using UnityEngine;
using UnityEngine.Events;

public class Food : MonoBehaviour, IPoolable
{
    public static event UnityAction<Food> OnFoodDespawned;
    public static event UnityAction OnFoodSpawned;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!other.TryGetComponent(out FishGrowthController fish))
            return;

        if(fish.HasEatenRecently)
            return;

        fish.HandleFoodInteraction();
        LeanPool.Despawn(this);
    }

    public void OnSpawn()
    {
        rb.velocity = Vector3.zero;

        if(gameObject.activeInHierarchy) //temp
            LeanPool.Despawn(this, 3f);

        StartCoroutine(Delay());
        IEnumerator Delay()
        {
            yield return null;
            OnFoodSpawned?.Invoke();
        }
    }

    public void OnDespawn()
    {
        FoodManager.Instance.RemoveFood(this);
        OnFoodDespawned?.Invoke(this);
    }
}
