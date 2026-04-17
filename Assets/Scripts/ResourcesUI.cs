using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourcesUI : MonoBehaviour
{
    [Header("Paliwo (Fuel)")]
    [SerializeField] private Slider fuelSlider;
    [SerializeField] private TextMeshProUGUI fuelText;

    [Header("Jedzenie (Food)")]
    [SerializeField] private Slider foodSlider;
    [SerializeField] private TextMeshProUGUI foodText;

    [Header("Części (Parts)")]
    [SerializeField] private Slider partsSlider;
    [SerializeField] private TextMeshProUGUI partsText;

    void Start()
    {
        UpdateFuelUI(0, PlayerManager.Instance.GetFuel());
        UpdateFoodUI(0, PlayerManager.Instance.GetFood());
        UpdatePartsUI(0, PlayerManager.Instance.GetParts());

        PlayerManager.Instance.OnFuelChanged += UpdateFuelUI;
        PlayerManager.Instance.OnFoodChanged += UpdateFoodUI;
        PlayerManager.Instance.OnPartsChanged += UpdatePartsUI;
    }

    void OnDisable()
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.OnFuelChanged -= UpdateFuelUI;
            PlayerManager.Instance.OnFoodChanged -= UpdateFoodUI;
            PlayerManager.Instance.OnPartsChanged -= UpdatePartsUI;
        }
    }

    private void UpdateFuelUI(int oldVal, int newVal)
    {
        fuelSlider.value = newVal;
        fuelText.text = newVal.ToString();
    }

    private void UpdateFoodUI(int oldVal, int newVal)
    {
        foodSlider.value = newVal;
        foodText.text = newVal.ToString();
    }

    private void UpdatePartsUI(int oldVal, int newVal)
    {
        partsSlider.value = newVal;
        partsText.text = newVal.ToString();
    }
}