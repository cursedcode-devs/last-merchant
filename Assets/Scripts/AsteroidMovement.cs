using UnityEngine;
using System.Collections.Generic;

public class AsteroidMovement : MonoBehaviour
{
    public Asteroid[] prefabs;
    public int asteroidCount = 60;

    public float herdSpeed = 4f;
    public float spawnRadius = 1.5f;

    public EdgeCollider2D border;

    [Header("Progressive Spawning")]
    public float initialSpawnDelay = 10f;
    public float minSpawnDelay = 2f;
    public float spawnDelayDecreaseRate = 0.05f;
    public int maxAsteroidsOnScreen = 300;

    [HideInInspector] public float borderRadiusX;
    [HideInInspector] public float borderRadiusY;
    [HideInInspector] public Vector3 borderCenter;

    private List<Asteroid> asteroids = new List<Asteroid>();
    private float currentSpawnDelay;
    private float spawnTimer;

    void Awake()
    {
        Bounds bounds = new Bounds(border.transform.TransformPoint(border.points[0]), Vector3.zero);

        foreach (Vector2 point in border.points)
            bounds.Encapsulate(border.transform.TransformPoint(point));

        borderCenter = bounds.center;
        borderRadiusX = bounds.extents.x;
        borderRadiusY = bounds.extents.y;

        currentSpawnDelay = initialSpawnDelay;
        spawnTimer = 0f; // Od razu spawnujemy pierwszy rój na starcie
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.isDialogueActive) return;

        // Stopniowo zmniejszaj czas między spawnami rojów
        if (currentSpawnDelay > minSpawnDelay)
        {
            currentSpawnDelay -= spawnDelayDecreaseRate * Time.deltaTime;
            currentSpawnDelay = Mathf.Max(currentSpawnDelay, minSpawnDelay);
        }

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnHerd();
            spawnTimer = currentSpawnDelay;
        }
    }

    public void OnAsteroidDestroyed(Asteroid asteroid)
    {
        asteroids.Remove(asteroid);
    }

    void SpawnHerd()
    {
        if (asteroids.Count >= maxAsteroidsOnScreen)
            return;

        float angle = Random.Range(0f, Mathf.PI * 2f);

        Vector3 spawnCenter = new Vector3(
            borderCenter.x + Mathf.Cos(angle) * borderRadiusX,
            borderCenter.y + Mathf.Sin(angle) * borderRadiusY,
            0f
        );

        Vector3 inward = (borderCenter - spawnCenter).normalized;

        Vector3 herdVelocity = inward * herdSpeed;

        for (int i = 0; i < asteroidCount; i++)
        {
            int prefabIndex = Random.Range(0, prefabs.Length);

            Asteroid asteroid = Instantiate(prefabs[prefabIndex], transform);

            Vector3 offset = Random.insideUnitCircle * spawnRadius;

            asteroid.Init(this, spawnCenter + offset, herdVelocity);

            asteroids.Add(asteroid);
        }
    }
}