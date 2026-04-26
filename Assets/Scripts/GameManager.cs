using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

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
    
    [Header("Survival Settings")]
    [Tooltip("Czas (w sekundach) co jaki gracz traci 1 jednostkę jedzenia na starcie gry")]
    public float initialFoodConsumeInterval = 10f;
    [Tooltip("Minimalny czas (w sekundach) między utratą jedzenia (maksymalny poziom trudności)")]
    public float minFoodConsumeInterval = 2f;
    [Tooltip("O ile sekund skraca się czas między utratą jedzenia z każdą sekundą gry")]
    public float foodConsumeDecreaseRate = 0.01f;
    
    private float currentFoodConsumeInterval;
    
    private float lastFoodDiminish;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PromptTextSetActive(false);
        // Zapisujemy domylny kolor z materiau na starcie gry
        if (crtMaterial != null)
        {
            originalBloomColor = defaultBloomColor;
            crtMaterial.SetColor(bloomPropertyName, originalBloomColor);
        }
        
        currentFoodConsumeInterval = initialFoodConsumeInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDialogueActive)
        {
            time += Time.deltaTime;
            
            // Zwiększanie trudności: zmniejszanie czasu między znikaniem jedzenia
            if (currentFoodConsumeInterval > minFoodConsumeInterval)
            {
                currentFoodConsumeInterval -= foodConsumeDecreaseRate * Time.deltaTime;
                currentFoodConsumeInterval = Mathf.Max(currentFoodConsumeInterval, minFoodConsumeInterval);
            }
            
            fPress();
        }
        else
        {
            NextDialogue();
        }
        if (PlayerManager.Instance.GetFood() > 0 && (time - lastFoodDiminish) > currentFoodConsumeInterval)
        {
            PlayerManager.Instance?.ConsumeFood(1);
            lastFoodDiminish = time;
        }
    }

    public void fPress()
    {
        if (!playerNearPlanet) return;
        if (currentPlanet.planet.GetHp() <= 0)
        {
            return;
        }
        if (Keyboard.current.fKey.wasPressedThisFrame)
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
        if (Keyboard.current.fKey.wasPressedThisFrame && tradeActive == false)
        {
            dialogueText.text = dialogue.dialogues[dialogueSentence];
            dialogueSentence++;
            FMODUnity.RuntimeManager.PlayOneShot(dialogueSound, this.transform.position);
        }
        if (dialogueSentence == dialogue.dialogues.Length && tradeActive == false)
        {
            dialogueText.text = currentPlanet.trade.tradeText;
            tradeActive = true;
            if (nextDialogueHintObj != null) nextDialogueHintObj.SetActive(false);
            if (tradeDialogueHintObj != null) tradeDialogueHintObj.SetActive(true);
        }
        if (tradeActive)
        {
            if (Keyboard.current.yKey.wasPressedThisFrame)
            {
                ResetDialogue();
                switch(currentPlanet.planetData.planetTypeGive)
                {
                    case PlanetTypes.Food:
                        {
                            playerManager.AddFood(-currentPlanet.trade.GetRecive());
                            break;
                        }
                    case PlanetTypes.Fuel:
                        {
                            int x = -currentPlanet.trade.GetRecive();
                            playerManager.AddFuel((float)x);
                            break;
                        }
                    case PlanetTypes.Parts:
                        {
                            playerManager.AddParts(-currentPlanet.trade.GetRecive());
                            break;
                        }
                }

                switch (currentPlanet.planetData.planetTypeRecive)
                {
                    case PlanetTypes.Food:
                        {
                            playerManager.AddFood(currentPlanet.trade.GetGive());
                            break;
                        }
                    case PlanetTypes.Fuel:
                        {
                            int x = currentPlanet.trade.GetGive();
                            playerManager.AddFuel(x);
                            break;
                        }
                    case PlanetTypes.Parts:
                        {
                            playerManager.AddParts(currentPlanet.trade.GetGive());
                            break;
                        }
                }

                currentPlanet.trade.GenerateTrade(currentPlanet.planetData.planetTypeRecive, currentPlanet.planetData.planetTypeGive);
                currentPlanet.planet.AddHp(currentPlanet.trade.restoreHealth);
            }

            if (Keyboard.current.nKey.wasPressedThisFrame)
            {
                ResetDialogue();
            }
        }
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
}
