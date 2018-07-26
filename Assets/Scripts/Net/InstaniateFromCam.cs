using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateFromCam : MonoBehaviour {
    PullManager Pull;

    // Use this for initialization
    void Start() {

    }
   void SceneSynchro()
    {
        //-------------------------------------Here should be parser--------------------------------------------------------

        //-------------------------------------Here he ends------------------------------------------------------------------

        // 10 is stub
        Pull.ResetScene();
        int NumberofObjects=10;
        string[] Names = new string[10]; //IDs
        Vector3[] PositionCoords = new Vector3[10];
        Quaternion[] RotationCoords = new Quaternion[10];
        
      
        for (int i=0;i<NumberofObjects;++i)
        {
            GameObject obj= Pull.Find(Names[i]); 
            obj.transform.rotation = RotationCoords[i];
            obj.transform.position = PositionCoords[i];
            obj.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
