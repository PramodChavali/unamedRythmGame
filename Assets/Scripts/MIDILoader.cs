using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.Linq;
using System.Collections.Generic;
using Melanchall.DryWetMidi.MusicTheory;
using System;
using Unity.VisualScripting;

public class MIDILoader : MonoBehaviour
{
    [SerializeField] private string midiFilePath;
    [SerializeField] private GameObject quarterNote;
    [SerializeField] private GameObject halfNote;
    [SerializeField] private GameObject wholeNote;

    [SerializeField] private GameObject quarterRest;
    [SerializeField] private GameObject halfRest;
    [SerializeField] private GameObject wholeRest;

    [SerializeField] private GameObject barPrefab;

    private MidiFile midiFile;
    private List<noteSO> noteList = new List<noteSO>();
    public List<barSO> barList = new List<barSO>();
    private float BPM;
    private float beatDurationMultiplier;

    [SerializeField] private float gapSpace = 3f;
    [SerializeField] private float noteSpace = 3f;
    [SerializeField] private float startingNotePos = 1f;

    private float numGaps;
    public static MIDILoader Instance;
    

    private void Awake()
    {

        Instance = this;

        midiFile = MidiFile.Read(midiFilePath);
        var notes = midiFile.GetNotes().ToList();
        TempoMap tempoMap = midiFile.GetTempoMap();
        var timeSignature = tempoMap.GetTimeSignatureAtTime(new MidiTimeSpan(0));
        beatDurationMultiplier = timeSignature.Denominator / 4;
        var tempo = tempoMap.GetTempoAtTime(new MidiTimeSpan(0));
        BPM = (float) 60000000.0 / tempo.MicrosecondsPerQuarterNote;

        //assign note names in scientific pitch notation
        for (int i = 0; i < notes.Count(); i++)
        {
            noteList.Add(ScriptableObject.CreateInstance<noteSO>());
            int octaveNumber = (notes[i].NoteNumber / 12) - 1;
            noteList[i].noteOctave = octaveNumber;
            noteList[i].noteName = notes[i].NoteName.ToString() + octaveNumber.ToString();
        }

        //assign note durations in beats + start time
        for (int i = 0; i < noteList.Count(); i++)
        {   
            if (noteList[i].rest == false)
            {
                TicksPerQuarterNoteTimeDivision timeDivision = (TicksPerQuarterNoteTimeDivision)midiFile.TimeDivision;
                float ticksPerQuarterNote = timeDivision.TicksPerQuarterNote;

                noteList[i].noteDuration = (float)notes[i].Length / ticksPerQuarterNote * beatDurationMultiplier; //1 = quarter, 2 = half, .5 = eighth, etc.
                noteList[i].noteDuration = Mathf.Round(noteList[i].noteDuration * 10f) / 10f;
                noteList[i].noteStartTime = notes[i].Time;

                //calculate if next note is a rest
                try
                {
                    float restDuration = noteList[i].noteStartTime + notes[i].Length - notes[i + 1].Time;

                    int restDurationInBeats = Mathf.RoundToInt(restDuration / ticksPerQuarterNote * beatDurationMultiplier);

                    if (restDurationInBeats != 0)
                    {
                        if (restDurationInBeats == 1)
                        {
                            noteList.Insert(i + 1, ScriptableObject.CreateInstance<noteSO>());
                            noteList[i + 1].noteDuration = restDurationInBeats;
                            noteList[i + 1].rest = true;
                            noteList[i].prefab = quarterRest;

                        }
                        else if (restDurationInBeats == 2)
                        {
                            noteList.Insert(i + 1, ScriptableObject.CreateInstance<noteSO>());
                            noteList[i + 1].noteDuration = restDurationInBeats;
                            noteList[i + 1].rest = true;
                            noteList[i].prefab = halfRest;
                        }
                        else if (restDurationInBeats == 4)
                        {
                            noteList.Insert(i + 1, ScriptableObject.CreateInstance<noteSO>());
                            noteList[i + 1].noteDuration = restDurationInBeats;
                            noteList[i + 1].rest = true;
                            noteList[i].prefab = wholeRest;
                        }

                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    //this is on the last note, so nothing needs to happen
                }

                
            }
            
        }

        //assign if the stem should go up or down
        for (int i = 0; i < noteList.Count(); i++)
        {
            if (noteList[i].noteOctave >= 4)
            {
                if (noteList[i].noteName.Contains("C") || noteList[i].noteName.Contains("D") || noteList[i].noteName.Contains("E") || noteList[i].noteName.Contains("F"))
                {
                    noteList[i].upOrDownStem = true;
                }
                else
                {
                    noteList[i].upOrDownStem = false;
                }
            }
            else
            {
                noteList[i].upOrDownStem = true;
            }

        }

        //assign prefabs
        for (int i = 0; i < noteList.Count(); i++)
        {
            switch (noteList[i].noteDuration)
            {
                case 1f:
                    noteList[i].prefab = quarterNote;
                    break;
                case 2f:
                    noteList[i].prefab = halfNote;
                    break;
                case 4f:
                    noteList[i].prefab = wholeNote;
                    break;
            }
        }
        
        //create bars and assign them notes
        for (int i = 0; i < noteList.Count(); i++)
        {
            float tempDuration = 0f;

            if (i % timeSignature.Numerator == 0)
            {
                barList.Add(ScriptableObject.CreateInstance<barSO>());
            }

            
            barList[barList.Count() - 1].notesInBar.Add(noteList[i]);
            tempDuration += noteList[i].noteDuration;
            
            
        }

        //assign size of bars and stuff
        for (int i = 0; i < barList.Count(); i++)
        {
            barList[i].prefab = barPrefab;
            numGaps = barList[i].notesInBar.Count() + 1;

            Transform barLine1 = barList[i].prefab.gameObject.transform.Find("barline 1");
            Transform barLine2 = barList[i].prefab.gameObject.transform.Find("barline 2");
            barLine1.localScale = new Vector3(1, 1, 1);
            barLine2.localScale = new Vector3(1, 1, 1);

            float baseBarSize = barList[i].prefab.gameObject.transform.Find("noteStartPoint").transform.position.x - barList[i].prefab.gameObject.transform.Find("noteEndPoint").transform.position.x;

            float totalBarSize = barList[i].notesInBar.Count() * noteSpace + numGaps * gapSpace;
            barList[i].size = totalBarSize;
            Vector3 newScaleX = new Vector3();

            newScaleX.x = totalBarSize / baseBarSize;
            newScaleX.y = barList[i].prefab.gameObject.transform.localScale.y;
            newScaleX.z = barList[i].prefab.gameObject.transform.localScale.z;

            barList[i].prefab.gameObject.transform.localScale = newScaleX;

            //assign the positions of each note within the bar

            float lastPointX = barList[i].prefab.gameObject.transform.Find("noteStartPoint").transform.position.x;


            for (int j = 0; j < barList[i].notesInBar.Count(); j++)
            {
                float xPos = noteSpace * (j + 1) + gapSpace * j;

                if (j == 0)
                {
                    xPos += startingNotePos;
                }

                barList[i].notesInBar[j].xPosistionInBar = xPos;
                barList[i].notesInBar[j].prefab.transform.parent = barList[i].prefab.transform;
            }
        }

        for (int i = 0; i < barList.Count(); i++)
        {
            for (int j = 0; j < barList[i].notesInBar.Count; j++)
            {
                Debug.Log($"bar {i}, note {j} is {barList[i].notesInBar[j].noteName}");
            }
        }
    }
    
    private void Update()
    {

    }

    public float GetBPM()
    {
        return BPM;
    }
    private float pitchToHeight(string noteName)
    {
        return 1f;
    }
}
