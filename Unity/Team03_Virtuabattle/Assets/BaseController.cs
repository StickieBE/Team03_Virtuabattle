using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour {

    public int TeamNumber;

	// Use this for initialization
	void Start () {

        foreach (TurretScript _ts in GetComponentsInChildren<TurretScript>())
            _ts.TeamNumber = TeamNumber;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
