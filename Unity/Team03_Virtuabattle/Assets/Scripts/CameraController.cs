using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InputController))]
[RequireComponent(typeof(VariableController))]
[RequireComponent(typeof(CharacterController))]
public class CameraController : MonoBehaviour {

    public float RotationSpeed;
    public Transform CameraPivot;

    InputController inputController;
    VariableController variableController;
    CharacterController charController;
    int playerNumber;

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        variableController = GetComponent<VariableController>();
        inputController = GetComponentInChildren<InputController>();
        charController = GetComponentInChildren<CharacterController>();
        playerNumber = variableController.Player;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCamera();
    }

    void UpdateCamera()
    {
        Vector3 rotation = CameraPivot.localEulerAngles;
        //rotation.y += inputController.CameraMovementHorizontal * _speed; // Static camera rotation around player.
        charController.transform.Rotate(0, inputController.CameraMovementHorizontal * RotationSpeed, 0); // Player rotates, camera follows rotation.
        CameraPivot.localEulerAngles = rotation;
        Vector3 rotationCameraPivot = CameraPivot.localEulerAngles;
        rotationCameraPivot.x += inputController.CameraMovementVertical * RotationSpeed;
        rotationCameraPivot.x = CharController.ClampAngle(rotationCameraPivot.x, -30, 50);
        CameraPivot.localEulerAngles = rotationCameraPivot;
    }
}
