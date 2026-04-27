using UnityEngine;
using System.Collections.Generic;

public class ResourceClusterSpawner : MonoBehaviour
{
    [Header("Assign In Inspector")]
    [SerializeField] private EdgeCollider2D playArea;
    [SerializeField] private ResourcePickup[] pickupPrefabs;

    [Header("Spawn Settings")]
    [SerializeField] private int spawnCount = 5;
    [SerializeField] private float pickupSpacing = 2f;
    [SerializeField] private int maxAttempts = 50;

    [Header("Block Layers")]
    [SerializeField] private LayerMask blockedLayers;
    [SerializeField] private float blockedRadius = 1.2f;

    private Bounds areaBounds;
    private List<ResourcePickup> spawned = new List<ResourcePickup>();

    void Start()
    {
        BuildBounds();
        SpawnAll();
    }

    void BuildBounds()
    {
        Vector3 first = playArea.transform.TransformPoint(playArea.points[0]);
        areaBounds = new Bounds(first, Vector3.zero);

        foreach (Vector2 p in playArea.points)
            areaBounds.Encapsulate(playArea.transform.TransformPoint(p));
    }

    void SpawnAll()
    {
        for (int i = 0; i < spawnCount; i++)
            SpawnOne();
    }

    public void SpawnOne()
    {
        Vector2 pos;

        if (!FindSpawnPoint(out pos))
            return;

        if (pickupPrefabs == null || pickupPrefabs.Length == 0) return;
        
        ResourcePickup prefab = pickupPrefabs[Random.Range(0, pickupPrefabs.Length)];
        ResourcePickup obj =
            Instantiate(prefab, pos, Quaternion.identity, transform);

        obj.Setup(this);
        spawned.Add(obj);
    }

    bool FindSpawnPoint(out Vector2 point)
    {
        // Reduce the radius by 15% so pickups do not spawn on the edge
        float radius = Mathf.Min(areaBounds.extents.x, areaBounds.extents.y) * 0.85f;
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 p = (Vector2)areaBounds.center + Random.insideUnitCircle * radius;

            if (Physics2D.OverlapCircle(p, blockedRadius, blockedLayers))
                continue;

            bool nearOther = false;

            foreach (var item in spawned)
            {
                if (item == null) continue;

                if (Vector2.Distance(p, item.transform.position) < pickupSpacing)
                {
                    nearOther = true;
                    break;
                }
            }

            if (nearOther) continue;

            point = p;
            return true;
        }

        point = Vector2.zero;
        return false;
    }

    public void Collected(ResourcePickup pickup)
    {
        spawned.Remove(pickup);
        Destroy(pickup.gameObject);

        SpawnOne();
    }
}