using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualDebug : MonoBehaviour {
    
    CharacterController charController;
    CharController charControlScript;
    InputController inputController;
    VariableController variableController;
    Rect rect;
    Rect guiRect;

    private void Start()
    {
        charController = GetComponent<CharacterController>();
        charControlScript = GetComponent<CharController>();
        inputController = GetComponent<InputController>();
        variableController = GetComponent<VariableController>();
        rect = GetComponentInChildren<Camera>().pixelRect;
        guiRect = new Rect(rect.center.x - (rect.width / 2) + 20, Screen.height - rect.center.y - (rect.height / 2) + 20, 250, 670);
    }    

    void OnGUI()
    {
        

        GUILayout.BeginArea(guiRect);

        GUILayout.Label("-------------- STATS --------------");
        GUILayout.Label("Player number: " + variableController.Player);
       
        GUILayout.Label("-------------- POS --------------");
        GUILayout.Label("isGrounded: " + charController.isGrounded);
        GUILayout.Label("Velocity: " + charControlScript.Velocity);
        GUILayout.Label("InputMovement: " + charControlScript.InputMovement);

        GUILayout.Label("-------------- AXIS --------------");
        GUILayout.Label("Vertical: " + inputController.PlayerMovementVertical);
        GUILayout.Label("Horizontal: " + inputController.PlayerMovementHorizontal);
        GUILayout.Label("Vertical [CAM]: " + inputController.CameraMovementVertical);
        GUILayout.Label("Horizontal [CAM]: " + inputController.CameraMovementHorizontal);

        GUILayout.Label("-------------- BUTTONS --------------");
        GUILayout.Label("Jump: " + inputController.JumpPressed);
        GUILayout.Label("Action: " + inputController.ActionPressed);
        GUILayout.EndArea();
    }
}
