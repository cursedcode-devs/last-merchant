using UnityEngine;
using TMPro;
using System.Diagnostics;

public class DeathHole : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        UnityEngine.Debug.Log("Player died in black hole.");
    }

}