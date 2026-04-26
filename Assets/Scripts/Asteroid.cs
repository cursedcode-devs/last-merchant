using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Sprite[] animationSprites;
    public float animationSpeed = 0.1f;

    [Header("Damage")]
    public int damageAmount = 10;
    [Tooltip("Minimalny odst\u0119p (w sekundach) mi\u0119dzy kolejnymi obra\u017ceniami od tego samego asteroidu")]
    public float damageCooldown = 1f;

    private SpriteRenderer spriteRenderer;
    private int animationFrame;
    private AsteroidMovement spawner;
    private Vector3 localOffset;
    private float lastDamageTime = -999f;
    private Vector3 currentVelocity;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Init(AsteroidMovement spawnerRef, Vector3 startPos, Vector3 velocity)
    {
        spawner = spawnerRef;
        transform.position = startPos;
        currentVelocity = velocity;
        localOffset = startPos - spawner.transform.position;
        InvokeRepeating(nameof(Animate), animationSpeed, animationSpeed);
    }

    void Update()
    {
        if (spawner == null) return;

        transform.position += currentVelocity * Time.deltaTime;

        Vector3 pos = transform.position - spawner.borderCenter;
        float ex = pos.x / spawner.borderRadiusX;
        float ey = pos.y / spawner.borderRadiusY;

        if ((ex * ex) + (ey * ey) >= 1f)
        {
            spawner.OnAsteroidDestroyed(this);
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        TryDamagePlayer();
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        TryDamagePlayer();
    }

    void TryDamagePlayer()
    {
        if (Time.time - lastDamageTime < damageCooldown) return;
        lastDamageTime = Time.time;
        PlayerManager.Instance?.ConsumeParts(damageAmount);
    }

    void Animate()
    {
        if (animationSprites.Length == 0) return;
        animationFrame = (animationFrame + 1) % animationSprites.Length;
        spriteRenderer.sprite = animationSprites[animationFrame];
    }
}