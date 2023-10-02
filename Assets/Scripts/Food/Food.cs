using System.Collections;
using Lean.Pool;
using UnityEngine;
using UnityEngine.Events;

public class Food : MonoBehaviour, IPoolable, IEatable
{
    [SerializeField] private float torqueForceAtSpawn = 15f;
    public static event UnityAction<Food> OnFoodDespawned;
    public static event UnityAction OnFoodSpawned;

    private Rigidbody rb;

    public GameObject GameObject => this.gameObject;

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

        if(fish.HasStoppedEatingFood)
            return;

        fish.HandleFoodInteraction();
        LeanPool.Despawn(this);
    }
    
    void Update()
    {
        if(transform.position.y <= -4f)
            LeanPool.Despawn(this);
    }

    public void OnSpawn()
    {
        rb.velocity = Vector3.zero;

        StartCoroutine(Delay());
        IEnumerator Delay()
        {
            yield return null;
            OnFoodSpawned?.Invoke();
        }

        rb.AddTorque(new Vector3(Random.value, Random.value, Random.value) * torqueForceAtSpawn);
    }

    public void OnDespawn()
    {
        FoodManager.Instance.RemoveFood(this);
        OnFoodDespawned?.Invoke(this);
    }
}
