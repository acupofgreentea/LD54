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
        if(fish.CurrentFood != null)
        {
            fish.FishMovement.TargetPosition = fish.CurrentFood.transform.position;
        }
        
        fish.FishMovement.MoveToTarget();

        if(fish.FishMovement.HasReachedTarget())
        {
            fish.FishStateController.ChangeState(FishState.Patrol);
        }
    }

    public override void ExitState()
    {

    }

}
