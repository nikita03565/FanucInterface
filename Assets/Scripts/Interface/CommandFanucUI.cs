using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandFanucUI : MonoBehaviour
{
    public CommandFanuc command;
    protected Button ConfirmButton;
    //public int mode; // 0 - joint, 2 - world
    //public float[] coord;
    //public int grasp; // 1 - grasp, 0 - do nothing, -1 - ungrasp
    public ToggleGroup ModeGroup;
    public ToggleGroup GraspGroup;
    public InputField coordField;
    public InputField timeField;
    public InputField energyField;
    public InputField nameField;
    float[] coord = new float[6];
    string objName;

    public void Start()
    {
        
        ConfirmButton = this.transform.Find("Confirm").GetComponent<Button>();
        ConfirmButton.onClick.AddListener(() => OnConfirm());

        ModeGroup = this.transform.Find("Mode Toggle Group").GetComponent<ToggleGroup>();
        ModeGroup.SetActive(0);

        GraspGroup = this.transform.Find("Grasp Toggle Group").GetComponent<ToggleGroup>();
        GraspGroup.SetActive(2);
        coordField = this.transform.Find("SetCoordField").GetComponent<InputField>();
        timeField = this.transform.Find("SetTimeField").GetComponent<InputField>();
        energyField = this.transform.Find("SetEnergyField").GetComponent<InputField>();
        nameField = this.transform.Find("ObjNameField").GetComponent<InputField>();
        //coordField.onEndEdit.AddListener(delegate { LockInput(coordField); });
    }

    internal void show()
    {
        this.gameObject.SetActive(true);
        SceneManager.dropdownSceneObjects.gameObject.SetActive(true);
    }

    public bool LockInput(InputField input)
    {
        try
        {
            if (input.text.Contains(","))
            {
                input.text = "Don't use commas";
                throw new System.Exception();
            }
            if (input.text.Length > 0)
            {
                var arr = input.text.Split();

                if (arr.Length == 6)
                {

                    for (int i = 0; i < 6; ++i)
                    {
                        coord[i] = float.Parse(arr[i]);
                    }
                    return true;
                }
                else
                {
                    input.text = "6 coords are required";
                    throw new System.Exception();
                }
            }
            else
            {
                input.text = "You must write something";
                throw new System.Exception();
            };
        }
        catch (System.Exception)
        {

            return false;
        }
        
    }

    public void DoCommand()
    {
        SceneManager.fanuc.mode = command.mode;
        StartCoroutine(SceneManager.fanuc.Move(coord));
        //throw new System.NotImplementedException();
    }

    public void OnConfirm()
    {
        var toggle = ModeGroup.GetActive();
        if (toggle.name == "Mode1")
            command.mode = 0;
        else if (toggle.name == "Mode2")
            command.mode = 2;

        var GraspToggle = GraspGroup.GetActive();
        if (GraspToggle.name == "Grasp0")
            command.grasp = 0;
        else if (GraspToggle.name == "Grasp+")
            command.grasp = 1;
        else if (GraspToggle.name == "Grasp-")
            command.grasp = -1;
        if (timeField.text.Length != 0)
        {
            command.time = int.Parse(timeField.text);
        }
        if (energyField.text.Length != 0)
        {
            command.energy = int.Parse(energyField.text);
        }
        if (nameField.text.Length != 0)
        {
            objName = nameField.text;
            if (LockInput(coordField) && (coordField.text.Length != 0))
            {
                command.command = RobotCommands.FanucMoving(coordField.text, objName);
                Debug.Log(RobotCommands.FanucMoving(coordField.text, objName));
                ModeGroup.SetActive(0);
                coordField.text = "";
                nameField.text = "";
                GraspGroup.SetActive(2);
                //DoCommand();
                this.gameObject.SetActive(false);
                SceneManager.dropdownSceneObjects.gameObject.SetActive(false);
            }
        }

        if (LockInput(coordField) && coordField.text.Length != 0)
        {
            command.command = RobotCommands.FanucMoving(coordField.text);

            ModeGroup.SetActive(0);
            coordField.text = "";
            GraspGroup.SetActive(2);
            DoCommand();
            this.gameObject.SetActive(false);
            SceneManager.dropdownSceneObjects.gameObject.SetActive(false);
        }
           
        //throw new System.NotImplementedException();
    }

    public void Save()
    {
        Debug.Log("FANUCSAVED!");
        Destroy(this.gameObject);
        //throw new System.NotImplementedException();
    }

    public void Load()
    {
        throw new System.NotImplementedException();
    }

    public void CloseWindow()
    {
        SceneManager.FanucSettingsPanel.gameObject.SetActive(false);
        SceneManager.dropdownSceneObjects.gameObject.SetActive(false);
    }

}
