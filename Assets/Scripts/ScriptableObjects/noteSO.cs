using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using UnityEngine;

[CreateAssetMenu(fileName = "noteSO", menuName = "Scriptable Objects/noteSO")]
public class noteSO : ScriptableObject
{
    public string noteName;
    public float noteDuration;
    public float noteStartTime;
    public bool upOrDownStem; // true for up stem, false for down stem
    public GameObject prefab;
    public int noteOctave;
    public bool rest = false;

    public void constructNote(string noteName, float noteDuration, float noteStartTime, bool upOrDownStem, GameObject prefab, int noteOctave)
    {
        this.name = noteName;
        this.noteDuration = noteDuration;
        this.noteStartTime = noteStartTime;
        this.upOrDownStem = upOrDownStem;
        this.prefab = prefab;
        this.noteOctave = noteOctave;
    }
    public void constructRest(bool rest, float noteDuration, GameObject prefab)
    {
        this.rest = rest;
        this.noteDuration = noteDuration;
        this.prefab = prefab;
    }
}
