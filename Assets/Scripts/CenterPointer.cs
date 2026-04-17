using UnityEngine;

public class CenterPointer : MonoBehaviour
{
    [Header("Ustawienia Celu i Gracza")]
    public Transform player; // Przeci¹gnij tu obiekt gracza z inspektora
    public Vector3 mapCenter = Vector3.zero; // Œrodek mapy (domyœlnie X:0, Y:0, Z:0)

    [Header("Wygl¹d")]
    public float radius = 2f; // Odleg³oœæ, w jakiej strza³ka kr¹¿y wokó³ gracza

    void Update()
    {
        // Sprawdzamy, czy gracz istnieje, ¿eby unikn¹æ b³êdów
        if (player == null) return;

        // 1. Obliczamy wektor kierunku (od gracza do œrodka mapy)
        Vector3 direction = mapCenter - player.position;
        direction.z = 0; // Upewniamy siê, ¿e ignorujemy oœ Z (dla gier 2D)

        // 2. Normalizujemy kierunek (zmieniamy jego d³ugoœæ na 1) i mno¿ymy przez nasz promieñ
        Vector3 offset = direction.normalized * radius;

        // 3. Ustawiamy pozycjê wskaŸnika (pozycja gracza + przesuniêcie)
        transform.position = player.position + offset;

        // 4. Obliczamy k¹t obrotu za pomoc¹ Atan2 i zamieniamy radiany na stopnie
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 5. Obracamy wskaŸnik. 
        // UWAGA: Odejmujemy 90 stopni (- 90f) zak³adaj¹c, ¿e Twoja grafika strza³ki 
        // domyœlnie jest narysowana grotem skierowanym w górê. 
        // Jeœli Twoja strza³ka wskazuje domyœlnie w prawo, usuñ to "- 90f".
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }
}