using Lean.Pool;
using UnityEngine;
using UnityEngine.Events;

public class PlayerActionHandler : MonoBehaviour
{
    public static event UnityAction<Vector3> OnFeed;    
    public static event UnityAction<Vector3> OnWarnFishes;

    [SerializeField] private ParticleSystem splashParticle;

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
            var splash = LeanPool.Spawn(splashParticle);   
            splash.transform.position = worldPosition;    
            splash.Play();
            LeanPool.Despawn(splash, 1f);
        }

        if(Input.GetMouseButtonDown(1))
        {
            OnFeed?.Invoke(worldPosition);
        }
    }

}
