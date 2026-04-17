using UnityEngine;
using UnityEngine.InputSystem;

public class PlanetObject : MonoBehaviour
{
    Planet planet;
    public GameManager manager;
    public PlanetScriptObject planetData;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        planet = new Planet(planetData.startHp, planetData.planetType);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Wesz這 na planete NIE GRACZ");
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Wesz這 na planete");
            manager.SetPlayerNearPlanet(true);
            manager.PromptTextSetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Wesz這 na planete NIE GRACZ");
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Wesz這 na planete");
            manager.SetPlayerNearPlanet(false);
            manager.PromptTextSetActive(false);
        }
    }
}
