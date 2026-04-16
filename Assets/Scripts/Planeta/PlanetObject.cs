using UnityEngine;

public class PlanetObject : MonoBehaviour
{
    Planet planet;
    public PlanetScriptObject planetData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        planet = new Planet(planetData.startFuel, planetData.startFood, planetData.startParts);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
