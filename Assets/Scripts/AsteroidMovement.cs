using UnityEngine;

public class AsteroidMovement : MonoBehaviour
{
    public Asteroid[] prefabs;
    public int rows = 5;
    public int columns = 3;
    private void Awake()
    {
        for (int row = 0; row < rows; row++)
        {
            float width = 2.0f * (this.columns - 1);
            float height = 2.0f * (this.rows - 1);
            Vector2 centering = new Vector2(-width / 2, -height / 2);
            Vector3 rowPosition = new Vector3(centering.x,centering.y + (row * 2.0f), 0);
            for (int col = 0; col < columns; col++)
            {
                Asteroid asteroid = Instantiate(this.prefabs[row],this.transform);
                Vector3 position = rowPosition;
                position.x += col * 2.0f;
                asteroid.transform.localPosition = position;
            }
        }
    }
}
