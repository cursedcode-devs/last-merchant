using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Rendering.MaterialUpgrader;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialoguePromptText;

    // --- NOWE ZMIENNE DO SHADERA ---
    [Header("Ustawienia CRT")]
    [SerializeField] private Material crtMaterial;
    [SerializeField] private string bloomPropertyName = "_Bloom_color";
    
    private Color originalBloomColor;
    private PlanetScriptObject currentPlanet;
    // -------------------------------

    [SerializeField] private PlayerMovement playerMovement;
    public bool playerNearPlanet = false;
    public bool isDialogueActive = false;
    public float time;
    Dialogue dialogue;
    public GameObject DialogueObject;
    public GameObject DialogueImage;
    public TextMeshProUGUI dialogueText;
    private float lastFoodDiminish;
    public FMODUnity.EventReference interactionSound;
    public FMODUnity.EventReference dialogueSound;

    public int dialogueSentence = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PromptTextSetActive(false);

        // Zapisujemy domyślny kolor z materiału na starcie gry
        if (crtMaterial != null)
        {
            originalBloomColor = crtMaterial.GetColor(bloomPropertyName);
        }
    }

    void FixedUpdate()
    {
        if (!isDialogueActive)
        {
            time += Time.deltaTime;
            fPress();
        }
         else
        {
            playerMovement.currentSpeed = 0;
            NextDialogue();
        }
        if (PlayerManager.Instance.GetFood()>0 && (time - lastFoodDiminish) > 10f)
        {
            PlayerManager.Instance?.ConsumeFood(1);
            lastFoodDiminish = time;
        }
    }

    public void fPress()
    {
        if (!playerNearPlanet) return;
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            Debug.Log("Klikni�to f");
            PromptTextSetActive(false);
            isDialogueActive = true;
            DialogueObject.SetActive(true);

            // Zmieniamy kolor shadera na kolor planety
            if (crtMaterial != null && currentPlanet != null)
            {
                crtMaterial.SetColor(bloomPropertyName, currentPlanet.planetColor);
            }
            FMODUnity.RuntimeManager.PlayOneShot(interactionSound, this.transform.position);
        }
    }

    public void NextDialogue()
    {
        if ( Keyboard.current.enterKey.wasPressedThisFrame )
        {
            dialogueText.text=dialogue.dialogues[dialogueSentence];
            dialogueSentence++;
            FMODUnity.RuntimeManager.PlayOneShot(dialogueSound, this.transform.position);
        }
        if (dialogueSentence == dialogue.dialogues.Length)
        {
            dialogueSentence = 0;
            isDialogueActive = false;
            DialogueObject.SetActive(false);

            // Przywracamy domyślny kolor shadera po zakończeniu dialogu
            if (crtMaterial != null)
            {
                crtMaterial.SetColor(bloomPropertyName, originalBloomColor);
            }
            
        }
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

    // Nowa metoda do odbierania danych o planecie
    public void SetCurrentPlanet(PlanetScriptObject planet)
    {
        currentPlanet = planet;
    }
}