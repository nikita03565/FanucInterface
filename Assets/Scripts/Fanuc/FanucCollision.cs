using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanucCollision : MonoBehaviour {
    FanucScript fanucScript;
	// Use this for initialization
	void Start () {
       fanucScript= FindObjectOfType<FanucScript>();
	}
    void OnTriggerStay(Collider col)
    {
       
        if (col.gameObject.tag == "fanuc")
        {
          
            fanucScript.CollisionLimiter();
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
