using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour {

    public int TeamNumber;

	// Use this for initialization
	void Start () {

        foreach (TurretScript _ts in GetComponentsInChildren<TurretScript>())
        {
            _ts.TeamNumber = TeamNumber;
            _ts.Captured = true;
            _ts.turretHead.GetComponent<Renderer>().material = LevelController.Instance.TeamColors[TeamNumber-1];
        }


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
