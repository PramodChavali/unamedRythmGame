using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "barSO", menuName = "Scriptable Objects/barSO")]
public class barSO : ScriptableObject
{
    public List<noteSO> notesInBar = new List<noteSO>();
    public GameObject prefab;
    public float size;

    public void showBar()
    {
        prefab.GetComponentInChildren<Transform>().gameObject.SetActive(true);
        foreach (noteSO note in notesInBar)
        {
            Transform visual = note.prefab.transform.GetComponentInChildren<Transform>();
            visual.gameObject.SetActive(true);
            
        }
    }

    public void hideBar()
    {

        prefab.GetComponentInChildren<Transform>().gameObject.SetActive(true);
        foreach (noteSO note in notesInBar)
        {   

            Transform visual = note.prefab.transform.GetComponentInChildren<Transform>();
            visual.gameObject.SetActive(false);

        }
    }
}