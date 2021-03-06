﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddPoint : MonoBehaviour {

    public InputField input;

    public GameObject Camera, PointList, ScrollBar;


	// Use this for initialization
	void Start () {
        //input = this.transform.Find("InputTelegaCoordField").GetComponent<InputField>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public static void AddFromObserver(float x, float y)
    {
        raycast cam = GameObject.FindObjectOfType<raycast>();
        cam.aims.Add(CoordTransformation.RobotToUnityPosOnly(new Vector3(x, y, 381f)));
        cam.balls.Add(Instantiate(cam.GetComponent<raycast>().BallPr));
        cam.balls[cam.GetComponent<raycast>().balls.Count - 1].transform.position = cam.GetComponent<raycast>().aims[cam.GetComponent<raycast>().balls.Count - 1];

    }
    public void Add()
    { 
        if (!SceneManager.telega.telega.isMoved)
        {
            try
            {
                if (input.text.Contains(","))
                {
                    throw new System.Exception();
                }
                if (input.text.Length > 0)
                {
                    var arr = input.text.Split();

                    if (arr.Length == 2)
                    {
                        raycast cam = GameObject.FindObjectOfType<raycast>();
                        //var aimsRobot = CoordTransformation.UnityToRobotPosOnly(new Vector4(aims[j].x, aims[j].y, aims[j].z, 1));
                        cam.aims.Add(CoordTransformation.RobotToUnityPosOnly(new Vector3(float.Parse(arr[0]), float.Parse(arr[1]), 381f)));
                        cam.balls.Add(Instantiate(cam.GetComponent<raycast>().BallPr));
                        cam.balls[cam.GetComponent<raycast>().balls.Count - 1].transform.position = cam.GetComponent<raycast>().aims[cam.GetComponent<raycast>().balls.Count - 1];
                        input.text = "";

                        if (Camera.GetComponent<raycast>().balls.Count >= 10)
                        {
                            PointList.GetComponent<ContentSizeFitter>().enabled = true;
                            // GameObject.Find("PointList").transform.position = new Vector3(GameObject.Find("PointList").transform.position.x, GameObject.Find("RectObject").transform.position.y, GameObject.Find("PointList").transform.position.z);
                            ScrollBar.GetComponent<Scrollbar>().value = 0.00000f;
                        }
                    }
                    else
                    {
                        throw new System.Exception();
                    }
                }
                else
                {
                    throw new System.Exception();
                };

            }
            catch (System.Exception)
            {
                Debug.Log("Wrong string");
                input.text = "Wrong string. Try again";
            }
        }
        
    }

    public void Delete()
    {
        int count = Camera.GetComponent<raycast>().aims.Count;
        if (count > 0)
        {
            Camera.GetComponent<raycast>().aims.RemoveAt(count - 1);
            Destroy(Camera.GetComponent<raycast>().balls[count - 1]);
            Camera.GetComponent<raycast>().balls.RemoveAt(count - 1);

            if (Camera.GetComponent<raycast>().balls.Count < 9)
            {
                PointList.GetComponent<ContentSizeFitter>().enabled = false;
                PointList.transform.position = PointList.transform.parent.position;
                PointList.GetComponent<RectTransform>().sizeDelta = PointList.transform.parent.GetComponent<RectTransform>().sizeDelta; //new Vector2(150, 166);
            }
        }
    }
}
