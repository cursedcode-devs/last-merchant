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
    public int whichDialogue = 0;
    private int maxHealth = 100;
    private float maxHealthBarWidth = 5f;
    private float lastTimeDamaged = 0f;
    public Trade trade;

    public void IncrementDialogueIndex()
    {
        if (planetData.dialogues != null && whichDialogue < planetData.dialogues.Length - 1)
        {
            whichDialogue++;
        }
    }
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
        planet = new Planet(planetData.startHp, planetData.planetTypeRecive, planetData.planetTypeGive);

        trade = new Trade(planetData.planetTypeRecive, planetData.planetTypeGive);

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
        if (manager.time >= lastTimeDamaged + 5 && planet.GetHp() > 0)
        {
            planet.AddHp(-1);
            Debug.Log("Odejmowanie hp");
            UpdateHealthBarUI();
            if (planet.GetHp() <= 0)
            {
                manager.planetNumber--;
                healthBar.SetActive(false);
                nameText.gameObject.SetActive(false);
            }
            lastTimeDamaged = manager.time;
        }
    }

    private void UpdateHealthBarUI()
    {
        float currentHp = planet.GetHp();
        float healthPercentage = currentHp / (float)maxHealth;
        healthPercentage = Mathf.Clamp01(healthPercentage);
        healthBar.transform.localScale = new Vector3(maxHealthBarWidth * healthPercentage, 1f, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Wesz³o na planete NIE GRACZ");
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Wesz³o na planete");
            manager.currentPlanet = this;
            manager.SetDialogue(planetData.dialogues[whichDialogue], planetData.sprite);
            manager.SetPlayerNearPlanet(true);
            manager.PromptTextSetActive(true);

            // Przekazujemy planete do GameManagera
            manager.SetCurrentPlanet(planetData);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Wesz³o na planete NIE GRACZ");
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Wesz³o na planete");
           // manager.currentPlanet = null;
            manager.SetPlayerNearPlanet(false);
            manager.PromptTextSetActive(false);
            manager.SetCurrentPlanet(null);
        }
    }
}
