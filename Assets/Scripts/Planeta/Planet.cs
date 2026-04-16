using UnityEditor.Tilemaps;
using UnityEngine;

public class Planet : ResourceManagers
{
    public Planet(int fuel, int food, int parts)
    {
        this.fuel = fuel;
        this.food = food;
        this.parts = parts;
    }
}
