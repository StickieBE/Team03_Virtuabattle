using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour {

    #region Fields

    //------- Public
    public int TeamNumber;

    #endregion

    #region Methods

    // Use this for initialization
    void Start () {

        foreach (TurretScript _ts in GetComponentsInChildren<TurretScript>())
            _ts.TeamNumber = TeamNumber;
        foreach (SpawnUnitsScript _spawner in GetComponentsInChildren<SpawnUnitsScript>())
            _spawner.TeamNumber = TeamNumber;
	}

    #endregion
}
