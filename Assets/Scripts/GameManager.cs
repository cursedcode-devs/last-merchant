using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialoguePromptText;

    public bool playerNearPlanet = false;

    public float time;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PromptTextSetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        fPress();
    }

    public void fPress()
    {
        if (!playerNearPlanet) return;
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            Debug.Log("Klikniêto f");
            PromptTextSetActive(false);
        }
    }

   

    public void PromptTextSetActive(bool isActive)
    {
        dialoguePromptText.gameObject.SetActive(isActive);
    }

    public void SetPlayerNearPlanet(bool near)
    {
        playerNearPlanet = near;
    }
}
