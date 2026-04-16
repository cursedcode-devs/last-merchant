using UnityEngine;
using TMPro; 
public class FuelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fuelText;

    void OnEnable()
    {
        PlayerManager.Instance.OnFuelChanged += HandleFuelChanged;
    }

    void OnDisable()
    {
        PlayerManager.Instance.OnFuelChanged -= HandleFuelChanged;
    }

    void HandleFuelChanged(int oldFuel, int newFuel)
    {
        fuelText.text = $"Fuel: {newFuel}";
    }
}