using UnityEngine;


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
        if (GameManager.Instance != null && (GameManager.Instance.isGameOver || GameManager.Instance.isDialogueActive))
            return;

        if (currentFuelMultiplier < maxFuelMultiplier)
        {
            currentFuelMultiplier += fuelMultiplierIncreaseRate * Time.deltaTime;
            currentFuelMultiplier = Mathf.Min(currentFuelMultiplier, maxFuelMultiplier);
        }

        if (usePassiveConsumption)
        {
            if (currentPassiveInterval > minPassiveInterval)
            {
                currentPassiveInterval -= passiveIntervalDecreaseRate * Time.deltaTime;
                currentPassiveInterval = Mathf.Max(currentPassiveInterval, minPassiveInterval);
            }

            if (PlayerManager.Instance != null && PlayerManager.Instance.GetFuel() > 0)
            {
                float currentTime = GameManager.Instance != null ? GameManager.Instance.time : Time.time;
                if ((currentTime - lastPassiveDiminish) > currentPassiveInterval)
                {
                    PlayerManager.Instance.ConsumeFuel(1f);
                    lastPassiveDiminish = currentTime;
                }
            }
        }
    }


    public float GetFuelMultiplier()
    {
        return currentFuelMultiplier;
    }
}
