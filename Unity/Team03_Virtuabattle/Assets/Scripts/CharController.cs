using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class CharController : MonoBehaviour {

    [SerializeField] private float gravityMultiplier;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _drag;
    [SerializeField] private float _maximumXZVelocity = (30 * 1000) / (60 * 60); //[m/s] 30km/h
    [SerializeField] private float _jumpHeight;

    Transform _absoluteTransform;
    CharacterController _char;
    Animator _anim;
    InputController inputController;

    [HideInInspector] public Vector3 Velocity = Vector3.zero; // [m/s]
    [HideInInspector] public Vector3 InputMovement;
    private bool _jump;

    void Start ()
    {
        _char = GetComponent<CharacterController>();
        _absoluteTransform = GetComponentInChildren<Camera>().transform;
        inputController = GetComponent<InputController>();

        #if DEBUG
        Assert.IsNotNull(_char, "DEPENDENCY ERROR: CharacterController missing from PlayerScript");
        #endif

    }

    private void Update()
    {
        InputMovement = new Vector3(inputController.PlayerMovementHorizontal, 0, inputController.PlayerMovementVertical).normalized;
        if (_char.isGrounded && inputController.Jump) _jump = true;
    }

    void FixedUpdate ()
    {
        if (_char.isGrounded) ApplyGround();
        ApplyGravity();
        //if (!_char.isGrounded) ApplyGravity();
        if (_char.isGrounded) ApplyMovement();
        if (_char.isGrounded) ApplyDragOnGround();
        if (_char.isGrounded) ApplyJump();
        LimitXZVelocity();

        Vector3 XZvel = Vector3.Scale(Velocity, new Vector3(1, 0, 1));
        Vector3 localVelXZ = gameObject.transform.InverseTransformDirection(XZvel);

        //if (Velocity != Vector3.zero) DoMovement();
        DoMovement();

        //Reset downwards velocity if charactercontroller is grounded.
        //if (_char.isGrounded && Velocity.y < 0)
        //    Velocity.y = 0;
    }


    private Vector3 RelativeDirection(Vector3 direction)
    {

        Vector3 xzForward = Vector3.Scale(_absoluteTransform.forward, new Vector3(1, 0, 1));
        Quaternion relativeRot = Quaternion.LookRotation(direction);

        return relativeRot.eulerAngles;
    }


    private void ApplyGround()
    {
        Velocity -= Vector3.Project(Velocity, Physics.gravity);
    }

    private void ApplyGravity()
    {
        Velocity += (Physics.gravity * gravityMultiplier) * Time.deltaTime;
    }

    private void ApplyMovement()
    {
        Vector3 xzForward = Vector3.Scale(_absoluteTransform.forward, new Vector3(1, 0, 1));
        Quaternion relativeRot = Quaternion.LookRotation(xzForward);
        Vector3 relativeMov = relativeRot * InputMovement;
        Velocity += relativeMov * _acceleration * Time.deltaTime;
    }

    private void LimitXZVelocity()
    {
        Vector3 yVel = Vector3.Scale(Velocity, Vector3.up);
        Vector3 xzVel = Vector3.Scale(Velocity, new Vector3(1, 0, 1));

        xzVel = Vector3.ClampMagnitude(xzVel, _maximumXZVelocity);

        Velocity = xzVel + yVel;
    }

    private void ApplyDragOnGround()
    {
        Velocity = Velocity * (1 - _drag * Time.deltaTime); //same as lerp
    }

    private void ApplyJump()
    {
        if (_jump)
        {
            Velocity.y += Mathf.Sqrt(2 * Physics.gravity.magnitude * _jumpHeight);
            _jump = false;
        }
    }

    private void DoMovement()
    {
        Vector3 movement = Velocity * Time.deltaTime;
        _char.Move(movement);
    }

    public static float ClampAngle(float angle, float min, float max)
        {
        angle = Mathf.Repeat(angle, 360);
        min = Mathf.Repeat(min, 360);
        max = Mathf.Repeat(max, 360);
        bool inverse = false;
        var tmin = min;
        var tangle = angle;
        if (min > 180)
            {
            inverse = !inverse;
            tmin -= 180;
            }
        if (angle > 180)
            {
            inverse = !inverse;
            tangle -= 180;
            }
        var result = !inverse ? tangle > tmin : tangle < tmin;
        if (!result)
            angle = min;

        inverse = false;
        tangle = angle;
        var tmax = max;
        if (angle > 180)
            {
            inverse = !inverse;
            tangle -= 180;
            }
        if (max > 180)
            {
            inverse = !inverse;
            tmax -= 180;
            }

        result = !inverse ? tangle < tmax : tangle > tmax;
        if (!result)
            angle = max;
        return angle;
        }
    }
