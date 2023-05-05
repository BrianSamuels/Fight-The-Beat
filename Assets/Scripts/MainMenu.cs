using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{   
    //Holds each game/scene
    public string firstLevel;
    public string secondLevel;
    public string thirdLevel;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Will move to each respective level when their button is pressed
    public void startGame1()
    {
        SceneManager.LoadScene(firstLevel);
        Debug.Log("Go to Level 1");
    }

    public void startGame2()
    {
        SceneManager.LoadScene(secondLevel);
        Debug.Log("Go to Level 2");
    }

    public void startGame3()
    {
        SceneManager.LoadScene(thirdLevel);
        Debug.Log("Go to level 3");
    }

    //Delete later if not needed
    public void openOptions()
    {

    }

    public void closeOptions()
    {

    }

    //Will exit out of game when quit button is pressed
    public void quitGame()
    {
        //Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
        Debug.Log("Quitting");
    }

}
