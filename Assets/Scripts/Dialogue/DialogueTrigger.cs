using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public string myDialogueID = "KYT"; // Wpisujesz ID w inspektorze
    public DatabaseManager dbManager; // Referencja do bazy z JSONem

    public void TriggerDialogue()
    {
        // Pobieramy dane z JSONa
        SingleDialogue data = dbManager.GetDialogue(myDialogueID);

        if (data != null)
        {
            // Odpalamy dok³adnie tak, jak poprzednio!
            DialogueManager.Instance.StartDialogue(data.name, data.sentences);
        }
    }
}