using UnityEditor.Tilemaps;
using UnityEngine;

public class Planet : ResourceManagers
{
    public PlanetTypes planetType;
    [SerializeField] private int hp;

    public Planet(int fuel, int food, int parts)
    {
        this.fuel = fuel;
        this.food = food;
        this.parts = parts;
    }

    public Planet(int startHp, PlanetTypes planetType)
    {
        hp = startHp; 
        this.planetType = planetType;
    }
}
