using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "barSO", menuName = "Scriptable Objects/barSO")]
public class barSO : ScriptableObject
{
    public List<noteSO> notesInBar = new List<noteSO>();
    public GameObject prefab;

    public void showBar()
    {
        foreach (noteSO note in notesInBar)
        {
            Transform visual = note.prefab.transform.GetComponentInChildren<Transform>();
            visual.gameObject.SetActive(true);
            
        }
    }

    public void hideBar()
    {
        foreach (noteSO note in notesInBar)
        {
            Transform visual = note.prefab.transform.GetComponentInChildren<Transform>();
            visual.gameObject.SetActive(false);

        }
    }
}