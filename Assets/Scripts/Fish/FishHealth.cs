using DG.Tweening;
using Lean.Pool;
using UnityEngine;
using UnityEngine.Events;

public class FishHealth : MonoBehaviour
{
    [SerializeField] private float moveDuration = 3f;
    public event UnityAction OnDie;
    private Fish fish;

    public FishHealth Init(Fish fish)
    {
        this.fish = fish;

        return this;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
            Die();
    }

    [ContextMenu("Die")]
    private void Die()
    {
        OnDie?.Invoke();
        Vector3 targetPosition = transform.position;
        targetPosition.y = 0f;

        transform.DOMove(targetPosition, moveDuration).
        OnComplete(() => transform.DOScale(Vector3.zero, 0.45f).
        OnComplete(()=> LeanPool.Despawn(fish)));
    }    
}
