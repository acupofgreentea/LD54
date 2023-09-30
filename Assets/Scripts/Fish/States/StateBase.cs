public abstract class StateBase
{
    protected Fish fish;
    public StateBase(Fish fish)
    {
        this.fish = fish;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}