using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlanetObject : MonoBehaviour
{
    [SerializeField] public Planet planet;
    public GameManager manager;
    public PlanetScriptObject planetData;
    public TextMeshPro nameText;
    public GameObject healthBar;
    public int whichDialogue = 1;
    private int maxHealth = 100;
    private float maxHealthBarWidth = 5f;
    private float lastTimeDamaged = 0f;

    private void initHealthBar()
    {
        healthBar = new GameObject(planetData.name + " bar");
        healthBar.transform.SetParent(this.transform, false);
        SpriteRenderer sr = healthBar.AddComponent<SpriteRenderer>();
        Texture2D tex = Texture2D.whiteTexture;
        Sprite rectSprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0f, 0.5f), 100f);

        sr.sprite = rectSprite;
        sr.color = Color.green;
        healthBar.transform.localScale = new Vector3(maxHealthBarWidth, 1f, 1f);

        Vector3 vector = new Vector3(-0.1f, 0.25f, 0f);
        Quaternion quat = Quaternion.identity;
        healthBar.transform.SetLocalPositionAndRotation(vector, quat);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        planet = new Planet(planetData.startHp, planetData.planetType);

        maxHealth = planetData.startHp;

        initHealthBar();
        // 1. Tworzymy nowy pusty obiekt w scenie i nadajemy mu nazwê
        GameObject textObject = new GameObject(planetData.name + " Text");
        textObject.transform.SetParent(this.transform, false);
        nameText = textObject.AddComponent<TextMeshPro>();
        nameText.text = planetData.name;
        nameText.alignment = TextAlignmentOptions.Center;
        nameText.fontSize = 1f;

        Vector3 vector = new Vector3(0f, -0.25f, 0f);
        Quaternion quat = Quaternion.identity;
        nameText.transform.SetLocalPositionAndRotation(vector, quat);
    }

    // Update is called once per frame
    void Update()
    {
        if (manager.time >= lastTimeDamaged + 1)
        {
            planet.AddHp(-1);
            Debug.Log("Odejmowanie hp");
            UpdateHealthBarUI();

            lastTimeDamaged = manager.time;
        }
    }

    private void UpdateHealthBarUI()
    {
        // WA¯NE: Zak³adam, ¿e masz dostêp do aktualnego HP planety. 
        // Jeœli zmienna nazywa siê inaczej ni¿ 'hp' (np. currentHp, GetHp()), podmieñ to poni¿ej.
        float currentHp = planet.GetHp();

        // Rzutujemy na (float), ¿eby wynik z dzielenia nie zaokr¹gli³ siê do 0 lub 1
        float healthPercentage = currentHp / (float)maxHealth;

        // Zabezpieczenie, ¿eby pasek nie zacz¹³ rysowaæ siê w drug¹ stronê, jeœli HP spadnie poni¿ej zera
        healthPercentage = Mathf.Clamp01(healthPercentage);

        // Ustawiamy now¹ szerokoœæ: maksymalna szerokoœæ * procent zdrowia
        healthBar.transform.localScale = new Vector3(maxHealthBarWidth * healthPercentage, 1f, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Wesz³o na planete NIE GRACZ");
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Wesz³o na planete");
            manager.SetPlayerNearPlanet(true);
            manager.PromptTextSetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Wesz³o na planete NIE GRACZ");
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Wesz³o na planete");
            manager.SetPlayerNearPlanet(false);
            manager.PromptTextSetActive(false);
        }
    }
}
