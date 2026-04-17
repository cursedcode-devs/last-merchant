using UnityEngine;

[CreateAssetMenu(fileName = "PlanetScriptObject", menuName = "Nasze Obiekty/Planeta")]
public class PlanetScriptObject : ScriptableObject
{
    [SerializeField] public string name;
    [SerializeField] public PlanetTypes planetType;
    [SerializeField] public int startHp;    //100 to maks
    [SerializeField] public Color planetColor;
    [SerializeField] public Sprite sprite;
    [SerializeField] public Dialogue firstDialogue;
    [SerializeField] public Dialogue genericTrade;
    [SerializeField] public Dialogue[] dialogues;
    // [SerializeField] Quest quest;
}
