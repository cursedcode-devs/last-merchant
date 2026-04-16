using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("Referencje UI")]
    public GameObject dialogueUI;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    private Queue<string> sentencesQueue; // Zmiana nazwy na sentencesQueue dla czytelnoœci

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        sentencesQueue = new Queue<string>();
        dialogueUI.SetActive(false);
    }

    // TÊ METODÊ ZMIENILIŒMY:
    // Zamiast (Dialogue dialogue) przyjmujemy bezpoœrednio imiê i tablicê zdañ
    public void StartDialogue(string characterName, string[] sentences)
    {
        dialogueUI.SetActive(true);
        nameText.text = characterName;
        sentencesQueue.Clear();

        // Wrzucamy wszystkie pobrane z JSONa zdania do kolejki
        foreach (string sentence in sentences)
        {
            sentencesQueue.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentencesQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentencesQueue.Dequeue();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }

    public void EndDialogue()
    {
        dialogueUI.SetActive(false);
        Debug.Log("Koniec rozmowy.");
    }
}