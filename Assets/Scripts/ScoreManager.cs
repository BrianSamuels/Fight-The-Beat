using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager Instance;

    //sound for when we hit a note
    public AudioSource hitSFX;

    //sound for when we miss a note
    public AudioSource missSFX;

    public TMPro.TextMeshPro scoreText;
    static int comboScore;



    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        //resets combo score
        comboScore = 0;
    }
    
    //plays hit sound
    public static void Hit()
    {
        comboScore += 5;
        Instance.hitSFX.Play();
    }

    //plays miss sound
    public static void Miss()
    {
        comboScore -= 10;
        Instance.missSFX.Play();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = comboScore.ToString();
    }
}
