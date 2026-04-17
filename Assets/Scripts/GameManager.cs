<<<<<<< HEAD
using System.Diagnostics.CodeAnalysis;
=======
using JetBrains.Annotations;
>>>>>>> origin/Alpha
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
<<<<<<< HEAD
    private float lastFoodDiminish;
=======
    public FMODUnity.EventReference interactionSound;
    public FMODUnity.EventReference dialogueSound;

    public int dialogueSentence = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
>>>>>>> origin/Alpha
    void Start()
    {
        PromptTextSetActive(false);
    }

    void FixedUpdate()
    {
        if (!isDialogueActive)
        {
            time += Time.deltaTime;
            fPress();
        }
<<<<<<< HEAD
        if (PlayerManager.Instance.GetFood()>0 && (time - lastFoodDiminish) > 10f)
        {
            PlayerManager.Instance?.ConsumeFood(1);
            lastFoodDiminish = time;
        }

=======
        else
        {
            playerMovement.currentSpeed = 0;
            NextDialogue();
        }
>>>>>>> origin/Alpha
    }

    public void fPress()
    {
        if (!playerNearPlanet) return;
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
<<<<<<< HEAD
            Debug.Log("Klikni�to f");
=======
            Debug.Log("Kliknij to f");
            dialogueText.text = "";
>>>>>>> origin/Alpha
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
