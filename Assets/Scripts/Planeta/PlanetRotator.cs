using UnityEngine;

/// <summary>
/// Slowly rotates a planet around its Y axis (simulates slow spin visible in 2D/2.5D view).
/// </summary>
public class PlanetRotator : MonoBehaviour
{
    [Tooltip("Degrees per second around the Y axis")]
    public float rotationSpeed = 30f;

    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.Self);
    }
}
