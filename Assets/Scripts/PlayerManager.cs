using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [SerializeField] private int startFuel = 50;
    [SerializeField] private int startFood = 50;
    [SerializeField] private int startParts = 50;

    private Player player;

    public event Action<float, float> OnFuelChanged;
    public event Action<int, int> OnFoodChanged;
    public event Action<int, int> OnPartsChanged;

    public bool isImmortal = false;

void Awake()
{
    Instance = this;
    player = new Player(startFuel, startFood, startParts); 
}
    public void RaiseFuelChanged(float oldValue, float newValue)
    {
        OnFuelChanged?.Invoke(oldValue, newValue);
    }

    public bool ConsumeFuel(float amount)
    {
        player.AddFuel(-amount);
        return true;
    }

    public void RaisePartsChanged(int oldValue, int newValue)
    {
        OnPartsChanged?.Invoke(oldValue, newValue);
    }

    public bool ConsumeParts(int amount)
    {
        if (isImmortal) return false;
        player.AddParts(-amount);
        if(player.GetParts() < 0){
            player.SetParts(0);
        }
        return true;
    }

    public void RaiseFoodChanged(int oldValue, int newValue)
    {
        OnFoodChanged?.Invoke(oldValue, newValue);
    }
    public bool ConsumeFood(int amount)
    {
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