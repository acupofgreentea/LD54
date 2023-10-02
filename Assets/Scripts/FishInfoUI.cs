using UnityEngine;

[CreateAssetMenu(menuName = "FishInfoUI")]
public class FishInfoUI : ScriptableObject
{
    [field : SerializeField] public Sprite FishImage {get; private set;}
    [field : SerializeField] public string FishName {get; private set;}
}
