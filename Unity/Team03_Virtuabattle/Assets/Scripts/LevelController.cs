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

    public List<GameObject> Players = new List<GameObject>();

    private SaveInformation _saveInfo;
    private void Awake()
    {
        _saveInfo = GameObject.FindGameObjectWithTag("GameController").GetComponent<SaveInformation>();
        if (Instance == null) Instance = this;
        AmountOfPlayers = _saveInfo.PlayerCount;

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
            Players.Add(_createdPlayer);
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
        for (int i = 0; i < Players.Count; i++)
        {
            if (Players[i] == null)
                Players.RemoveAt(i);
        }
        Debug.Log(Players.Count);
        if (Players.Count == 1)
        {
            GameObject GameController = GameObject.FindGameObjectWithTag("GameController");

            GameController.GetComponent<SaveInformation>().WinnerMat = TeamColors[Players[0].GetComponent<VariableController>().Player - 1];
            SceneManager.LoadScene(WinnerScreen);
            Debug.Log("Load Victory");
        }
    }
}
