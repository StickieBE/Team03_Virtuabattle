using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerCollorScript : MonoBehaviour {

    private SaveInformation SaveInformationObject;

    public Renderer WinnerPart;
    private Material[] Materials;
	// Use this for initialization
	void Start () {
        SaveInformationObject = GameObject.FindGameObjectWithTag("GameController").GetComponent<SaveInformation>();
        //WinnerPart.materials[2].color = SaveInformationObject.WinnerMat.color;
        //Debug.Log(SaveInformationObject.WinnerMat.name);
        WinnerPart.material = SaveInformationObject.WinnerMat;
    }
	
	// Update is called once per frame
	void Update () {
    }
}
