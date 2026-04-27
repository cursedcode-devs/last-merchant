using UnityEngine;

/// <summary>
/// Skrypt odpowiedzialny za zwiększanie poziomu trudności poprzez skalowanie zużycia paliwa
/// podczas trwania rozgrywki. Inspirowany mechaniką zużycia jedzenia w GameManager.
/// </summary>
public class FuelConsumptionManager : MonoBehaviour
{
    public static FuelConsumptionManager Instance;

    [Header("Skalowanie Zużycia Paliwa (Mnożnik)")]
    [Tooltip("Początkowy mnożnik zużycia paliwa (np. przy ruchach statku)")]
    public float initialFuelMultiplier = 1f;
    [Tooltip("O ile zwiększa się mnożnik zużycia paliwa na sekundę gry")]
    public float fuelMultiplierIncreaseRate = 0.005f;
    [Tooltip("Maksymalny mnożnik zużycia paliwa")]
    public float maxFuelMultiplier = 5f;

    private float currentFuelMultiplier;

    [Header("Pasywne Zużycie Paliwa (Opcjonalne)")]
    [Tooltip("Czy paliwo ma być zużywane pasywnie w czasie, podobnie jak jedzenie?")]
    public bool usePassiveConsumption = false;
    [Tooltip("Początkowy odstęp czasu (w sekundach) między pasywną utratą paliwa")]
    public float initialPassiveInterval = 15f;
    [Tooltip("Minimalny odstęp czasu między pasywną utratą paliwa")]
    public float minPassiveInterval = 5f;
    [Tooltip("O ile skraca się odstęp czasu na sekundę gry")]
    public float passiveIntervalDecreaseRate = 0.01f;

    private float currentPassiveInterval;
    private float lastPassiveDiminish;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        currentFuelMultiplier = initialFuelMultiplier;
        currentPassiveInterval = initialPassiveInterval;
    }

    void Update()
    {
        // Sprawdzamy stan gry w GameManager, aby nie zwiększać trudności w menu lub podczas dialogów
        if (GameManager.Instance != null && (GameManager.Instance.isGameOver || GameManager.Instance.isDialogueActive))
            return;

        // 1. Zwiększanie mnożnika zużycia dla akcji (ruch, skręcanie)
        if (currentFuelMultiplier < maxFuelMultiplier)
        {
            currentFuelMultiplier += fuelMultiplierIncreaseRate * Time.deltaTime;
            currentFuelMultiplier = Mathf.Min(currentFuelMultiplier, maxFuelMultiplier);
        }

        // 2. Skalowanie i obsługa pasywnego zużycia (jeśli włączone w inspektorze)
        if (usePassiveConsumption)
        {
            if (currentPassiveInterval > minPassiveInterval)
            {
                currentPassiveInterval -= passiveIntervalDecreaseRate * Time.deltaTime;
                currentPassiveInterval = Mathf.Max(currentPassiveInterval, minPassiveInterval);
            }

            if (PlayerManager.Instance != null && PlayerManager.Instance.GetFuel() > 0)
            {
                // Używamy czasu z GameManager jeśli dostępny, w przeciwnym razie Time.time
                float currentTime = GameManager.Instance != null ? GameManager.Instance.time : Time.time;
                if ((currentTime - lastPassiveDiminish) > currentPassiveInterval)
                {
                    PlayerManager.Instance.ConsumeFuel(1f);
                    lastPassiveDiminish = currentTime;
                }
            }
        }
    }

    /// <summary>
    /// Zwraca aktualny mnożnik zużycia paliwa.
    /// Powinien być używany w PlayerMovement przy wywoływaniu ConsumeFuel.
    /// </summary>
    public float GetFuelMultiplier()
    {
        return currentFuelMultiplier;
    }
}
