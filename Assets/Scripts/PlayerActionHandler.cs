using Lean.Pool;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerActionHandler : MonoBehaviour
{
    public static event UnityAction<Vector3> OnFeed;    
    public static event UnityAction<Vector3> OnWarnFishes;

    [SerializeField] private ParticleSystem splashParticle;

    [SerializeField] private LayerMask hitLayer;

    private Camera cam;
    private bool acceptInput = true;

    void Awake()
    {
        cam = Camera.main;
    }

    void Start()
    {
        AquariumEventsManager.AllEventsCompleted += HandleLevelFinish;
        TestAquarium.OnAquariumIsEmpty += HandleLevelFinish;
    }

    private void HandleLevelFinish()
    {
        acceptInput = false;
    }

    void Update()
    {
        if(!acceptInput)
            return;
            
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if(!Physics.Raycast(ray, out var hit, Mathf.Infinity, hitLayer))
            return;

        Vector3 worldPosition = hit.point;   
        worldPosition.z = 0f;     

        if(Input.GetMouseButtonDown(0))
        {
            OnWarnFishes?.Invoke(worldPosition);
            var splash = LeanPool.Spawn(splashParticle);   
            splash.transform.position = hit.point;    
            splash.Play();
            LeanPool.Despawn(splash, 1f);
        }

        if(Input.GetMouseButtonDown(1))
        {
            if(DataManager.CurrentMoney <= 0)
                return;
            
            DataManager.CurrentMoney -= 1;
            OnFeed?.Invoke(hit.point);
        }
    }

    void OnDestroy()
    {
        AquariumEventsManager.AllEventsCompleted -= HandleLevelFinish;
        TestAquarium.OnAquariumIsEmpty -= HandleLevelFinish;
    }

}
