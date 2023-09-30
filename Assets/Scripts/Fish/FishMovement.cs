using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float rotationSpeed = 2f;

    public Vector3? TargetPosition {get; set;}
    private Fish fish;
    public FishMovement Init(Fish fish)
    {
        this.fish = fish;

        return this;
    }

    public void MoveToTarget()
    {
        if(TargetPosition == null)
            return;
        
        transform.position = Vector3.MoveTowards(transform.position, TargetPosition.Value, moveSpeed * Time.deltaTime);
        Vector3 direction = TargetPosition.Value - transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
    }

    public bool HasReachedTarget()
    {
        if(TargetPosition == null)
            return false;
            
        return Vector3.Distance(transform.position, TargetPosition.Value) <= 0.5f;
    }
}
