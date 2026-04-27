using UnityEngine;

/// <summary>
/// Skrypt zarządzający poziomem trudności poprzez skracanie czasu między utratą HP przez planety.
/// Działa analogicznie do mechaniki jedzenia i paliwa.
/// </summary>
public class PlanetDifficultyManager : MonoBehaviour
{
    public static PlanetDifficultyManager Instance;

    [Header("Skalowanie Przetrwania Planet")]
    [Tooltip("Początkowy odstęp czasu (w sekundach) między utratą HP przez planety")]
    public float initialHealthReductionInterval = 2f;
    [Tooltip("Minimalny odstęp czasu (w sekundach) między utratą HP (maksymalna trudność)")]
    public float minHealthReductionInterval = 0.2f;
    [Tooltip("O ile sekund skraca się odstęp czasu z każdą sekundą gry")]
    public float reductionIntervalDecreaseRate = 0.006f;

    private float currentHealthReductionInterval;

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

        currentHealthReductionInterval = initialHealthReductionInterval;
    }

    void Update()
    {
        // Sprawdzamy stan gry w GameManager, aby nie zwiększać trudności w menu lub podczas dialogów
        if (GameManager.Instance != null && (GameManager.Instance.isGameOver || GameManager.Instance.isDialogueActive))
            return;

        // Skracanie interwału (zwiększanie częstotliwości utraty HP)
        if (currentHealthReductionInterval > minHealthReductionInterval)
        {
            currentHealthReductionInterval -= reductionIntervalDecreaseRate * Time.deltaTime;
            currentHealthReductionInterval = Mathf.Max(currentHealthReductionInterval, minHealthReductionInterval);
        }
    }

    /// <summary>
    /// Zwraca aktualny odstęp czasu między utratą HP.
    /// </summary>
    public float GetCurrentInterval()
    {
        return currentHealthReductionInterval;
    }
}
