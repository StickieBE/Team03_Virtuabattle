using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour {

    public int AmountOfPlayers, SceneToLoad;
    public GameObject PlayerPrefab;
    public GameObject[] Spawns;
    public Material[] TeamColors;
    public static LevelController Instance;
    public int WinnerScreen;

    public GameObject[] _players;
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    // Use this for initialization
    void Start () {

        GameSettings.AmountOfPlayers = (AmountOfPlayers == 0) ? 4 : AmountOfPlayers;
        GameSettings.SceneToLoad = (SceneToLoad == 0) ? 2 : SceneToLoad;

        Debug.Log(
            "Amount of players: " + GameSettings.AmountOfPlayers +
            " / " +
            "Loaded scene: " + GameSettings.SceneToLoad);

        if (GameSettings.AmountOfPlayers > 0) InitializePlayers();
    }

    private void InitializePlayers()
    {
        Camera.main.gameObject.SetActive(false);

        GameObject _parent = new GameObject("Players");

        for (int i = 0; i < GameSettings.AmountOfPlayers; i++)
        {
            GameObject _createdPlayer = Instantiate(PlayerPrefab, Spawns[i].transform);
            _createdPlayer.transform.parent = _parent.transform;
            _createdPlayer.GetComponent<VariableController>().Player = i + 1;
            _createdPlayer.transform.LookAt(Vector3.zero);
        }
    }

    // Update is called once per frame
    void Update () {
        //Debug.Log(
        //    "Amount of players: " + LevelController.AmountOfPlayers +
        //    " / " + 
        //    "Loaded scene: " + LevelController.SceneToLoad);
	}

    //Warning: This code is shit.
    public void WinConditionCheck()
    {
        _players = GameObject.FindGameObjectsWithTag("Player");
        Debug.Log(_players.Length);
        if (_players.Length == 2)
        {
            GameObject GameController = GameObject.FindGameObjectWithTag("GameController");

            GameController.GetComponent<SaveInformation>().WinnerMat = TeamColors[_players[0].GetComponent<VariableController>().Player - 1];
            SceneManager.LoadScene(WinnerScreen);
            Debug.Log("Load Victory");
        }
    }
}
