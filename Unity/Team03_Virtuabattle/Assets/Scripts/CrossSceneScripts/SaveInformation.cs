using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveInformation : MonoBehaviour {

    public Material WinnerMat;
    public int PlayerCount;


	// Use this for initialization
	void Start () {
        PlayerCount = 2;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetPlayerCount()
    {
        PlayerCount = (int) (GameObject.FindGameObjectWithTag("Slider").GetComponent<Slider>().value);
    }
}
