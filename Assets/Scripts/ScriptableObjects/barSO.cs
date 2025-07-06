using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "barSO", menuName = "Scriptable Objects/barSO")]
public class barSO : ScriptableObject
{
    public List<noteSO> notesInBar = new List<noteSO>();
    

}
