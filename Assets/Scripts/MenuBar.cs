using UnityEngine;

public class PlanetBarManager : MonoBehaviour
{
    [Header("Referencje")]
    public Transform planetBarTransform; 
    public MeshRenderer planetRenderer;

    [Header("Ustawienia")]
    public float decreaseSpeed = 0.2f;

    private float currentScaleX;
    private Vector3 initialScale;
    private bool isDepleted = false;

    void Start()
    {
        if (planetBarTransform != null)
        {
            initialScale = planetBarTransform.localScale;
            currentScaleX = initialScale.x;
        }
    }

    void Update()
    {
        if (currentScaleX > 0f)
        {
            currentScaleX -= decreaseSpeed * Time.deltaTime;

            if (currentScaleX < 0f) currentScaleX = 0f;

            planetBarTransform.localScale = new Vector3(currentScaleX, initialScale.y, initialScale.z);

            if (currentScaleX <= 0f && !isDepleted)
            {
                planetBarTransform.gameObject.SetActive(false);
                planetRenderer.gameObject.SetActive(false);
                isDepleted = true;
            }
        }
    }
}