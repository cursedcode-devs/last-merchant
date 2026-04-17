using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Sprite[] animationSprites;
    public float animationSpeed = 0.1f;

    private SpriteRenderer spriteRenderer;
    private int animationFrame;

    private AsteroidMovement spawner;

    private Vector3 localOffset;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(AsteroidMovement spawnerRef, Vector3 startPos)
    {
        spawner = spawnerRef;

        transform.position = startPos;

        localOffset = startPos - spawner.transform.position;

        InvokeRepeating(nameof(Animate), animationSpeed, animationSpeed);
    }

    void Update()
    {
        transform.position += spawner.herdVelocity * Time.deltaTime;

        Vector3 pos = transform.position - spawner.borderCenter;

        float ex = pos.x / spawner.borderRadiusX;
        float ey = pos.y / spawner.borderRadiusY;

        if ((ex * ex) + (ey * ey) >= 1f)
        {
            Destroy(gameObject);
        }
    }

    void Animate()
    {
        if (animationSprites.Length == 0) return;

        animationFrame = (animationFrame + 1) % animationSprites.Length;
        spriteRenderer.sprite = animationSprites[animationFrame];
    }
}