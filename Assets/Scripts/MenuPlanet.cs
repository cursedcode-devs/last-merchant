using UnityEngine;

public class MenuPlanet : MonoBehaviour
{
    public float rotationSpeed = 20f;

    void Update()
    {
        transform.Rotate(Vector3.down, rotationSpeed * Time.deltaTime, Space.World);
    }
}