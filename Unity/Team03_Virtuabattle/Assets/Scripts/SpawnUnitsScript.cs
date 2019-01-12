using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnitsScript : MonoBehaviour {

    #region Fields

    //----- Public
    public float CoolDownTime;
    public enum TroopType
    {
        Land,
        Sky,
        Other
    }
    public TroopType troopType;
    public GameObject LandPrefab;
    public Vector3 _tempSpawnPos;
    //public GameObject SkyPrefab;

    //----- Private
    VariableController variableController;
    float _timer;
    GameObject _lastSpawnedUnit;
    AIPathControl _gameControl;
    GameObject _unitPrefab;
    //int _goldCost;

    #endregion

    #region Properties

    public int TeamNumber { get; set; }

    #endregion

    #region Methods

    // Use this for initialization
    void Start ()
    {
        variableController = GetComponent<VariableController>();
        _gameControl = LevelController.Instance.GetComponent<AIPathControl>();

        //GetComponent<Renderer>().material = _gameControl.TeamMaterials[TeamNumber - 1];

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

    public void SpawnUnit(VariableController _variableControllerPlayer)
    {
        if (_timer <= 0)
        {
            _variableControllerPlayer.RemoveGold(1);
            //#if DEBUG
            //Debug.Log("Spawn");
            //#endif
            _lastSpawnedUnit = Instantiate(_unitPrefab, _tempSpawnPos, Quaternion.identity);
            //_lastSpawnedUnit.GetComponent<Renderer>().material = _gameControl.TeamMaterials[TeamNumber - 1];
            _lastSpawnedUnit.GetComponent<AIScript>().TeamNumber = TeamNumber;
            _timer = CoolDownTime;
        }
    }

    #endregion
}
