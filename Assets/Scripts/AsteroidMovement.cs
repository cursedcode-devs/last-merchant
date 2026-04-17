using UnityEngine;
using System.Collections.Generic;

public class AsteroidMovement : MonoBehaviour
{
    public Asteroid[] prefabs;
    public int asteroidCount = 60;

    public float herdSpeed = 4f;
    public float spawnRadius = 1.5f;

    public EdgeCollider2D border;

    [HideInInspector] public float borderRadiusX;
    [HideInInspector] public float borderRadiusY;
    [HideInInspector] public Vector3 borderCenter;

    [HideInInspector] public Vector3 herdVelocity;

    private List<Asteroid> asteroids = new List<Asteroid>();

    void Awake()
    {
        Bounds bounds = new Bounds(border.transform.TransformPoint(border.points[0]), Vector3.zero);

        foreach (Vector2 point in border.points)
            bounds.Encapsulate(border.transform.TransformPoint(point));

        borderCenter = bounds.center;
        borderRadiusX = bounds.extents.x;
        borderRadiusY = bounds.extents.y;

        SpawnHerd();
    }

    public void OnAsteroidDestroyed(Asteroid asteroid)
    {
        asteroids.Remove(asteroid);

        if (asteroids.Count == 0)
            SpawnHerd();
    }

    void SpawnHerd()
    {
        asteroids.Clear();

        float angle = Random.Range(0f, Mathf.PI * 2f);

        Vector3 spawnCenter = new Vector3(
            borderCenter.x + Mathf.Cos(angle) * borderRadiusX,
            borderCenter.y + Mathf.Sin(angle) * borderRadiusY,
            0f
        );

        Vector3 inward = (borderCenter - spawnCenter).normalized;

        herdVelocity = inward * herdSpeed;

        for (int i = 0; i < asteroidCount; i++)
        {
            int prefabIndex = Random.Range(0, prefabs.Length);

            Asteroid asteroid = Instantiate(prefabs[prefabIndex], transform);

            Vector3 offset = Random.insideUnitCircle * spawnRadius;

            asteroid.Init(this, spawnCenter + offset);

            asteroids.Add(asteroid);
        }
    }
}