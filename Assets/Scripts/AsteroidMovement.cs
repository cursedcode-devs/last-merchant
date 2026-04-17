using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    public Asteroid[] prefabs;
    public int rows = 70;
    public int columns = 100;
    public float ellipseRadiusX = 20.0f;
    public float ellipseRadiusY = 15.5f;
    [Range(0f, 1f)]
    public float spawnChance = 0.75f;
    private Vector3 direction = Vector3.right;
    public float speed = 5f;

    private void Awake()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                float nx = (col / (float)(columns - 1)) * 2f - 1f;
                float ny = (row / (float)(rows - 1)) * 2f - 1f;

                if ((nx * nx) + (ny * ny) > 1f) continue;
                if (Random.value > spawnChance) continue;

                Vector3 position = new Vector3(
                    nx * ellipseRadiusX,
                    ny * ellipseRadiusY,
                    0f
                );

                int prefabIndex = Random.Range(0, prefabs.Length);
                Asteroid asteroid = Instantiate(prefabs[prefabIndex], this.transform);
                asteroid.transform.localPosition = position;
            }
        }
    }
    private void Update()
    {
        this.transform.position += direction * this.speed * Time.deltaTime;
    }
}