using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialoguePromptText;

    public bool playerNearPlanet = false;
    public bool isDialogueActive = false;
    public float time;
    public GameObject Dialogue;
    public TextMeshProUGUI dialogueText;

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
    }

    public void fPress()
    {
        if (!playerNearPlanet) return;
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            Debug.Log("Klikniêto f");
            PromptTextSetActive(false);
            isDialogueActive=true;
            Dialogue.SetActive(true);
        }
    }

    public void CreateDialogue(Dialogue dialogue)
    {

    }

    public void PromptTextSetActive(bool isActive)
    {
        if (dialoguePromptText == null) { return; }
        dialoguePromptText.gameObject.SetActive(isActive);
    }

    public void SetPlayerNearPlanet(bool near)
    {
        playerNearPlanet = near;
    }
}
