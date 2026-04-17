using System.Diagnostics.CodeAnalysis;
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
    private float lastFoodDiminish;
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
            isDialogueActive=true;
            Dialogue.SetActive(true);
        }
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
