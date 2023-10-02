using DG.Tweening;
using Lean.Pool;
using UnityEngine;
using UnityEngine.Events;

public class FishHealth : MonoBehaviour
{
    [SerializeField] private float moveDuration = 3f;

    [SerializeField] private AudioSource dieAudio;
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
        dieAudio.Play();
        if(willFall)
        {
            MoveToBottom();
        }
        else
            TestAquarium.Instance.RemoveFish(fish);
    }    

    private void MoveToBottom()
    {
        Vector3 targetPosition = transform.position;
        targetPosition.y = -4f;

        transform.DOMove(targetPosition, moveDuration).
        OnComplete(() => transform.DOScale(Vector3.zero, 0.45f).
        OnComplete(()=> 
        {
            LeanPool.Despawn(fish);
            TestAquarium.Instance.RemoveFish(fish);
        }));
    }
}
