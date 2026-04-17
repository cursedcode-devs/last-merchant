using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Rendering.MaterialUpgrader;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialoguePromptText;

    [SerializeField] private PlayerMovement playerMovement;
    public bool playerNearPlanet = false;
    public bool isDialogueActive = false;
    public float time;
    Dialogue dialogue;
    public GameObject DialogueObject;
    public GameObject DialogueImage;
    public TextMeshProUGUI dialogueText;
    public FMODUnity.EventReference interactionSound;
    public FMODUnity.EventReference dialogueSound;

    public int dialogueSentence = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PromptTextSetActive(false);
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
            playerMovement.currentSpeed = 0;
            NextDialogue();
        }
    }

    public void fPress()
    {
        if (!playerNearPlanet) return;
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            Debug.Log("Kliknij to f");
            dialogueText.text = "";
            PromptTextSetActive(false);
            isDialogueActive = true;
            DialogueObject.SetActive(true);
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
}
