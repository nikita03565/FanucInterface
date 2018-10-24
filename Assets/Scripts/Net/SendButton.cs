using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SendButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
        this.gameObject.GetComponent<Button>().onClick.AddListener(()=>StartCoroutine(Click()));
	}
	
	// Update is called once per frame
    IEnumerator Click()
    {
        this.gameObject.GetComponent<Button>().interactable = false;
        SceneManager.Net.Sender(RobotCommands.Sensors());
        yield return new WaitForSeconds(5);
        SceneManager.Net.Sender(RobotCommands.GetSceneInf());
        this.gameObject.GetComponent<Button>().interactable = true;

    }
   
    
}
