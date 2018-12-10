﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    VariableController variableController;
    int playerNumber;

    void Start()
    {
        variableController = GetComponent<VariableController>();
        playerNumber = variableController.Player;
    }

    // Update is called once per frame
    void Update ()
    {
        // Buttons
        if (Input.GetButtonDown("ButtonSquare" + playerNumber)) Debug.Log("Player " + playerNumber + " pressed []");
        if (Input.GetButtonDown("ButtonCross" + playerNumber)) Debug.Log("Player " + playerNumber + " pressed X");
        if (Input.GetButtonDown("ButtonCircle" + playerNumber)) Debug.Log("Player " + playerNumber + " pressed O");
        if (Input.GetButtonDown("ButtonTriangle" + playerNumber)) Debug.Log("Player " + playerNumber + @" pressed /_\");
        if (Input.GetButtonDown("L1" + playerNumber)) Debug.Log("Player " + playerNumber + " pressed L1");
        if (Input.GetButtonDown("R1" + playerNumber)) Debug.Log("Player " + playerNumber + " pressed R1");

        // Axis
        if (Input.GetAxisRaw("L2" + playerNumber) != 0) Debug.Log("Player " + playerNumber + " pressed L2");
        if (Input.GetAxisRaw("R2" + playerNumber) != 0) Debug.Log("Player " + playerNumber + " pressed R2");
        if (Input.GetAxisRaw("L3" + playerNumber) != 0) Debug.Log("Player " + playerNumber + " pressed L3");
        if (Input.GetAxisRaw("R3" + playerNumber) != 0) Debug.Log("Player " + playerNumber + " pressed R3");
        if (Input.GetAxisRaw("StartButton" + playerNumber) != 0) Debug.Log("Player " + playerNumber + " pressed start");
        if (Input.GetAxisRaw("BackButton" + playerNumber) != 0) Debug.Log("Player " + playerNumber + " pressed select");

        // MovementAxis
        if (Input.GetAxisRaw("LeftStickHorizontal" + playerNumber) != 0) LogPlayerMovement(Input.GetAxisRaw("LeftStickHorizontal" + playerNumber), "left analog stick", "horizontal");
        if (Input.GetAxisRaw("LeftStickVertical" + playerNumber) != 0) LogPlayerMovement(Input.GetAxisRaw("LeftStickVertical" + playerNumber), "left analog stick", "vertical");

        if (Input.GetAxisRaw("RightStickHorizontal" + playerNumber) != 0) LogPlayerMovement(Input.GetAxisRaw("RightStickHorizontal" + playerNumber), "right analog stick", "horizontal");
        if (Input.GetAxisRaw("RightStickVertical" + playerNumber) != 0) LogPlayerMovement(Input.GetAxisRaw("RightStickVertical" + playerNumber), "right analog stick", "vertical");

        if (Input.GetAxisRaw("DPADHorizontal" + playerNumber) != 0) LogPlayerMovement(Input.GetAxisRaw("DPADHorizontal" + playerNumber), "DPAD", "horizontal");
        if (Input.GetAxisRaw("DPADVertical" + playerNumber) != 0) LogPlayerMovement(Input.GetAxisRaw("DPADVertical" + playerNumber), "DPAD", "vertical");
    }

    private void LogPlayerMovement(float v1, string v2, string v3)
    {
        string _direction = (v3 == "horizontal") ? ((v1 > 0) ? "right" : "left") : ((v1 > 0) ? "forward" : "backward");

        Debug.Log("Player " + playerNumber + " is moving " + _direction + " using " + v2);
    }
}