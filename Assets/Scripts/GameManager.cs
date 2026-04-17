using JetBrains.Annotations;
using TMPro;
using Unity.Multiplayer.PlayMode;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Rendering.MaterialUpgrader;

public class GameManager : MonoBehaviour
{
    // --- NOWE ZMIENNE DO SHADERA ---
    [Header("Ustawienia CRT")]
    [SerializeField] private Material crtMaterial;
    [SerializeField] private string bloomPropertyName = "_Bloom_color";
    public FMODUnity.EventReference interactionSound;
    public FMODUnity.EventReference dialogueSound;

    private Color originalBloomColor;
    private PlanetScriptObject currentPlanetS;
    // -------------------------------
    [SerializeField] private TextMeshProUGUI dialoguePromptText;
    public PlayerManager playerManager;
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
    private float lastFoodDiminish;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PromptTextSetActive(false);
        // Zapisujemy domyœlny kolor z materia³u na starcie gry
        if (crtMaterial != null)
        {
            originalBloomColor = crtMaterial.GetColor(bloomPropertyName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDialogueActive)
        {
            time += Time.deltaTime;
            fPress();
        }
        else
        {
            NextDialogue();
        }
        if (PlayerManager.Instance.GetFood() > 0 && (time - lastFoodDiminish) > 10f)
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
            Debug.Log("Klikniêto f");
            currentPlanet.planet.GetHp();
            dialogueText.text = "";
            PromptTextSetActive(false);
            isDialogueActive = true;
            DialogueObject.SetActive(true);
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
        if (Keyboard.current.enterKey.wasPressedThisFrame && tradeActive == false)
        {
            dialogueText.text = dialogue.dialogues[dialogueSentence];
            dialogueSentence++;
            FMODUnity.RuntimeManager.PlayOneShot(dialogueSound, this.transform.position);
        }
        if (dialogueSentence == dialogue.dialogues.Length && tradeActive == false)
        {
            dialogueText.text = currentPlanet.trade.tradeText;
            tradeActive = true;
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
                            playerManager.AddFuel(-currentPlanet.trade.GetRecive());
                            break;
                        }
                    case PlanetTypes.Parts:
                        {
                            playerManager.AddParts(-currentPlanet.trade.GetRecive());
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
        tradeActive = false;
        DialogueObject.SetActive(false);
        dialogue = currentPlanet.planetData.dialogues[currentPlanet.whichDialogue];
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
