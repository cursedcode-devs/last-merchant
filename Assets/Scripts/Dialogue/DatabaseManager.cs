using System.Collections.Generic;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    [Header("Plik z dialogami")]
    public TextAsset jsonFile; // Przeci¹gnij tu swój plik JSON z zak³adki Project

    // S³ownik to najlepszy sposób na b³yskawiczne wyszukiwanie dialogów po ich ID
    private Dictionary<string, SingleDialogue> dialogueDictionary;

    void Awake()
    {
        LoadDialogues();
    }

    private void LoadDialogues()
    {
        // 1. Zmiana tekstu JSON na obiekty C# (Parsowanie)
        DialogueDatabase database = JsonUtility.FromJson<DialogueDatabase>(jsonFile.text);

        // 2. Przerzucamy je do S³ownika dla wygody
        dialogueDictionary = new Dictionary<string, SingleDialogue>();

        foreach (var dialog in database.dialogues)
        {
            dialogueDictionary.Add(dialog.name, dialog);
        }

        Debug.Log($"Wczytano {dialogueDictionary.Count} dialogów!");
    }

    // Funkcja do wyci¹gania konkretnego dialogu
    public SingleDialogue GetDialogue(string id)
    {
        if (dialogueDictionary.ContainsKey(id))
        {
            return dialogueDictionary[id];
        }

        Debug.LogWarning("Nie znaleziono dialogu o ID: " + id);
        return null;
    }
}