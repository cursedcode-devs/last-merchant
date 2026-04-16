using UnityEngine;

[CreateAssetMenu(fileName = "PlanetScriptObject", menuName = "Nasze Obiekty/Planeta")]
public class PlanetScriptObject : ScriptableObject
{
    [SerializeField] public string name;
    [SerializeField] public PlanetTypes planetType;

   // [SerializeField] Quest quest;
}
