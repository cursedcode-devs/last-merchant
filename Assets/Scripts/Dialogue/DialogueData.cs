using System;

// [Serializable] jest kluczowe! Bez tego Unity nie przekonwertuje JSONa na ten obiekt.
[Serializable]
public class SingleDialogue
{
    public string name;
    public string[] sentences;
}

[Serializable]
public class DialogueDatabase
{
    public SingleDialogue[] dialogues; // Nazwa tej zmiennej MUSI byæ taka sama jak w pliku JSON
}