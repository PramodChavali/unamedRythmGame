using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "StatsObject", menuName = "Scriptable Objects/StatsObject")]
public class StatsObject : ScriptableObject
{
    [SerializeField] int level;
}
