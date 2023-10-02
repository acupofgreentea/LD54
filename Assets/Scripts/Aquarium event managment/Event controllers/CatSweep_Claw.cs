using UnityEngine;

public class CatSweep_Claw : MonoBehaviour
{

    const string tag_fish = "Fish";
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tag_fish))
        {
            //Lean.Pool.LeanPool.Despawn(other.GetComponent<Fish>());
            other.GetComponent<FishHealth>().Die();
        }
    }
}
