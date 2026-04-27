using System;
using UnityEngine;

[Serializable]
public class Planet : ResourceManagers
{
    public PlanetTypes planetTypeRecive;
    public PlanetTypes planetTypeGive;
    [SerializeField] private int hp;

    public int GetHp() {  return hp; }
    public PlanetTypes GetPlanetTypeRecive() {  return planetTypeRecive; }
    public PlanetTypes GetPlanetTypeGive() { return planetTypeGive; }
    public void SetHp(int hp) { this.hp = hp; }
    public void AddHp(int hp) { this.hp  += hp; }

    public Planet(int fuel, int food, int parts)
    {
        this.fuel = fuel;
        this.food = food;
        this.parts = parts;
    }

    public Planet(int startHp, PlanetTypes planetTypeRecive, PlanetTypes planetTypeGive)
    {
        hp = startHp; 
        this.planetTypeRecive = planetTypeRecive;
        this.planetTypeGive = planetTypeGive;
    }
}
