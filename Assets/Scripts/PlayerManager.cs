using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [SerializeField] private int startFuel = 100;
    [SerializeField] private int startFood = 100;
    [SerializeField] private int startParts = 100;

    private Player player;

    public event Action<float, float> OnFuelChanged;
    public event Action<int, int> OnFoodChanged;
    public event Action<int, int> OnPartsChanged;

    void Awake()
{
    if (Instance == null)
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        player = new Player(startFuel, startFood, startParts); 
    }
    else
    {
        Destroy(gameObject);
    }
}
    public void RaiseFuelChanged(float oldValue, float newValue)
    {
        OnFuelChanged?.Invoke(oldValue, newValue);
    }

    public bool ConsumeFuel(float amount)
    {
        if (player.GetFuel() < amount) return false;
        player.AddFuel(-amount);
        return true;
    }

    public void RaisePartsChanged(int oldValue, int newValue)
    {
        OnPartsChanged?.Invoke(oldValue, newValue);
    }

    public bool ConsumeParts(int amount)
    {
        if (player.GetParts() < amount) return false;
        player.AddParts(-amount);
        return true;
    }

    public void RaiseFoodChanged(int oldValue, int newValue)
    {
        OnFoodChanged?.Invoke(oldValue, newValue);
    }
    public bool ConsumeFood(int amount)
    {
        if (player.GetFood() < amount) return false;
        player.AddFood(-amount);
        return true;
    }

    public void AddFuel(float amount)
    {
        player.AddFuel(amount);
    }

    public void AddFood(int amount)
    {
        player.AddFood(amount);
    }

    public void AddParts(int amount)
    {
        player.AddParts(amount);
    }

    public float GetFuel() => player.GetFuel();
    public int GetParts() => player.GetParts();
    public int GetFood() => player.GetFood();

}