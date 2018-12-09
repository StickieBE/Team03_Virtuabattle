using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMouseRotationBehaviour : MonoBehaviour {
    public float _speed;
    private Transform _camera;
    public Transform CameraPivot;
	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        _camera = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {


            Vector3 rotation = transform.localEulerAngles;
            rotation.y += Input.GetAxis("Mouse X") * _speed;
            transform.localEulerAngles = rotation;
        Vector3 rotationCameraPivot = CameraPivot.transform.localEulerAngles;
        rotationCameraPivot.x += Input.GetAxis("Mouse Y") * -_speed;
        rotationCameraPivot.x = BasePlayerScript.ClampAngle(rotationCameraPivot.x, -30, 50);
        CameraPivot.localEulerAngles = rotationCameraPivot;


    }
}
