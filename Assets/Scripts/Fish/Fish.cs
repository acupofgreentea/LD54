using System;
using Lean.Pool;
using UnityEngine;
using UnityEngine.Events;

public class Fish : MonoBehaviour, IPoolable
{
    public FishMovement FishMovement {get; private set;}
    public FishStateController FishStateController {get; private set;}
    public FishGrowthController FishGrowthController {get; private set;}
    public FishHealth FishHealth {get; private set;}
    
    public event UnityAction OnSpawned;
    public event UnityAction OnDespawned;

    void Awake()
    {
        FishMovement = GetComponent<FishMovement>().Init(this);
        FishStateController = GetComponent<FishStateController>().Init(this);
        FishGrowthController = GetComponent<FishGrowthController>().Init(this);
        FishHealth = GetComponent<FishHealth>().Init(this);
    }
    public void OnSpawn()
    {
        OnSpawned?.Invoke();
    }

    public void OnDespawn()
    {
        OnDespawned?.Invoke();
    }
}
