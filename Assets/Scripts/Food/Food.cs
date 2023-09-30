using Lean.Pool;
using UnityEngine;
using UnityEngine.Events;

public class Food : MonoBehaviour, IPoolable
{
    public static event UnityAction<Food> OnFoodDespawned;
    public static event UnityAction<Food> OnFoodSpawned;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!other.TryGetComponent(out Fish fish))
            return;

        LeanPool.Despawn(this);
    }

    public void OnSpawn()
    {
        rb.velocity = Vector3.zero;
        OnFoodSpawned?.Invoke(this);
        if(gameObject.activeInHierarchy) //temp
            LeanPool.Despawn(this, 3f);
    }

    public void OnDespawn()
    {
        FoodManager.Instance.RemoveFood(this);
        OnFoodDespawned?.Invoke(this);
    }
}
