using UnityEngine;

public class CenterPointer : MonoBehaviour
{
    [Header("Ustawienia Celu i Gracza")]
    public Transform player; // Przeciagnij tu obiekt gracza z inspektora
    
    [Header("Wyglad")]
    public float radius = 2f; // Odleglosc strzalki

    [Header("Efekty dzwiekowe")]
    public FMODUnity.EventReference scanSFX;
    
    private SpriteRenderer[] renderers;
    private bool isVisible = false;

    void Start()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
        // Sprawdzamy też samego siebie, jeśli skrypt jest na tym samym obiekcie co SpriteRenderer
        SpriteRenderer myRenderer = GetComponent<SpriteRenderer>();
        if (myRenderer != null && System.Array.IndexOf(renderers, myRenderer) == -1)
        {
            var list = new System.Collections.Generic.List<SpriteRenderer>(renderers);
            list.Add(myRenderer);
            renderers = list.ToArray();
        }
        
        SetVisible(false);
    }

    void Update()
    {
        if (player == null) return;

        PlanetObject closestPlanet = null;
        float minDistance = float.MaxValue;

        // Znajdz najblizsza planete
        foreach (var planet in PlanetObject.allPlanets)
        {
            if (planet == null) continue;
            
            float dist = Vector3.Distance(player.position, planet.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closestPlanet = planet;
            }
        }

        // Zdecyduj czy strzalka powinna byc widoczna
        bool shouldBeVisible = false;

        if (closestPlanet != null && minDistance <= closestPlanet.scanRange)
        {
            shouldBeVisible = true;

            // Obliczamy wektor kierunku (od gracza do planety)
            Vector3 direction = closestPlanet.transform.position - player.position;
            direction.z = 0;

            // Normalizujemy kierunek i mnozymy przez promien
            Vector3 offset = direction.normalized * radius;
            transform.position = player.position + offset;

            // Obracamy wskaznik
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }

        // Jesli zmienila sie widocznosc na true - odtworz dzwiek i pokaz
        if (shouldBeVisible && !isVisible)
        {
            SetVisible(true);
            if (!scanSFX.IsNull)
            {
                FMODUnity.RuntimeManager.PlayOneShot(scanSFX, transform.position);
            }
        }
        else if (!shouldBeVisible && isVisible)
        {
            SetVisible(false);
        }
    }

    private void SetVisible(bool state)
    {
        isVisible = state;
        foreach (var r in renderers)
        {
            if (r != null) r.enabled = state;
        }
    }
}
