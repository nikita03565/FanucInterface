using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

	// Use this for initialization
	void Start () {


        Debug.Log("Position of normal cube (which named data) "+CoordTransformation.UnityToRobotPosOnly(this.transform.position));

    }

    // Update is called once per frame
    void Update () {
		
	}
}
