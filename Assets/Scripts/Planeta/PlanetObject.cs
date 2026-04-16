using UnityEngine;

public class PlanetObject : MonoBehaviour
{
    Planet planet;
    public PlanetScriptObject planetData;
    public int startHp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        planet = new Planet(startHp, planetData.planetType);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
