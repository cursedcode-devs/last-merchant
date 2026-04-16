using UnityEngine;

[CreateAssetMenu(fileName = "PlanetScriptObject", menuName = "Nasze Obiekty/Planeta")]
public class PlanetScriptObject : ScriptableObject
{
    [SerializeField] public string name;
    [SerializeField] public int startFuel;
    [SerializeField] public int startFood;
    [SerializeField] public int startParts;

   // [SerializeField] Quest quest;
}
