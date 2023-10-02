using DG.Tweening;
using Lean.Pool;
using UnityEngine;
using UnityEngine.Events;

public class FishHealth : MonoBehaviour
{
    [SerializeField] private float moveDuration = 3f;
    public event UnityAction OnDie;
    private Fish fish;

    private Animator animator;
    public FishHealth Init(Fish fish)
    {
        this.fish = fish;
        animator = GetComponentInChildren<Animator>();

        return this;
    }

#if UNITY_EDITOR
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
            Die();
    }
#endif
    [ContextMenu("Die")]
    public void Die(bool willFall = true)
    {
        OnDie?.Invoke();
        animator.enabled = false;
        TestAquarium.Instance.RemoveFish(fish);

        if(willFall)
        {
            MoveToBottom();
        }
    }    

    private void MoveToBottom()
    {
        Vector3 targetPosition = transform.position;
        targetPosition.y = 0f;

        transform.DOMove(targetPosition, moveDuration).
        OnComplete(() => transform.DOScale(Vector3.zero, 0.45f).
        OnComplete(()=> LeanPool.Despawn(fish)));
    }
}
