using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullManager : MonoBehaviour {
    public List<GameObject> PullObjects=new List<GameObject>();
    public List<string> PullObjectsNames = new List<string>();
	// Use this for initialization
	void Start ()
    {
        foreach (Transform child in transform)
        { 
            PullObjects.Add(child.gameObject);
            PullObjectsNames.Add(child.gameObject.name);

        }
	}
	

	public void ResetScene()
    {
        for(int i=0;i<PullObjects.Count;++i)
        {
            PullObjects[i].SetActive(false);
        }
    }

    public void Add (GameObject obj)
    {
        PullObjects.Add(obj);
    }

    public GameObject Find(string Name)
    {
        for (int i = 0; i < PullObjects.Count; ++i)
        {
            if (PullObjectsNames[i] == Name)
                return PullObjects[i];
            
        }
        return null;
    }

    public GameObject Find(int n)
    {
        return PullObjects[n];
    }
}
