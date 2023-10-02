public class MoveToTargetState : StateBase
{
    public MoveToTargetState(Fish fish) : base(fish)
    {

    }

    public override void EnterState()
    {

    }
    
    public override void UpdateState()
    {
        if(fish.FishGrowthController.CurrentFood != null)
        {
            fish.FishMovement.TargetPosition = fish.FishGrowthController.CurrentFood.GameObject.transform.position;
        }

        fish.FishMovement.MoveToTarget();

        if(fish.FishMovement.HasReachedTarget() || !fish.FishMovement.HasPath)
        {
            fish.FishStateController.ChangeState(FishState.Patrol);
        }
    }

    public override void ExitState()
    {

    }

}
