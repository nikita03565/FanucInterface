﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownSceneObjects : MonoBehaviour {
    public Dropdown SceneObjects;
    public string outCoordsStr;

	// Use this for initialization
	void Start ()
    {
        SceneObjects = GetComponent<Dropdown>();
        SceneObjects.onValueChanged.AddListener(delegate {DropdownValueChanged(SceneObjects);});
        
        List<string> tmp = new List<string>();
        for(int i=0;i<SceneManager.Pull.PullObjects.Count;++i)
        {
            tmp.Add(SceneManager.Pull.PullObjects[i].name);
        }
        SceneObjects.AddOptions(tmp);
	}

    void DropdownValueChanged(Dropdown change)
    {
        var obj = SceneManager.Pull.Find(change.value);
        //Debug.Log("ROT: " + obj.transform.rotation.eulerAngles);
        //Vector4 coordsPos = new Vector4 ( obj.transform.position[0], obj.transform.position[1], obj.transform.position[2], 1 );
        Debug.Log(obj.transform.eulerAngles);
        Matrix4x4 coords = FanucModel.rotMatrixDegrees(obj.transform.eulerAngles.x, obj.transform.eulerAngles.y, obj.transform.eulerAngles.z);
        coords[0, 3] = obj.transform.position[0];
        coords[1, 3] = obj.transform.position[1];
        coords[2, 3] = obj.transform.position[2];
        coords = CoordTransformation.UnityToRobot(coords);
        var outCoords = FanucModel.GetCoordsFromMat(coords);
        outCoordsStr = outCoords[0] + " " + outCoords[1] + " " + (outCoords[2] + 190f) + " " + outCoords[3] + " "
            + outCoords[4] + " " + outCoords[5];
        SceneManager.FanucSettingsPanel.coordField.text = outCoordsStr;
        
        //coordText.text = coordTrans.UnityToRobotPosOnly(coordsPos).ToString();
    }
}
