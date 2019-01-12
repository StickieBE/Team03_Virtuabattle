using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollorChanger : MonoBehaviour {

    //public int RendererIndex;
    public GameObject ColorChangingObject;
    private Renderer _charRenderer;

	// Use this for initialization
	void Start () {
        _charRenderer = GetComponentInChildren<MeshRenderer>();
        //_charRenderer.materials[RendererIndex].color = LevelController.Instance.TeamColors[GetComponent<VariableController>().Player - 1].color;
        ColorChangingObject.GetComponent<Renderer>().material = LevelController.Instance.TeamColors[GetComponent<VariableController>().Player - 1];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
