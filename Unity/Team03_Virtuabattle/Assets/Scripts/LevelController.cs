using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

    public int AmountOfPlayers, SceneToLoad;
    public GameObject PlayerPrefab;
    public GameObject[] Spawns;
    public static LevelController Instance;

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
}
