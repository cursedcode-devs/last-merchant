using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public Sprite[] animationSprites;
    public float animationSpeed = 0.1f;
    private SpriteRenderer spriteRenderer;
    private int animationFrame;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        InvokeRepeating(nameof(Animate), this.animationSpeed, this.animationSpeed);
    }
    private void Animate()
    {
        animationFrame++;
        if (animationFrame >= this.animationSprites.Length)
        {
            animationFrame = 0;
        }
        spriteRenderer.sprite = this.animationSprites[animationFrame];
    }
}
