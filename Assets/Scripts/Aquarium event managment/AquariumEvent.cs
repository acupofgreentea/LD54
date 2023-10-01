using UnityEngine;

[CreateAssetMenu(fileName = "_AE", menuName = "ScriptableObjects/Aquarium event")]
public class AquariumEvent : ScriptableObject
{
    public string Name;
    public Sprite Pic;
    public int afterTime;
    public IAquariumEvent theEvent { get { return _theEvent as IAquariumEvent; } set { _theEvent = value as MonoBehaviour; } }
    [SerializeField]    private MonoBehaviour _theEvent;

    //public float Prize;
    //public float PrizeForEachEarly;


    //*************************************


    public void TheHappening()
    {
        (Lean.Pool.LeanPool.Spawn<MonoBehaviour>(theEvent as MonoBehaviour) as IAquariumEvent).Happen();

    }
}
