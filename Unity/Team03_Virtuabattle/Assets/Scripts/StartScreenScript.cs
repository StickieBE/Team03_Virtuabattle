using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenScript : MonoBehaviour {

public void Play1()
    {
       
        SceneManager.LoadScene("Level1");
    }

    public void Play2()
    {

        SceneManager.LoadScene("Level2");
    }

    public void Play3()
    {

        SceneManager.LoadScene("Level3");
    }

    public void QuitGame()
    {
        Application.Quit();

    }



}
