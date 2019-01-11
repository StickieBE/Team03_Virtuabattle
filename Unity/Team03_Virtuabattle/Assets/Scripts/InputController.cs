using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(VariableController))]
public class InputController : MonoBehaviour {


    #region Fields

    //---------- Private
    VariableController variableController;
    public GameObject Barrel { get; set; }
    int playerNumber;
    Transform _cameraTransform;
    public GameObject BulletPrefab;

    //---------- Public
    public LayerMask CastLayer;

    // Action-related
    public bool ActionPressed => Input.GetButton("ButtonSquare" + playerNumber);
    public bool Action => Input.GetButtonDown("ButtonSquare" + playerNumber);
    public bool JumpPressed => Input.GetButton("ButtonCross" + playerNumber);
    public bool Jump => Input.GetButtonDown("ButtonCross" + playerNumber);
    public bool IsShooting => Input.GetButton("R1" + playerNumber);
    public bool Shoot => Input.GetButtonDown("R1" + playerNumber);
    float _timer = .25f;

    // Movement-related
    public float PlayerMovementHorizontal => Input.GetAxis("LeftStickHorizontal" + playerNumber);
    public float PlayerMovementVertical => Input.GetAxis("LeftStickVertical" + playerNumber);

    // Camera-related
    public float CameraMovementHorizontal => Input.GetAxis("RightStickHorizontal" + playerNumber);
    public float CameraMovementVertical => Input.GetAxis("RightStickVertical" + playerNumber);

    #endregion

    #region Methods

    void Start()
    {
        variableController = GetComponent<VariableController>();
        playerNumber = variableController.Player;
        _cameraTransform = GetComponentInChildren<Camera>().transform;
        foreach (Transform _t in GetComponentsInChildren<Transform>())
            if (_t.name == "Barrel") Barrel = _t.gameObject;
    }

    // Update is called once per frame
    void Update ()
    {
        _timer -= Time.deltaTime;

        //DebugInput();

        if (Action)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, _cameraTransform.forward, out hit, 10, CastLayer))
            {
                GameObject _hitObject = hit.transform.gameObject;
                if (_hitObject.tag == "SpawnPillar")
                {
                    SpawnUnitsScript _spawner = _hitObject.GetComponent<SpawnUnitsScript>();
                    if (_spawner.TeamNumber == variableController.Player) _spawner.SpawnUnit();
                }
            }
            Debug.DrawRay(transform.position, _cameraTransform.forward, Color.red, 100);
        }

        if (IsShooting && _timer <= 0) ShootPrimary();

        if (ActionPressed)
        {

            Debug.Log("ActionPressed");
            RaycastHit hit;

            if (Physics.Raycast(transform.position, _cameraTransform.forward, out hit, 100, CastLayer))
            {
                GameObject _hitObject = hit.transform.gameObject;
                if (_hitObject.tag == "Turret")
                {
                    TurretScript _turret = _hitObject.GetComponent<TurretScript>();
                    if (_turret.Captured == false) _turret.Capture(playerNumber);
                }
                Debug.DrawRay(transform.position, _cameraTransform.forward, Color.red, 100);
            }
        }
    }

    private void ShootPrimary()
    {
        GameObject _bullet = Instantiate(BulletPrefab, Barrel.transform.position, Barrel.transform.rotation);
        _bullet.GetComponent<BulletScript>().ShootStraight(gameObject, playerNumber);
        _timer = .25f;
    }

    void DebugInput()
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

    #endregion
}
