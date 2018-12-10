using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(VariableController))]
public class SplitscreenInit : MonoBehaviour {

    VariableController variableController;

    // Use this for initialization
    void Start () {
        variableController = GetComponent<VariableController>();
        InitializeCamera();
    }

    private void InitializeCamera()
    {
        switch (GameSettings.AmountOfPlayers)
        {
            case 1:
                InitializeSinglePlayer();
                break;
            case 2:
                InitializeTwoPlayers();
                break;
            case 3:
                InitializeThreePlayers();
                break;
            case 4:
                InitializeFourPlayers();
                break;
            default:
                break;
        }
    }

    private void InitializeSinglePlayer()
    {
        Rect _newCameraRectangle = new Rect(0, 0, 1, 1);

        Camera _camera = GetComponentInChildren<Camera>();

        _camera.rect = _newCameraRectangle;
    }

    private void InitializeTwoPlayers()
    {
        Rect _newCameraRectangle = new Rect(
            0,
            1 - (0.5f * variableController.Player),
            1,
            0.5f
        );

        Camera _camera = GetComponentInChildren<Camera>();

        _camera.rect = _newCameraRectangle;
    }

    private void InitializeThreePlayers()
    {
        Rect _newCameraRectangle = new Rect(
            variableController.Player == 2 ? 0.5f : 0,
            variableController.Player == 3 ? 0 : 0.5f,
            variableController.Player == 3 ? 1 : 0.5f,
            0.5f
        );

        Camera _camera = GetComponentInChildren<Camera>();

        _camera.rect = _newCameraRectangle;
    }

    private void InitializeFourPlayers()
    {
        Rect _newCameraRectangle = new Rect(
            (variableController.Player % 2) == 1 ? 0 : 0.5f,
            variableController.Player > 2 ? 0 : 0.5f,
            0.5f,
            0.5f
        );

        Camera _camera = GetComponentInChildren<Camera>();

        _camera.rect = _newCameraRectangle;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
