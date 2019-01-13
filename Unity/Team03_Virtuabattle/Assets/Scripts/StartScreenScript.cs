﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenScript : MonoBehaviour {

    public int[] Number;

public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Level()
    {

        SceneManager.LoadScene("Level" + Number);

    }

    public void QuitGame()
    {
        Application.Quit();

    }



}