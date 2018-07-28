using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TelegaModeSwitch : MonoBehaviour {

    public Text textOnButton;
    public GameObject panel;

    public bool isTelegaMode;
    private new GameObject camera; 
    
	// Use this for initialization
	void Start () {
     
     camera = GameObject.Find("Camera");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Click ()
    {
        if (!isTelegaMode)
        {
            isTelegaMode = true;
            camera.GetComponent<raycast>().enabled = true;
            camera.GetComponent<CameraOrbit>().SwitchMode();
            
            camera.GetComponent<Camera>().orthographic = true;
            textOnButton.text = "Normal mode";
            panel.transform.position = new Vector3 (88, panel.transform.position.y, panel.transform.position.z);
            camera.GetComponent<CameraOrbit>().SwitchMode();
        }
        else
        {
            isTelegaMode = false;
            camera.GetComponent<raycast>().enabled = false;
            camera.GetComponent<Camera>().orthographic = false;
            textOnButton.text = "Telega Mode";
            panel.transform.position = new Vector3(-100, panel.transform.position.y, panel.transform.position.z);

        }
    }

    public void Synchronize()
    {
        // GameObject.Find("telega").GetComponent<telegaScript>().enabled = true;
        GameObject.Find("telega").GetComponent<telegaScript>().isMoved = true;
    }
}
