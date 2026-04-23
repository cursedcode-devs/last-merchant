using UnityEngine;
using TMPro;
public class ResourceUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fuelText;
    [SerializeField] private TextMeshProUGUI partsText;
    [SerializeField] private TextMeshProUGUI foodText;

    void Start()
    {
        PlayerManager.Instance.OnFuelChanged += HandleFuelChanged;
        PlayerManager.Instance.OnPartsChanged += HandlePartsChanged;
        PlayerManager.Instance.OnFoodChanged += HandleFoodChanged;
        fuelText.text = $"Fuel: {PlayerManager.Instance.GetFuel():0}";
        partsText.text = $"Parts: {PlayerManager.Instance.GetParts()}";
        foodText.text = $"Food: {PlayerManager.Instance.GetFood()}";
    }

    void OnDisable()
    {
        PlayerManager.Instance.OnFuelChanged -= HandleFuelChanged;
        PlayerManager.Instance.OnPartsChanged -= HandlePartsChanged;
        PlayerManager.Instance.OnFoodChanged -= HandleFoodChanged;
    }

    void HandleFuelChanged(float oldFuel, float newFuel)
    {
        fuelText.text = $"Fuel: {newFuel:0}";
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