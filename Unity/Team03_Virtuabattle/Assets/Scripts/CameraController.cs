using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float _speed;
    public Transform CameraPivot;

    InputController inputController;

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        inputController = GetComponentInChildren<InputController>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCamera();
    }

    void UpdateCamera()
    {
        Vector3 rotation = CameraPivot.localEulerAngles;
        rotation.y += inputController.CameraMovementHorizontal * _speed;
        CameraPivot.localEulerAngles = rotation;
        Vector3 rotationCameraPivot = CameraPivot.localEulerAngles;
        rotationCameraPivot.x += inputController.CameraMovementVertical * _speed;
        rotationCameraPivot.x = CharController.ClampAngle(rotationCameraPivot.x, -30, 50);
        CameraPivot.localEulerAngles = rotationCameraPivot;
    }
}
