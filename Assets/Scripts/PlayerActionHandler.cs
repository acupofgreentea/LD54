using Lean.Pool;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerActionHandler : MonoBehaviour
{
    public static event UnityAction<Vector3> OnFeed;    
    public static event UnityAction<Vector3> OnWarnFishes;

    [SerializeField] private ParticleSystem splashParticle;

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

        if(!Physics.Raycast(ray, out var hit))
            return;

        Vector3 worldPosition = hit.point;        

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
            if(DataManager.CurrentMoney <= 0)
                return;
            
            DataManager.CurrentMoney -= 1;
            OnFeed?.Invoke(worldPosition);
        }
    }

    void OnDestroy()
    {
        AquariumEventsManager.AllEventsCompleted -= HandleLevelFinish;
        TestAquarium.OnAquariumIsEmpty -= HandleLevelFinish;
    }

}
