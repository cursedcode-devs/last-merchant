using UnityEngine;
using TMPro; 
public class ResourcesUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fuelText;
    [SerializeField] private TextMeshProUGUI partsText;
    [SerializeField] private TextMeshProUGUI foodText;

    void Start()
    {
        PlayerManager.Instance.OnFuelChanged += HandleFuelChanged;
        PlayerManager.Instance.OnPartsChanged += HandlePartsChanged;
        PlayerManager.Instance.OnFoodChanged += HandleFoodChanged;
        fuelText.text = $"Fuel: {PlayerManager.Instance.GetFuel()}";
        partsText.text = $"Parts: {PlayerManager.Instance.GetParts()}";
        foodText.text = $"Food: {PlayerManager.Instance.GetFood()}";
    }

    void OnDisable()
    {
        PlayerManager.Instance.OnFuelChanged -= HandleFuelChanged;
        PlayerManager.Instance.OnPartsChanged -= HandlePartsChanged;
        PlayerManager.Instance.OnFoodChanged -= HandleFoodChanged;
    }

    void HandleFuelChanged(int oldFuel, int newFuel)
    {
        fuelText.text = $"Fuel: {newFuel}";
    }

    void HandlePartsChanged(int oldParts, int newParts)
    {
        partsText.text = $"Parts: {newParts}";
    }

    void HandleFoodChanged(int oldFood, int newFood)
    {
        foodText.text = $"Food: {newFood}";
    }
}