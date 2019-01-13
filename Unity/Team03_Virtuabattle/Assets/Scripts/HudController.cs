using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour {

    public Text goldAmountUI;
    public Slider Health;
    VariableController _variableController;

	// Use this for initialization
	void Start () {
        _variableController = GetComponent<VariableController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (goldAmountUI.text != _variableController.Gold.ToString()) goldAmountUI.text = _variableController.Gold.ToString();
        if (Health.value != (_variableController.Health * 2))
        {
            Health.value = (_variableController.Health * 2);
        }
    }
}
