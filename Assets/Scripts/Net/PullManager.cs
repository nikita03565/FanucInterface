using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PullManager : MonoBehaviour {
    List<GameObject> PullObjects=new List<GameObject>();
	// Use this for initialization
	void Start () {
		foreach( Transform i in this.GetComponentsInChildren<Transform>())
        {
            PullObjects.Add(i.gameObject);
        }
	}
	

	public void ResetScene()
    {
        for(int i=0;i<PullObjects.Count;++i)
        {
            PullObjects[i].SetActive(false);
        }
    }
    public GameObject Find(string Name)
    {
        for (int i = 0; i < PullObjects.Count; ++i)
        {
            if (PullObjects[i].name == Name)
                return PullObjects[i];
            
        }
        return null;
    }
}
