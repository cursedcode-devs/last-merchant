using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [SerializeField] private int startFuel = 100;
    [SerializeField] private int startFood = 100;
    [SerializeField] private int startParts = 100;

    private Player player;

    public event Action<int, int> OnFuelChanged;
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
    public void RaiseFuelChanged(int oldValue, int newValue)
    {
        OnFuelChanged?.Invoke(oldValue, newValue);
    }

    public bool ConsumeFuel(int amount)
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

    public int GetFuel() => player.GetFuel();

    public int GetParts() => player.GetParts();
    
}