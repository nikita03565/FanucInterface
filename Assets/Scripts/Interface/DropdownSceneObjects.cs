using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DropdownSceneObjects : MonoBehaviour {
    public Dropdown CameraList;
	// Use this for initialization
	void Start () {
        List<string> tmp = new List<string>();
        for(int i=0;i<SceneManager.Pull.PullObjects.Count;++i)
        {
            tmp.Add(SceneManager.Pull.PullObjects[i].name);
        }

        CameraList.AddOptions(tmp);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
