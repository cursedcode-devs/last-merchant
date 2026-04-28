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
    private bool isDead = false;

    [Header("Ranges")]
    public float radioRange = 5f;
    public float scanRange = 50f;

    public static System.Collections.Generic.List<PlanetObject> allPlanets = new System.Collections.Generic.List<PlanetObject>();

    void OnEnable() { allPlanets.Add(this); }
    void OnDisable() { allPlanets.Remove(this); }

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
        sr.color = Color.white;
        healthBar.transform.localScale = new Vector3(maxHealthBarWidth, 1f, 1f);

        Vector3 vector = new Vector3(-0.1f, 0.2f, 0f);
        Quaternion quat = Quaternion.identity;
        healthBar.transform.SetLocalPositionAndRotation(vector, quat);
    }

    void Start()
    {
        CircleCollider2D col = GetComponent<CircleCollider2D>();
        if (col != null) col.radius = radioRange;

        planet = new Planet(planetData.startHp, planetData.planetTypeRecive, planetData.planetTypeGive);

        trade = new Trade(planetData.planetTypeRecive, planetData.planetTypeGive);

        maxHealth = planetData.startHp;

        initHealthBar();
        GameObject textObject = new GameObject(planetData.name + " Text");
        textObject.transform.SetParent(this.transform, false);
        nameText = textObject.AddComponent<TextMeshPro>();
        nameText.text = planetData.name;
        TMP_FontAsset fontAsset = Resources.Load<TMP_FontAsset>("Fonts/Jersey10-Regular SDF");
        if (fontAsset != null) { nameText.font = fontAsset; }
        nameText.alignment = TextAlignmentOptions.Center;
        nameText.fontSize = 1.5f;

        Vector3 vector = new Vector3(0f, -0.2f, 0f);
        Quaternion quat = Quaternion.identity;
        nameText.transform.SetLocalPositionAndRotation(vector, quat);
    }

    void Update()
    {
        float reductionInterval = PlanetDifficultyManager.Instance != null ? PlanetDifficultyManager.Instance.GetCurrentInterval() : 5f;
        if (manager.time >= lastTimeDamaged + reductionInterval && planet.GetHp() > 0 && !isDead)
        {
            planet.AddHp(-1);
            Debug.Log("Odejmowanie hp");
            UpdateHealthBarUI();
            if (planet.GetHp() <= 0)
            {
                Die();
            }
            lastTimeDamaged = manager.time;
        }

        if (manager.playerMovement != null && planet.GetHp() > 0 && !isDead)
        {
            float dist = Vector3.Distance(transform.position, manager.playerMovement.transform.position);
            
            if (dist <= radioRange)
            {
                if (manager.currentPlanet != this)
                {
                    manager.currentPlanet = this;
                    manager.SetDialogue(planetData.dialogues[whichDialogue], planetData.sprite);
                    manager.SetPlayerNearPlanet(true);
                    manager.PromptTextSetActive(true);
                    manager.SetCurrentPlanet(planetData);
                }
            }
            else if (dist > radioRange && manager.currentPlanet == this)
            {
                manager.SetPlayerNearPlanet(false);
                manager.PromptTextSetActive(false);
                manager.currentPlanet = null;
                manager.SetCurrentPlanet(null);
            }
        }
    }

    private void UpdateHealthBarUI()
    {
        float currentHp = planet.GetHp();
        float healthPercentage = currentHp / (float)maxHealth;
        healthPercentage = Mathf.Clamp01(healthPercentage);
        healthBar.transform.localScale = new Vector3(maxHealthBarWidth * healthPercentage, 1f, 1f);
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        manager.OnPlanetDied();
        healthBar.SetActive(false);
        nameText.gameObject.SetActive(false);
        
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;
        
        if (manager.currentPlanet == this)
        {
            manager.SetPlayerNearPlanet(false);
            manager.PromptTextSetActive(false);
            manager.currentPlanet = null;
            manager.SetCurrentPlanet(null);
        }

        StartCoroutine(DisappearCoroutine());
    }

    private System.Collections.IEnumerator DisappearCoroutine()
    {
        Vector3 initialScale = transform.localScale;
        float duration = 2.0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float scale = Mathf.Lerp(1f, 0f, t * t); 
            transform.localScale = initialScale * scale;
            yield return null;
        }

        Destroy(gameObject);
    }
}
