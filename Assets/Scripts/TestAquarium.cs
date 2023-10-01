using System.Collections.Generic;
using System.Linq;
using Lean.Pool;
using UnityEngine;
using UnityEngine.AI;

public class TestAquarium : MonoBehaviour
{
    private  int HEIGHT = 10;
    private  int WIDTH = 15;

    [SerializeField] private  float padding = 1f;

    [SerializeField] private Fish fishPrefab;

    [SerializeField] private List<Fish> fishesInAquarium;

    public static TestAquarium Instance;
    [SerializeField] private NavMeshSurface navMeshSurface;
    void Awake()
    {
        if(Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddFish(Fish fish)
    {
        if(fishesInAquarium.Contains(fish))
            return;

        fishesInAquarium.Add(fish);
    }

    public void RemoveFish(Fish fish)
    {
        if(!fishesInAquarium.Contains(fish))
            return;

        fishesInAquarium.Remove(fish);

    }

    public IEatable GetNearesEatable(Vector3 position)
    {
        if(fishesInAquarium.Count == 0)
        {
            return null;
        }
        List<IEatable> eatables = new();

        foreach (var fish in fishesInAquarium)
        {
            if(!fish.TryGetComponent(out IEatable eatable))
                continue;

            eatables.Add(eatable);
        }

        if(eatables.Count == 0)
        {
            Debug.LogError("there are no eatable fish in aquarium");
            return null;
        }

        IEatable nearest = eatables.OrderBy(x => Vector3.Distance(x.GameObject.transform.position, position)).First();
        return nearest;
    }

    public Vector3 GetRandomPointOnNavMesh()
    {
        NavMeshHit hit;
        Vector3 randomPoint = Vector3.zero;

        int maxAttempts = 30;
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            float randomX = Random.Range(navMeshSurface.transform.position.x - navMeshSurface.size.x / 2, navMeshSurface.transform.position.x + navMeshSurface.size.x / 2);
            float randomZ = Random.Range(navMeshSurface.transform.position.z - navMeshSurface.size.z / 2, navMeshSurface.transform.position.z + navMeshSurface.size.z / 2);

            randomPoint = new Vector3(randomX, navMeshSurface.transform.position.y, randomZ);

            if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
            {
                return hit.position;
            }

            attempts++;
        }

        Debug.LogWarning("Could not find a valid random point on NavMesh.");
        return randomPoint;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            var fish = LeanPool.Spawn(fishPrefab, GetRandomPointOnNavMesh(), Quaternion.identity);
            fishesInAquarium.Add(fish);
        }
    }

    public Vector3 ClampPosition(Vector3 targetPosition)
    {
        Vector3 clampedPosition = targetPosition;

        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -WIDTH/2 + padding, WIDTH/2 - padding);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, padding, HEIGHT - padding);
        //clampedPosition.z = Mathf.Clamp(clampedPosition.z, 0f, 0f); => fish state controller onworn method, we already change z position no need to clamp it.

        return clampedPosition;
    }
}
