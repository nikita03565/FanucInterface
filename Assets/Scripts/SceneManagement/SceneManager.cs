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
    public static AvailableCommands avaibleCommands;
    public static DropdownSceneObjects dropdownSceneObjects;
    public static FanucScript fanuc;
    public static TelegaManager telega;
    public static bool UserControlLock=false;

    //add SlotScript

    // Use this for initialization
    void Start ()
    {
        telega = FindObjectOfType<TelegaManager>();

        FanucSettingsPanel = FindObjectOfType<CommandFanucUI>();
        FanucSettingsPanel.gameObject.SetActive(false);

        CameraSettingsPanel = FindObjectOfType<CommandCameraUI>();
        CameraSettingsPanel.gameObject.SetActive(false);

        TelegaSettingsPanel = FindObjectOfType<CommandTelegaUI>();
        TelegaSettingsPanel.gameObject.SetActive(false);

        avaibleCommands = FindObjectOfType<AvailableCommands>();

        Net = FindObjectOfType<NetConnection>();
        Debug.Log(Net);
        //Net.transform.SetParent(FindObjectOfType<Canvas>().transform);
        // Net.Sender("ARRRRRRR");
        //Net.gameObject.SetActive(true);

        Pull = FindObjectOfType<PullManager>();
        
        ScenarioEditor = GameObject.Find("ScenarioEditor");
        ScenarioEditor.SetActive(false);

        dropdownSceneObjects = FindObjectOfType<DropdownSceneObjects>();
        dropdownSceneObjects.gameObject.SetActive(false);

        fanuc = FindObjectOfType<FanucScript>();
    }
	
	public void ShowScenarioEditor()
    {      
        ScenarioEditor.SetActive(!ScenarioEditor.activeInHierarchy);
        UserControlLock = !UserControlLock;
    }


}
