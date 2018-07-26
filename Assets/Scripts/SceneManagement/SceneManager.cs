using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour {
    public static CommandFanucUI FanucSettingsPanel;
    public static CommandCameraUI CameraSettingsPanel;
    public static CommandTelegaUI TelegaSettingsPanel;
    public static GameObject ScenarioEditor;
    public static NetConnection Net;
    public static PullManager Pull;
   
    // Use this for initialization
    void Start () {
        FanucSettingsPanel = FindObjectOfType<CommandFanucUI>();
        FanucSettingsPanel.gameObject.SetActive(false);
        CameraSettingsPanel = FindObjectOfType<CommandCameraUI>();
        CameraSettingsPanel.gameObject.SetActive(false);
        TelegaSettingsPanel = FindObjectOfType<CommandTelegaUI>();
        TelegaSettingsPanel.gameObject.SetActive(false);
        Net = FindObjectOfType<NetConnection>();
        //Net.gameObject.SetActive(false);
        Pull = FindObjectOfType<PullManager>();
        
        ScenarioEditor = GameObject.Find("ScenarioEditor");
        ScenarioEditor.SetActive(false);
        
    }
	
	public void ShowScenarioEditor()
    {
       
        ScenarioEditor.SetActive(!ScenarioEditor.activeInHierarchy);
    }
}
