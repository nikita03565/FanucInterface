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
    public static AvailableCommands avalaibleCommands;
    public static DropdownSceneObjects dropdownSceneObjects;
    public static FanucScript fanuc;
    public static TelegaManager telega;
    public static BuilderInterface builderInterface;
    public static InstantiateFromCam SceneSynchronization;
    public static bool UserControlLock=false;
     
    public static bool ObserverMode;
    bool Timer = true;
    //add SlotScript

    // Use this for initialization
    void Start ()
    {
        if (Net = FindObjectOfType<NetConnection>())
        {
            Net.transform.SetParent(FindObjectOfType<Canvas>().transform);
            ////Net.Sender("ARRRRRRR");
            Net.gameObject.SetActive(true);
            Debug.Log(Net);
            ObserverMode = Net.observerMode;
        }
        builderInterface = FindObjectOfType<BuilderInterface>();
        telega = FindObjectOfType<TelegaManager>();
        FanucSettingsPanel = FindObjectOfType<CommandFanucUI>();
        FanucSettingsPanel.gameObject.SetActive(false);
        CameraSettingsPanel = FindObjectOfType<CommandCameraUI>();
        CameraSettingsPanel.gameObject.SetActive(false);
        TelegaSettingsPanel = FindObjectOfType<CommandTelegaUI>();
        TelegaSettingsPanel.gameObject.SetActive(false);
        avalaibleCommands = FindObjectOfType<AvailableCommands>();
        Pull = FindObjectOfType<PullManager>();        
        ScenarioEditor = GameObject.Find("ScenarioEditor");
        ScenarioEditor.SetActive(false);
        dropdownSceneObjects = FindObjectOfType<DropdownSceneObjects>();
        dropdownSceneObjects.gameObject.SetActive(false);
        fanuc = FindObjectOfType<FanucScript>();
        SceneSynchronization = FindObjectOfType<InstantiateFromCam>();
        telega.telega.Type = telegaScript.MoveType.RotateBarsAndMove;
        if (ObserverMode)
        {
            CloseAllUI();
            UserControlLock = true;
            fanuc.mode = 0;
            telega.telega.Type = telegaScript.MoveType.RotateBarsAndMove;

        }

    }
	
    public void CloseAllUI()
    {
        foreach (Image UI in  GameObject.FindObjectsOfType<Image>())
        {
            UI.gameObject.SetActive(false);
        }
    }
	public void ShowScenarioEditor()
    {      
        ScenarioEditor.SetActive(!ScenarioEditor.activeInHierarchy);
        UserControlLock = !UserControlLock;
    }
    public IEnumerator SendingLimiter(string Message, float Delay)
    {
        Timer = false;
        Net.Sender(Message);
        yield return new WaitForSeconds(Delay);
        Timer = true;
    }
    void Update()
    {
        
        if (ObserverMode&&Timer)
        {
            StartCoroutine(SendingLimiter(RobotCommands.GetSceneInf(), 1f));
           
        }

    }

}

