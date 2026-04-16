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

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        player = new Player(startFuel, startFood, startParts);
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

    public int GetFuel() => player.GetFuel();
    
}