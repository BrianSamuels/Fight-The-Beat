using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using UnityEngine.Networking;
using System;

public class SongManager : MonoBehaviour
{
    //Allows access from other classes
    public static SongManager Instance;

    //Music
    public AudioSource audioSource;

    //Used to go back to main menu once song is over
    public string mainMenu;

    //Lanes
    public Lane[] lanes;

    //Delays song
    public float songDelayInSeconds;

    //handles how incorrect a player can be(seconds)
    public double marginOfError;

    //Handels input delay in case there are errors with the keyboard
    public int inputDelayInMilliseconds;

    //Holds the location of the MIDI file(music)
    public string fileLocation;

    //Controls how long notes stay on the screen, useful for gauging player reaction time
    public float noteTime;

    //Where the notes spawn 
    public float noteSpawnZ;

    // Where the notes are tapped/deleted by the player
    public float noteTapZ;

    //Where the notes will despawn if not clicked
    public float noteDespawnZ
    {
        get
        {
            return (float)(noteTapZ - (noteSpawnZ + 6.5 - noteTapZ));
        }
    }

    //Where the MIDI file will load on ram
    public static MidiFile midiFile;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        //Checks to see if the file is from a website and reads if True
        if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
        {
            StartCoroutine(ReadFromWebsite());
        }
        //else just reads from file
        else
        {
            ReadFromFile();
        }
    }

    private IEnumerator ReadFromWebsite()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + fileLocation))
        {
            //Waits for request to send
            yield return www.SendWebRequest();

            ///handles reques errors
            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                byte[] results = www.downloadHandler.data;
                using (var stream = new MemoryStream(results))
                {
                    midiFile = MidiFile.Read(stream);
                    GetDataFromMidi();
                }
            }


        }
    }

    //Just reads the Midi file
    private void ReadFromFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
        GetDataFromMidi();
    }

    //Manipulates data from Midi
    public void GetDataFromMidi()
    {
        //gets notes from the midi file
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        //Lanes
        foreach (var lane in lanes) lane.SetTimeStamps(array);
        
        Invoke(nameof(StartSong), songDelayInSeconds);
    }

    //Plays audio source
    public void StartSong()
    {
        audioSource.Play();
    }

    //Gets the time for the audio source
    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }
    // Update is called once per 
    void Update()
    {   
        //Goes back to main menu when song/audiosource ends during the game
        if (!audioSource.isPlaying)
        {
            //Application.Quit();
            //UnityEditor.EditorApplication.isPlaying = false;
            SceneManager.LoadScene(mainMenu);
            Debug.Log("Back to Main Menu");
        }

        //Goes back to main menu if "Q" is pressed during the game
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Application.Quit();
            //UnityEditor.EditorApplication.isPlaying = false;
            SceneManager.LoadScene(mainMenu);
            Debug.Log("Back to Main Menu");
        }

    }
}
