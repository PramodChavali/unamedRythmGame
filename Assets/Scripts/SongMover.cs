using UnityEngine;

public class SongMover : MonoBehaviour
{
    //this script takes the song and plays it

    //first start by assigning all the bars x and y positions
    private void Start()
    {
        for (int i = 0; i < MIDILoader.Instance.barList.Count; i++)
        {
            Instantiate(MIDILoader.Instance.barList[i].prefab);
            foreach (noteSO note in MIDILoader.Instance.barList[i].notesInBar)
            {
                Instantiate(note.prefab);
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < MIDILoader.Instance.barList.Count; i++)
        {
            if (MIDILoader.Instance.barList[i].prefab.transform.position.x >= -12 && MIDILoader.Instance.barList[i].prefab.transform.position.x <= -12)
            {
                MIDILoader.Instance.barList[i].showBar();
            }
            else
            {
                 //Destroy(MIDILoader.Instance.barList[i].prefab);
            }
                MIDILoader.Instance.barList[i].prefab.gameObject.transform.position -= new Vector3(5, 0, 0) * Time.deltaTime;
        }
    }
}
