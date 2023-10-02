using UnityEngine;

public class CatSweep_Claw : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out FishHealth fish))
        {
            //Lean.Pool.LeanPool.Despawn(other.GetComponent<Fish>());
            fish.Die();
        }
    }
}
