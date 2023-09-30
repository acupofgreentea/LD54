using System.Collections.Generic;
using Lean.Pool;
using UnityEngine;

public class TestAquarium : MonoBehaviour
{
    private  int HEIGHT = 10;
    private  int WIDTH = 15;

    [SerializeField] private  float padding = 1f;

    [SerializeField] private Fish fishPrefab;

    [SerializeField] private List<Fish> fishesInAquarium;

    public static TestAquarium Instance;
    void Awake()
    {
        if(Instance)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public Vector3 GetRandomPointInAquarium()
    {
        return new Vector3(Random.Range(-WIDTH/2 + padding, WIDTH/2 - padding), Random.Range(0, HEIGHT), 0f);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            var fish = LeanPool.Spawn(fishPrefab, GetRandomPointInAquarium(), Quaternion.identity);
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
