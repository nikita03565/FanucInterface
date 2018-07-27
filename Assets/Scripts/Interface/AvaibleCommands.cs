using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvaibleCommands : MonoBehaviour {
    public List<UIComplexCommand> AvaibleCommandsSet = new List<UIComplexCommand>();
	// Use this for initialization
	void Start () {
	 
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void RewriteAllrefs(UIComplexCommand com1, UIComplexCommand command)
    {
        foreach( UIComplexCommand com in AvaibleCommandsSet)
        {
            Debug.Log(com.name);
            for (int i = 0; i < com.UICommandElements.Count; ++i)
                if (com.UICommandElements[i] == com1)
                {
                    Debug.Log("Reference!");
                    com.UICommandElements[i] = command;
                }
        }
    }
}
