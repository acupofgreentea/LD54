using UnityEngine;
using UnityEngine.UI;

public class EventOnUI : MonoBehaviour
{
    public Image Pic;
    public Text CountDown;
    public RectTransform _transform;
    public AquariumEventsManager.ListedEvent indexed;


    //********************************


    public void EarlyEvent()
    {
        ReferenceManager.aquariumEventsManager.UseOfAnEvent(indexed);
    }
}
