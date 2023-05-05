using Melanchall.DryWetMidi.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lane : MonoBehaviour
{
    
    //Restricts Notes to a certain key
    public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;

    //Note input to a certain lane
    public KeyCode input;

    //Handles the notes that will be spawned in
    public GameObject notePrefab;

    //Handles text confirmation after hitting the note
    public GameObject niceHit;

    //Handles text confirmation after missing the note
    public GameObject badHit;

    //Handles the list of notes we spawn
    List<Note> notes = new List<Note>();

    //time in which the player needs to tap on the notes
    public List<double> timeStamps = new List<double>();

    //time stamp needs to be spawned and detected
    int spawnIndex = 0;
    int inputIndex = 0; 

    // Start is called before the first frame update
    void Start()
    {
        
    }
    //filter through the notes
    public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
    {
        foreach (var note in array)
        {
            if (note.NoteName == noteRestriction)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                
                //Converts metric time into seconds
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {   
        //Spawn notes
        if (spawnIndex < timeStamps.Count)
        {
            if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
            {
                var note = Instantiate(notePrefab, transform);
                notes.Add(note.GetComponent<Note>());
                note.GetComponent<Note>().assignedTime = (float)timeStamps[spawnIndex];
                spawnIndex++;
            }
        }
        
        if (inputIndex < timeStamps.Count)
        {
            double timeStamp = timeStamps[inputIndex];
            double marginOfError = SongManager.Instance.marginOfError;
            double audioTime = SongManager.GetAudioSourceTime() - (SongManager.Instance.inputDelayInMilliseconds / 1000.0);

            //checks if player hit the note
            if (Input.GetKeyDown(input))
            {
               if (Math.Abs(audioTime - timeStamp) < marginOfError)
                {
                    Hit();
                    //Creates and destroys confirmation for hitting note
                    Destroy(Instantiate(niceHit, transform.position, Quaternion.identity), 1);
                    //Instantiate(niceHit, transform.position,Quaternion.identity);
                    //Destroy(niceHit);
                    print($"Hit on {inputIndex} note");
                    Destroy(notes[inputIndex].gameObject);
                    //moves to next note
                    inputIndex++;
                }
               else
                {
                    print($"Hit inaccurate on {inputIndex}note with {Math.Abs(audioTime - timeStamp)} delay");
                
                }
            }

            //Checks if player missed the note
            if (timeStamp + marginOfError <= audioTime)
            {
                Miss();
                //Creates and destroys confirmation for miss hitting note
                Destroy(Instantiate(badHit, transform.position, Quaternion.identity), 1);
                //DestroyImmediate(badHit);
                print($"Missed {inputIndex} note");
                //moves to next note
                inputIndex++;

            }

        }
    }

    private void Hit()
    {
        ScoreManager.Hit();
    }

    private void Miss()
    {
        ScoreManager.Miss();
    }
}
