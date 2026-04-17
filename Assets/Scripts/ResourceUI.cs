using UnityEngine;
using TMPro; 
public class FuelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fuelText;
    [SerializeField] private TextMeshProUGUI partsText;

    void Start()
    {
        PlayerManager.Instance.OnFuelChanged += HandleFuelChanged;
        PlayerManager.Instance.OnPartsChanged += HandlePartsChanged;
        fuelText.text = $"Fuel: {PlayerManager.Instance.GetFuel()}";
        partsText.text = $"Parts: {PlayerManager.Instance.GetParts()}";
    }

    void OnDisable()
    {
        PlayerManager.Instance.OnFuelChanged -= HandleFuelChanged;
        PlayerManager.Instance.OnPartsChanged -= HandlePartsChanged;
    }

    void HandleFuelChanged(int oldFuel, int newFuel)
    {
        fuelText.text = $"Fuel: {newFuel}";
    }

    void HandlePartsChanged(int oldParts, int newParts)
    {
        partsText.text = $"Parts: {newParts}";
    }
}