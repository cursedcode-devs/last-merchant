using UnityEngine;

public class Player : ResourceManagers
{
    public int startFuel = 100;
    public int startFood = 100;
    public int startParts = 100;

    public Player(int fuel, int food, int parts)
    {
        this.fuel = fuel;
        this.food = food;
        this.parts = parts;
    }

    public override void SetFuel(int value)
    {
        int old = fuel;
        base.SetFuel(value);
        if (old != fuel) PlayerManager.Instance?.RaiseFuelChanged(old, fuel);
    }

    public override void AddFuel(int value)
    {
        int old = fuel;
        base.AddFuel(value);
        if (old != fuel) PlayerManager.Instance?.RaiseFuelChanged(old, fuel);
    }

    public override void SetParts(int value)
    {
        int old = parts;
        base.SetParts(value);
        if (old != parts) PlayerManager.Instance?.RaisePartsChanged(old, parts);
    }

    public override void AddParts(int value)
    {
        int old = parts;
        base.AddParts(value);
        if (old != parts) PlayerManager.Instance?.RaisePartsChanged(old, parts);
    }

    public override void SetFood(int value)
    {
        int old = food;
        base.SetFood(value);
        if (old != food) PlayerManager.Instance?.RaiseFoodChanged(old, food);
    }

    public override void AddFood(int value)
    {
        int old = food;
        base.AddFood(value);
        if (old != food) PlayerManager.Instance?.RaiseFoodChanged(old, food);
    }
}