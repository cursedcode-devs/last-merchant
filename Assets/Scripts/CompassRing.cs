using UnityEngine;
using UnityEngine.UI;

public class CompassIndicator : MonoBehaviour
{
    [Header("Ustawienia Celu")]
    public Transform target;          // Twój Hub
    public Transform playerTransform; // Kamera lub Gracz

    [Header("Ustawienia UI")]
    public RectTransform ringRect;    // Referencja do pierścienia
    public float radius = 100f;       // Promień

    [Header("Płynność")]
    [Range(1f, 20f)]
    public float smoothSpeed = 10f;   // Jak bardzo "leniwa" ma być ikona

    private float currentAngle;

    void Update()
    {
        if (target == null || playerTransform == null) return;

        // 1. Obliczamy kierunek w 3D
        Vector3 dir = target.position - playerTransform.position;

        // 2. Obliczamy kąt docelowy (targetAngle)
        // Używamy SignedAngle, aby wiedzieć, czy cel jest po lewej, czy po prawej
        float targetAngle = Vector2.SignedAngle(
            new Vector2(playerTransform.forward.x, playerTransform.forward.z),
            new Vector2(dir.x, dir.z)
        );

        // 3. PŁYNNOŚĆ: Interpolacja kąta
        // Mathf.LerpAngle jest kluczowy - rozumie, że 359 stopni jest blisko 1 stopnia
        currentAngle = Mathf.LerpAngle(currentAngle, targetAngle, Time.deltaTime * smoothSpeed);

        // 4. Obliczanie pozycji na okręgu
        float rad = -currentAngle * Mathf.Deg2Rad;
        float x = Mathf.Sin(rad) * radius;
        float y = Mathf.Cos(rad) * radius;

        // 5. Aktualizacja pozycji ikony
        ((RectTransform)transform).localPosition = new Vector3(x, y, 0);

        // 6. Opcjonalnie: Rotacja samej ikony (żeby zawsze była zwrócona "do góry" lub w stronę celu)
        transform.localRotation = Quaternion.Euler(0, 0, currentAngle);
    }
}