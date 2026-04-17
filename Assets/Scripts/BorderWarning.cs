using UnityEngine;
using TMPro;

public class BorderWarning : MonoBehaviour
{
    public TextMeshProUGUI warningText;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (warningText != null)
            warningText.gameObject.SetActive(true);
    }
    
}