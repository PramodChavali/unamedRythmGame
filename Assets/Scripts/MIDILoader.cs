using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.Linq;
using System.Collections.Generic;
using Melanchall.DryWetMidi.MusicTheory;
using System;

public class MIDILoader : MonoBehaviour
{
    [SerializeField] private string midiFilePath;
    [SerializeField] private GameObject quarterNote;
    [SerializeField] private GameObject halfNote;
    [SerializeField] private GameObject wholeNote;

    [SerializeField] private GameObject quarterRest;
    [SerializeField] private GameObject halfRest;
    [SerializeField] private GameObject wholeRest;

    private MidiFile midiFile;
    private List<noteSO> noteList = new List<noteSO>();
    private float BPM;
    private float beatDurationMultiplier;
    
    

    private void Awake()
    {
        
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
            Debug.Log(i);
            int octaveNumber = (notes[i].NoteNumber / 12) - 1;
            noteList[i].noteOctave = octaveNumber;
            noteList[i].noteName = notes[i].NoteName.ToString() + octaveNumber.ToString();
        }

        //assign note durations in beats + start time
        for (int i = 0; i < notes.Count(); i++)
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
        for (int i = 0; i < notes.Count(); i++)
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
        for (int i = 0; i < notes.Count(); i++)
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

        for (int i = 0; i < notes.Count(); i++)
        {
            Debug.Log($"note duration is {noteList[i].noteDuration.ToString()} beats, pitch is {noteList[i].noteName}");
        }
          
        
        //assign to a list of barSOs
    }

    private void Update()
    {
        
    }

    public float GetBPM()
    {
        return BPM;
    }
    private float CalculateDuration(int ticks, int tempoInMicroseconds, float multiplier)
    {
        //find out how long each note lasts in seconds
        return 0f;
    }
}
