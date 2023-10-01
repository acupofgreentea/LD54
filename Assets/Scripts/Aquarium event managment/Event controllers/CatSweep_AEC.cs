using UnityEngine;

public class CatSweep_AEC : MonoBehaviour, IAquariumEvent
{
    #region Interfaced
    public float Duration { get { return _duration; } set { _duration = value; } }
    #endregion
    #region Serialized
    [SerializeField] private float _duration;
    #endregion

    [Header("References")]
    public Animator Paw;
    public Transform PawT;


    //************************


    public void DependentItemPool(IAquariumEvent reference)
    {
        
    }


    const string trig_sweep = "sweep";
    float currentDuration;
    public void Happen()
    {
        Paw.SetTrigger(trig_sweep);

        currentDuration = Duration;
    }


    private void Update()
    {
        if (currentDuration > 0)
        {
            currentDuration -= Time.deltaTime;
            if (currentDuration <= 0)
            {
                currentDuration = 0;

                //ardýndan despawn ol
                Lean.Pool.LeanPool.Despawn(this);

            }
        }
    }

}
