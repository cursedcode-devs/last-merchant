using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        Instance = this;
    }

    // --- NOWE ZMIENNE DO SHADERA ---
    [Header("Ustawienia CRT")]
    [SerializeField] private Material crtMaterial;
    [SerializeField] private string bloomPropertyName = "_Bloom_color";
    public FMODUnity.EventReference interactionSound;
    public FMODUnity.EventReference dialogueSound;

    private Color originalBloomColor;
    private PlanetScriptObject currentPlanetS;
    [SerializeField] private Color defaultBloomColor = new Color(1.195026f, 0.39800557f, 0f, 1f);
    // -------------------------------
    [SerializeField] private TextMeshProUGUI dialoguePromptText;
    
    [Header("Hint UI Elements")]
    public GameObject nextDialogueHintObj;
    public GameObject tradeDialogueHintObj;
    
    [Header("Warning UI Elements")]
    public TextMeshProUGUI planetWarningsText;

    [Header("Game Over UI")]
    public GameObject gameOverPanel;
    public bool isGameOver = false;

    [Header("Timer UI")]
    public TextMeshProUGUI countdownTimerText;
    [Tooltip("Czas gry w sekundach (domyślnie 300 = 5 minut)")]
    public float gameDuration = 300f;
    private float currentCountdown;

    public PlayerManager playerManager;
    public PlayerMovement playerMovement;
    public bool playerNearPlanet = false;
    public bool isDialogueActive = false;
    public float time;
    Dialogue dialogue;
    public GameObject DialogueObject;
    public GameObject DialogueImage;
    public TextMeshProUGUI dialogueText;
    public PlanetObject currentPlanet;
    public int dialogueSentence = 0;
    public bool tradeActive = false;
    public int planetNumber = 10;
    private int totalDeadPlanets = 0;
    
    [Header("Survival Settings")]
    [Tooltip("Czas (w sekundach) co jaki gracz traci 1 jednostkę jedzenia na starcie gry")]
    public float initialFoodConsumeInterval = 10f;
    [Tooltip("Minimalny czas (w sekundach) między utratą jedzenia (maksymalny poziom trudności)")]
    public float minFoodConsumeInterval = 2f;
    [Tooltip("O ile sekund skraca się czas między utratą jedzenia z każdą sekundą gry")]
    public float foodConsumeDecreaseRate = 0.01f;
    
    private float currentFoodConsumeInterval;
    
    private float lastFoodDiminish;

    [Header("Planet Spawning")]
    [Tooltip("Maksymalny promień (odległość od środka), w którym mogą pojawić się planety")]
    [SerializeField] private float maxPlanetSpawnRadius = 100f;
    [Tooltip("Minimalna odległość od środka (0,0), aby unikać pojawiania się na graczu")]
    [SerializeField] private float minPlanetDistanceFromCenter = 25f;
    [Tooltip("Minimalna odległość między poszczególnymi planetami")]
    [SerializeField] private float minDistanceBetweenPlanets = 20f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RandomizePlanets();

        PromptTextSetActive(false);
        // Zapisujemy domylny kolor z materiau na starcie gry
        if (crtMaterial != null)
        {
            originalBloomColor = defaultBloomColor;
            crtMaterial.SetColor(bloomPropertyName, originalBloomColor);
        }
        
        currentFoodConsumeInterval = initialFoodConsumeInterval;
        currentCountdown = gameDuration;
        UpdateTimerUI();
    }

    private void RandomizePlanets()
    {
        PlanetObject[] planets = FindObjectsByType<PlanetObject>(FindObjectsSortMode.None);
        System.Collections.Generic.List<Vector3> placedPositions = new System.Collections.Generic.List<Vector3>();

        int globalAttempts = 0;
        bool allPlaced = false;

        while (!allPlaced && globalAttempts < 100)
        {
            globalAttempts++;
            placedPositions.Clear();
            allPlaced = true;

            foreach (var p in planets)
            {
                Vector3 pos = Vector3.zero;
                bool validPosition = false;
                int attempts = 0;

                while (!validPosition && attempts < 200)
                {
                    attempts++;
                    Vector2 randomDir = Random.insideUnitCircle.normalized;
                    float distance = Random.Range(minPlanetDistanceFromCenter, maxPlanetSpawnRadius);
                    pos = new Vector3(randomDir.x * distance, randomDir.y * distance, 0f);

                    validPosition = true;

                    foreach (var placed in placedPositions)
                    {
                        if (Vector3.Distance(pos, placed) < minDistanceBetweenPlanets)
                        {
                            validPosition = false;
                            break;
                        }
                    }
                }

                if (!validPosition)
                {
                    allPlaced = false;
                    break;
                }

                placedPositions.Add(pos);
            }

            if (allPlaced)
            {
                for (int i = 0; i < planets.Length; i++)
                {
                    Transform targetTransform = planets[i].transform.parent != null ? planets[i].transform.parent : planets[i].transform;
                    targetTransform.position = placedPositions[i];
                }
            }
        }
        
        if (!allPlaced)
        {
            Debug.LogError("Nie udało się rozmieścić planet z zachowaniem ustalonych odległości! Zmniejsz wymagane odstępy lub zwiększ promień w inspektorze.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver)
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene("Menu");
            }
            return;
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("Menu");
        }

        if (!isDialogueActive)
        {
            time += Time.deltaTime;
            currentCountdown -= Time.deltaTime;
            UpdateTimerUI();

            if (currentCountdown <= 0f)
            {
                TriggerGameOver("Udało ci się utrzymać cywilizację przy życiu!\nWciśnij ESC aby wrócić do menu", "<color=#33FF28>GRATULACJE</color>");
                return;
            }
            
            // Zwiększanie trudności: zmniejszanie czasu między znikaniem jedzenia
            if (currentFoodConsumeInterval > minFoodConsumeInterval)
            {
                currentFoodConsumeInterval -= foodConsumeDecreaseRate * Time.deltaTime;
                currentFoodConsumeInterval = Mathf.Max(currentFoodConsumeInterval, minFoodConsumeInterval);
            }
            
            ePress();
        }
        else
        {
            NextDialogue();
        }
        if ((time - lastFoodDiminish) > currentFoodConsumeInterval)
        {
            PlayerManager.Instance?.ConsumeFood(1);
            lastFoodDiminish = time;
        }

        UpdatePlanetWarnings();
        
        CheckGameOverConditions();

        // --- DEBUG ---
        //DebugReducePlanetHealth();
        //DebugSpeedUpTime();
    }

    private void CheckGameOverConditions()
    {
        if (PlayerManager.Instance != null)
        {
            if (PlayerManager.Instance.GetFuel() <= 0 ||
                PlayerManager.Instance.GetFood() <= 0 ||
                PlayerManager.Instance.GetParts() <= 0)
            {
                TriggerGameOver("Zabrakło zasobów\nWciśnij ESC aby wrócić do menu");
                return;
            }
        }

        if (totalDeadPlanets >= 5)
        {
            TriggerGameOver("Nie udało ci się utrzymać cywilizacji przy życiu\nWciśnij ESC aby wrócić do menu");
        }
    }

    public void TriggerGameOver(string subtextMessage, string titleMessage = "KONIEC GRY")
    {
        isGameOver = true;
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            Transform titleTransform = gameOverPanel.transform.Find("GameOverText");
            if (titleTransform != null)
            {
                TextMeshProUGUI title = titleTransform.GetComponent<TextMeshProUGUI>();
                if (title != null)
                {
                    title.text = titleMessage;
                }
            }

            Transform subtextTransform = gameOverPanel.transform.Find("GameOverSubtext");
            if (subtextTransform != null)
            {
                TextMeshProUGUI subtext = subtextTransform.GetComponent<TextMeshProUGUI>();
                if (subtext != null)
                {
                    subtext.text = subtextMessage;
                }
            }
        }
        Time.timeScale = 0f;
    }

    private void UpdateTimerUI()
    {
        if (countdownTimerText != null)
        {
            float displayTime = Mathf.Max(0, currentCountdown);
            int minutes = Mathf.FloorToInt(displayTime / 60f);
            int seconds = Mathf.FloorToInt(displayTime % 60f);
            countdownTimerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    public void ePress()
    {
        if (!playerNearPlanet) return;
        if (currentPlanet.planet.GetHp() <= 0)
        {
            return;
        }
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("Klikni�to f");
            // sprawiamy zeby gracz nie mogl latac podczas dialogu, zerujemy speed i zatrzymujemy rutyny
            if (playerMovement != null) playerMovement.canMove = false;
            playerMovement.stopPlayer();
            currentPlanet.planet.GetHp();
            dialogueText.text = "";
            PromptTextSetActive(false);
            isDialogueActive = true;
            if (PlayerManager.Instance != null) PlayerManager.Instance.isImmortal = true;
            DialogueObject.SetActive(true);
            if (nextDialogueHintObj != null) nextDialogueHintObj.SetActive(true);
            if (tradeDialogueHintObj != null) tradeDialogueHintObj.SetActive(false);
            currentPlanet.IncrementDialogueIndex();
            // Zmieniamy kolor shadera na kolor planety
            if (crtMaterial != null && currentPlanet != null)
            {
                crtMaterial.SetColor(bloomPropertyName, currentPlanetS.planetColor);
            }
            FMODUnity.RuntimeManager.PlayOneShot(interactionSound, this.transform.position);
        }
    }

    public void NextDialogue()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame && tradeActive == false)
        {
            dialogueSentence = dialogue.dialogues.Length;
            UpdateTradeDialogueText();
            tradeActive = true;
            if (nextDialogueHintObj != null) nextDialogueHintObj.SetActive(false);
            if (tradeDialogueHintObj != null) tradeDialogueHintObj.SetActive(true);
        }
        else if (Keyboard.current.eKey.wasPressedThisFrame && tradeActive == false)
        {
            if (dialogueSentence < dialogue.dialogues.Length)
            {
                dialogueText.text = dialogue.dialogues[dialogueSentence];
                dialogueSentence++;
                FMODUnity.RuntimeManager.PlayOneShot(dialogueSound, this.transform.position);
            }
            else if (dialogueSentence == dialogue.dialogues.Length)
            {
                UpdateTradeDialogueText();
                tradeActive = true;
                if (nextDialogueHintObj != null) nextDialogueHintObj.SetActive(false);
                if (tradeDialogueHintObj != null) tradeDialogueHintObj.SetActive(true);
            }
        }
        else if (tradeActive)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                int cost = currentPlanet.trade.GetGive();
                bool hasEnough = true;

                switch (currentPlanet.planetData.planetTypeGive)
                {
                    case PlanetTypes.Food:
                        if (playerManager.GetFood() <= cost) hasEnough = false;
                        break;
                    case PlanetTypes.Fuel:
                        if (playerManager.GetFuel() <= cost) hasEnough = false;
                        break;
                    case PlanetTypes.Parts:
                        if (playerManager.GetParts() <= cost) hasEnough = false;
                        break;
                }

                if (!hasEnough)
                {
                    UpdateTradeDialogueText("Nie masz wystarczająco zasobów!");
                    return;
                }

                ResetDialogue();
                switch(currentPlanet.planetData.planetTypeGive)
                {
                    case PlanetTypes.Food:
                        {
                            playerManager.AddFood(-cost);
                            break;
                        }
                    case PlanetTypes.Fuel:
                        {
                            playerManager.AddFuel((float)-cost);
                            break;
                        }
                    case PlanetTypes.Parts:
                        {
                            playerManager.AddParts(-cost);
                            break;
                        }
                }

                int reward = currentPlanet.trade.GetRecive();
                switch (currentPlanet.planetData.planetTypeRecive)
                {
                    case PlanetTypes.Food:
                        {
                            playerManager.AddFood(reward);
                            break;
                        }
                    case PlanetTypes.Fuel:
                        {
                            playerManager.AddFuel((float)reward);
                            break;
                        }
                    case PlanetTypes.Parts:
                        {
                            playerManager.AddParts(reward);
                            break;
                        }
                }

                currentPlanet.trade.GenerateTrade(currentPlanet.planetData.planetTypeRecive, currentPlanet.planetData.planetTypeGive);
                currentPlanet.planet.AddHp(currentPlanet.trade.restoreHealth);
            }

            if (Keyboard.current.qKey.wasPressedThisFrame)
            {
                ResetDialogue();
            }
        }
    }

    private void UpdateTradeDialogueText(string errorMsg = "")
    {
        if (currentPlanet == null || currentPlanet.trade == null || playerManager == null) return;

        string tradeMsg = currentPlanet.trade.tradeText;
        
        if (!string.IsNullOrEmpty(errorMsg))
        {
            tradeMsg += $"\n{errorMsg}";
        }

        // cursed code, siedziałem nad tym 3 godziny
        else {tradeMsg += $"\n";}
        string stats = $"\n\n" +
                       $"<voffset=0.2em><sprite name=\"fuel\"></voffset>{playerManager.GetFuel():F0}     " +
                       $"<voffset=0.2em><sprite name=\"food\"></voffset>{playerManager.GetFood()}     " +
                       $"<voffset=0.2em><sprite name=\"parts\"></voffset>{playerManager.GetParts()}     ";
        dialogueText.text = tradeMsg + stats;
    }

    public void ResetDialogue()
    {
        if (crtMaterial != null)
        {
            crtMaterial.SetColor(bloomPropertyName, originalBloomColor);
        }
        dialogueSentence = 0;
        isDialogueActive = false;
        if (PlayerManager.Instance != null) PlayerManager.Instance.isImmortal = false;
        tradeActive = false;
        DialogueObject.SetActive(false);
        if (nextDialogueHintObj != null) nextDialogueHintObj.SetActive(false);
        if (tradeDialogueHintObj != null) tradeDialogueHintObj.SetActive(false);
        dialogue = currentPlanet.planetData.dialogues[currentPlanet.whichDialogue];
        if (playerMovement != null) playerMovement.canMove = true;
    }

    public void OnPlanetDied()
    {
        totalDeadPlanets++;
        planetNumber--;
        Debug.Log($"Planeta wymarła! Łącznie wymarło: {totalDeadPlanets}");
    }

    public void PromptTextSetActive(bool isActive)
    {
        if (dialoguePromptText == null) { return; }
        dialoguePromptText.gameObject.SetActive(isActive);
    }

    public void SetDialogue(Dialogue dialogue, Sprite sprite)
    {
        this.dialogue = dialogue;
        DialogueImage.GetComponent<SpriteRenderer>().sprite = sprite;
    }

    public void SetPlayerNearPlanet(bool near)
    {
        playerNearPlanet = near;
    }

    public void SetCurrentPlanet(PlanetScriptObject planet)
    {
        currentPlanetS = planet;
    }

    private void UpdatePlanetWarnings()
    {
        if (planetWarningsText == null) return;
        
        string warnings = "";
        
        if (totalDeadPlanets > 0)
        {
            warnings += $"Wymarłe planety: {totalDeadPlanets}/5\n";
        }

        foreach (var p in PlanetObject.allPlanets)
        {
            if (p == null || p.planetData == null || p.planet == null) continue;
            
            float hp = p.planet.GetHp();
            float maxHp = p.planetData.startHp;
            
            if (hp > 0 && hp <= maxHp * 0.33f)
            {
                warnings += $"{p.planetData.name} jest zagrożone\n";
            }
        }
        planetWarningsText.text = warnings;
    }

    private void DebugReducePlanetHealth()
    {
        if (Keyboard.current == null) return;
        
        var keys = new UnityEngine.InputSystem.Controls.KeyControl[] {
            Keyboard.current.digit1Key, Keyboard.current.digit2Key, Keyboard.current.digit3Key,
            Keyboard.current.digit4Key, Keyboard.current.digit5Key, Keyboard.current.digit6Key,
            Keyboard.current.digit7Key, Keyboard.current.digit8Key, Keyboard.current.digit9Key,
            Keyboard.current.digit0Key
        };

        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i].wasPressedThisFrame && i < PlanetObject.allPlanets.Count)
            {
                var p = PlanetObject.allPlanets[i];
                if (p != null && p.planet != null && p.planetData != null)
                {
                    int targetHp = Mathf.FloorToInt(p.planetData.startHp * 0.21f);
                    p.planet.SetHp(targetHp);
                    Debug.Log($"[DEBUG] Zmniejszono życie planety {p.planetData.name} do {targetHp} (21%). Za chwile zniknie punkt hp i pokaze sie ostrzezenie.");
                }
            }
        }
    }

    private void DebugSpeedUpTime()
    {
        if (Keyboard.current == null || isGameOver) return;

        if (Keyboard.current.xKey.wasPressedThisFrame)
        {
            if (Time.timeScale == 1f)
            {
                Time.timeScale = 5f;
                Debug.Log("[DEBUG] Przyspieszono czas x5");
            }
            else
            {
                Time.timeScale = 1f;
                Debug.Log("[DEBUG] Przywrócono normalną prędkość czasu");
            }
        }
    }
}
