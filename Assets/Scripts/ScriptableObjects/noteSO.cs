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
    public float xPosistionInBar;
    public float yPosistionInBar;

}
