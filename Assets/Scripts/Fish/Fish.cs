using System;
using Lean.Pool;
using UnityEngine;
using UnityEngine.Events;

public class Fish : MonoBehaviour, IPoolable
{
    [field: SerializeField] public FishType FishType {get; private set;}
    [field: SerializeField] public FishInfoUI FishInfoUI {get; private set;} 

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

        FishGrowthController.OnHasBecomeMature += HandleBecomeMature;
    }

    private void HandleBecomeMature()
    {
        switch (FishType)
        {
            case FishType.Whale:
                break;
            case FishType.BigFish:
                DataManager.CurrentMoney += 30;
                break;
            case FishType.LittleFish:
                TestAquarium.Instance.SpawnFish(FishType, 3);
                break;
            case FishType.MediumFish:
                TestAquarium.Instance.SpawnFish(FishType, 1);
                break;
        }
    }

    private void SpawnFish(FishType type)
    {

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

public enum FishType
{
    Whale,
    BigFish,
    MediumFish,
    LittleFish,
}
