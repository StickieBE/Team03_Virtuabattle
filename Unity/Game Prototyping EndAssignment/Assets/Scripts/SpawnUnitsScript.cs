using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnitsScript : MonoBehaviour {

    public float CoolDownTime;
    private float _timer;
    private GameObject _lastSpwanedUnit;

    private AIPathControl _gameControl;

    public enum TroopType
    {
        Land,
        Sky,
        Other
    }
    public TroopType troopType;

    public GameObject LandPrefab;
    public GameObject SkyPrefab;

    private GameObject _unitPrefab;
    private int _goldCost;
    public int TeamNumber;



    // Use this for initialization
    void Start () {

        _gameControl = GameObject.Find("Game Control").GetComponent<AIPathControl>(); ;

        GetComponent<Renderer>().material = _gameControl.TeamMaterials[TeamNumber - 1];

		if (troopType == TroopType.Land)
        {
            _unitPrefab = LandPrefab;
            _goldCost = 1;
        }
        else
        {
            _unitPrefab = SkyPrefab;
            _goldCost = 3;
        }
        _timer = CoolDownTime;
	}
	
	// Update is called once per frame
	void Update () {
        _timer-= Time.deltaTime;
	}

    public int InitiateUnit(int TeamNumber)
    {
        if (_timer <= 0)
        {

            if (this.TeamNumber == TeamNumber)
            {


                _lastSpwanedUnit = Instantiate(_unitPrefab, _gameControl.TeamSpawnPoints[TeamNumber-1].position, Quaternion.identity);
                _lastSpwanedUnit.GetComponent<Renderer>().material = _gameControl.TeamMaterials[TeamNumber - 1];
                _lastSpwanedUnit.GetComponent<AIScript>().TeamNumber = TeamNumber;
                _lastSpwanedUnit.GetComponent<AIScript>()._spawnPoints = _gameControl.TeamSpawnPoints;
                _timer = CoolDownTime;
                return _goldCost;
            }

            else
            {
                return 0;
            }
        }

        else
        {
            return 0;
        }
    }
}
