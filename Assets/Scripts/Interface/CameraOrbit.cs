﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraOrbit : MonoBehaviour
{
    protected Transform _XForm_Camera;
    protected Transform _XForm_Parent;

    protected Vector3 _LocalRotation;
    protected float _CameraDistance = 2f;

    public float MouseSensitivity = 4f;
    public float ScrollSensitvity = 2f;
    public float OrbitDampening = 10f;
    public float ScrollDampening = 6f;

    public float num1 = 0f;
    public float num2 = 0f;

    public bool CameraDisabled = true;

    public InputField addPointField;
    // Use this for initialization
    void Start()
    {
        this._XForm_Camera = this.transform;
        this._XForm_Parent = this.transform.parent;
        addPointField = FindObjectOfType<AddPoint>().input;
    }

    public void SwitchMode()
    {
        this._LocalRotation = new Vector3(0, 90, 0);
    }

    void Update()
    {
        if (!SceneManager.fanuc.inputField.isFocused && !SceneManager.fanuc.InputSpeedField.isFocused && !addPointField.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
                GetComponent<Camera>().orthographic = !GetComponent<Camera>().orthographic;
            if (Input.GetMouseButton(1))
            {
                CameraDisabled = false;
            }
            else CameraDisabled = true;


            if (Input.GetKey(KeyCode.UpArrow))
            {
                num2 += 0.02f;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                num2 -= 0.02f;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                num1 += 0.02f;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                num1 -= 0.02f;
            }
        } 
    }

    void LateUpdate()
    {
        if (!CameraDisabled)
        {
            //Rotation of the Camera based on Mouse Coordinates
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                _LocalRotation.x += Input.GetAxis("Mouse X") * MouseSensitivity;
                _LocalRotation.y -= Input.GetAxis("Mouse Y") * MouseSensitivity;

                //Clamp the y Rotation to horizon and not flipping over at the top
                if (_LocalRotation.y < -20f)
                    _LocalRotation.y = -20f;
                else if (_LocalRotation.y > 90f)
                    _LocalRotation.y = 90f;
            }
            //Zooming Input from our Mouse Scroll Wheel
            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                if (this.GetComponent<Camera>().orthographic == false)
                {
                    float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitvity;

                    ScrollAmount *= (this._CameraDistance * 0.3f);

                    this._CameraDistance += ScrollAmount * -1f;

                    this._CameraDistance = Mathf.Clamp(this._CameraDistance, 1.5f, 100f);
                }
                if (this.GetComponent<Camera>().orthographic == true)
                {
                    float ScrollAmount = Input.GetAxis("Mouse ScrollWheel");
                    ScrollAmount *= (this.GetComponent<Camera>().orthographicSize);
                    this.GetComponent<Camera>().orthographicSize += ScrollAmount * -1f;

                }
            }
        }

        //Actual Camera Rig Transformations
        Quaternion QT = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);
        this._XForm_Parent.rotation = Quaternion.Lerp(this._XForm_Parent.rotation, QT, Time.deltaTime * OrbitDampening);

        //if (this._XForm_Camera.localPosition.z != this._CameraDistance * -1f)
        this._XForm_Camera.localPosition = new Vector3(num1, num2, Mathf.Lerp(this._XForm_Camera.localPosition.z, this._CameraDistance * -1f, Time.deltaTime * ScrollDampening));
        
    }
}