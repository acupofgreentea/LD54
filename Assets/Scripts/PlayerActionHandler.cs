using UnityEngine;
using UnityEngine.Events;

public class PlayerActionHandler : MonoBehaviour
{
    public static event UnityAction<Vector3> OnFeed;    
    public static event UnityAction<Vector3> OnWarnFishes;

    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if(!Physics.Raycast(ray, out var hit))
            return;

        Vector3 worldPosition = hit.point;        
        worldPosition.z = 0f;

        if(Input.GetMouseButtonDown(0))
        {
            OnWarnFishes?.Invoke(worldPosition);       
        }

        if(Input.GetMouseButtonDown(1))
        {
            OnFeed?.Invoke(worldPosition);
        }
    }

}
