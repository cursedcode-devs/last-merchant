using UnityEditor.Tilemaps;
using UnityEngine;

public class Planet : ResourceManagers
{
    public PlanetTypes planetType;
    [SerializeField] private int hp;

    public int GetHp() {  return hp; }
    public PlanetTypes GetPlanetType() {  return planetType; }
    public void SetHp(int hp) { this.hp = hp; }
    public void AddHp(int hp) { this.hp  += hp; }

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
