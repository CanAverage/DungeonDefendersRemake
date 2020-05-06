using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{

    [SerializeField] GameObject _player;
    private float _speedH = 2.0f;
    private float _speedV = 2.0f;

    private float _yaw = 0.0f;
    private float _pitch = 0.0f;

    void Start()
    {
        //Set Cursor to not be visible
        Cursor.visible = false;
    }

    void Update()
    {
        _yaw += _speedH * Input.GetAxis("Mouse X");
        _pitch -= _speedV * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(_pitch, _yaw, 0.0f);
        _player.transform.eulerAngles = new Vector3(0f, _yaw, 0.0f);
    }
}
