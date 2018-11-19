using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {


       
    }
    public void dothat(string message)
    {
        GameObject.FindObjectOfType<InstantiateFromCam>().Syncronization(message);
    }
    // Update is called once per frame
    void Update () {
		
	}
}
