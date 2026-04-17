using UnityEngine;
using TMPro; 
public class FuelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI fuelText;

    void Start()
    {
        PlayerManager.Instance.OnFuelChanged += HandleFuelChanged;
        fuelText.text = $"Fuel: {PlayerManager.Instance.GetFuel()}";
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