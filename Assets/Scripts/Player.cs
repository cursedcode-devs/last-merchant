using UnityEngine;

public class Player : ResourceManagers
{

    public Player(float fuel, int food, int parts)
    {
        this.fuel = fuel;
        this.food = food;
        this.parts = parts;
    }

    public override void SetFuel(int value)
    {
        float old = fuel;
        base.SetFuel(value);
        if (fuel > 100f) fuel = 100f;
        if (old != fuel) PlayerManager.Instance?.RaiseFuelChanged(old, fuel);
    }

    public override void AddFuel(float value)
    {
        float old = fuel;
        base.AddFuel(value);
        if (fuel > 100f) fuel = 100f;
        if (old != fuel) PlayerManager.Instance?.RaiseFuelChanged(old, fuel);
    }

    public override void SetParts(int value)
    {
        int old = parts;
        base.SetParts(value);
        if (parts > 100) parts = 100;
        if (old != parts) PlayerManager.Instance?.RaisePartsChanged(old, parts);
    }

    public override void AddParts(int value)
    {
        int old = parts;
        base.AddParts(value);
        if (parts > 100) parts = 100;
        if (old != parts) PlayerManager.Instance?.RaisePartsChanged(old, parts);
    }

    public override void SetFood(int value)
    {
        int old = food;
        base.SetFood(value);
        if (food > 100) food = 100;
        if (old != food) PlayerManager.Instance?.RaiseFoodChanged(old, food);
    }

    public override void AddFood(int value)
    {
        int old = food;
        base.AddFood(value);
        if (food > 100) food = 100;
        if (old != food) PlayerManager.Instance?.RaiseFoodChanged(old, food);
    }
}