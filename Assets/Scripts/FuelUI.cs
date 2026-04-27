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
        fuelText.text = $"Paliwo: {PlayerManager.Instance.GetFuel():0}";
        foodText.text = $"Jedzenie: {PlayerManager.Instance.GetFood()}";
        partsText.text = $"Części: {PlayerManager.Instance.GetParts()}";
        
    }

    void OnDisable()
    {
        PlayerManager.Instance.OnFuelChanged -= HandleFuelChanged;
        PlayerManager.Instance.OnPartsChanged -= HandlePartsChanged;
        PlayerManager.Instance.OnFoodChanged -= HandleFoodChanged;
    }

    void HandleFuelChanged(float oldFuel, float newFuel)
    {
        fuelText.text = $"Paliwo: {newFuel:0}";
    }

    void HandlePartsChanged(int oldParts, int newParts)
    {
        partsText.text = $"Części: {newParts}";
    }

    void HandleFoodChanged(int oldFood, int newFood)
    {
        foodText.text = $"Jedzenie: {newFood}";
    }

}