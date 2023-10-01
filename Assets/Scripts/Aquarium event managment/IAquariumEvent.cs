
public interface IAquariumEvent
{
    public float Duration { get; set; }


    //**************************************


    void DependentItemPool(IAquariumEvent reference);


    void Happen();
}
