public class PatrolState : StateBase
{
    public PatrolState(Fish fish) : base(fish)
    {
    }

    private void SetNewTargetPosition()
    {
        fish.FishMovement.TargetPosition = TestAquarium.Instance.GetRandomPointOnNavMesh();
    }

    public override void EnterState()
    {
        SetNewTargetPosition();
    }

    public override void UpdateState()
    {
        if(fish.FishMovement.HasReachedTarget() || !fish.FishMovement.HasPath)
        {
            SetNewTargetPosition();
        }

        fish.FishMovement.MoveToTarget();
    }

    public override void ExitState()
    {
        fish.FishMovement.TargetPosition = null;
    }

}