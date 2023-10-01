using UnityEngine;

public class ReferenceManager : MonoBehaviour
{
    #region References
    public static AquariumEventsManager aquariumEventsManager;
    #endregion
    #region VariableAttachments
    public AquariumEventsManager _aquariumEventsManager;
    #endregion


    //**********************************


    private void Awake()
    {
        aquariumEventsManager = _aquariumEventsManager;
    }
}
