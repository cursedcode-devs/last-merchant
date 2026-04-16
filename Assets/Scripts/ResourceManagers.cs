using UnityEngine;

public class ResourceManagers
{
    private int fuel;
    private int food;
    private int parts;

    public int GetFuel() {  return fuel; }
    public int GetFood() { return food; }
    public int GetParts() { return parts; } 

    public void SetFuel(int fuel)
    {
        this.fuel = fuel;  
    }

    public void SetFood(int food)
    {
        this.food = food;
    }

    public void SetParts(int parts)
    {
        this.parts = parts;
    }

    public void AddFuel(int fuel)
    {
        this.fuel += fuel;
    }

    public void AddFood(int food)
    {
        this.food += food;
    }

    public void AddParts(int parts)
    {
        this.parts+= parts;
    }
}
