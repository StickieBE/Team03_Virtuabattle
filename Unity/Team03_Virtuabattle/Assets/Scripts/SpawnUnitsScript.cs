using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnitsScript : MonoBehaviour {

    VariableController variableController;

    public int TeamNumber;
    public float CoolDownTime;
    private float _timer;
    private GameObject _lastSpawnedUnit;

    //private AIPathControl _gameControl;

    public enum TroopType
    {
        Land,
        Sky,
        Other
    }
    public TroopType troopType;

    public GameObject LandPrefab;
    //public GameObject SkyPrefab;

    private GameObject _unitPrefab;
    //private int _goldCost;

    public Vector3 _tempSpawnPos;

    // Use this for initialization
    void Start ()
    {
        variableController = GetComponent<VariableController>();

        //_gameControl = GameObject.Find("Game Control").GetComponent<AIPathControl>();

        //GetComponent<Renderer>().material = _gameControl.TeamMaterials[TeamNumber - 1];

        //_tempSpawnPos = _gameControl.TeamSpawnPoints[TeamNumber - 1].position;

        if (troopType == TroopType.Land)
        {
            _unitPrefab = LandPrefab;
            //_goldCost = 1;
        }
        //else
        //{
        //    _unitPrefab = SkyPrefab;
        //    //_goldCost = 3;
        //}
        _timer = CoolDownTime;
	}
	
	// Update is called once per frame
	void Update ()
    {
        _timer-= Time.deltaTime;
	}

    public void SpawnUnit()
    {
        if (_timer <= 0)
        {
            _lastSpawnedUnit = Instantiate(_unitPrefab, _tempSpawnPos, Quaternion.identity);
            //_lastSpwanedUnit.GetComponent<Renderer>().material = _gameControl.TeamMaterials[TeamNumber - 1];
            //_lastSpwanedUnit.GetComponent<AIScript>().TeamNumber = TeamNumber;
            //_lastSpwanedUnit.GetComponent<AIScript>().SpawnPoints = _gameControl.TeamSpawnPoints;
            _timer = CoolDownTime;
        }
    }
}
