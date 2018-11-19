using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class timing : MonoBehaviour {
    public int SumTime=0;
	// Use this for initialization
	void Start () {
        foreach (Command com in this.GetComponent<UIComplexCommand>().CommandsSet)
        {
            SumTime += com.time;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
