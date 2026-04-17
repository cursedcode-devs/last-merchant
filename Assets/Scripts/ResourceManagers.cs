using UnityEngine;

public class ResourceManagers
{
    protected float fuel;
    protected int food;
    protected int parts;

    public float GetFuel() {  return fuel; }
    public int GetFood() { return food; }
    public int GetParts() { return parts; } 

    public virtual void SetFuel(int fuel)
    {
        this.fuel = fuel;  
    }

    public virtual void SetFood(int food)
    {
        this.food = food;
    }

    public virtual void SetParts(int parts)
    {
        this.parts = parts;
    }

    public virtual void AddFuel(float fuel)
    {
        this.fuel += fuel;
    }

    public virtual void AddFood(int food)
    {
        this.food += food;
    }

    public virtual void AddParts(int parts)
    {
        this.parts+= parts;
    }

    
}
