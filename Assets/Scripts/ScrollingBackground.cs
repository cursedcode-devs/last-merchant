using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float scrollSpeedX = 0.5f;
    public float scrollSpeedY = 0.5f;

    private Material mat;

    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            mat = sr.material;
        }
    }

    void Update()
    {
        if (mat != null)
        {
            Vector2 offset = mat.mainTextureOffset;
            offset.x += scrollSpeedX * Time.deltaTime;
            offset.y += scrollSpeedY * Time.deltaTime;
            mat.mainTextureOffset = offset;
        }
    }
}