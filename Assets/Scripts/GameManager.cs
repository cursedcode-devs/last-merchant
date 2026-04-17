using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialoguePromptText;

    public bool playerNearPlanet = false;
    public bool isDialogueActive = false;
    public float time;
    Dialogue dialogue;
    public GameObject DialogueObject;
    public TextMeshProUGUI dialogueText;

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
            NextDialogue();
        }
    }

    public void fPress()
    {
        if (!playerNearPlanet) return;
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            Debug.Log("Klikniêto f");
            PromptTextSetActive(false);
            isDialogueActive = true;
            DialogueObject.SetActive(true);
        }
    }

    public void NextDialogue()
    {
        if ( Keyboard.current.enterKey.wasPressedThisFrame )
        {
            dialogueText.text=dialogue.dialogues[dialogueSentence];
            dialogueSentence++;
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

    public void SetDialogue(Dialogue dialogue)
    {
        this.dialogue = dialogue;
    }

    public void SetPlayerNearPlanet(bool near)
    {
        playerNearPlanet = near;
    }
}
