﻿using System.Collections;
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
    public InputField inputField;

    public void Start()
    {
        
        ConfirmButton = this.transform.Find("Confirm").GetComponent<Button>();
        ConfirmButton.onClick.AddListener(() => OnConfirm());

        ModeGroup = this.transform.Find("Mode Toggle Group").GetComponent<ToggleGroup>();
        ModeGroup.SetActive(0);

        GraspGroup = this.transform.Find("Grasp Toggle Group").GetComponent<ToggleGroup>();
        GraspGroup.SetActive(2);
        inputField = this.transform.Find("SetCoordField").GetComponent<InputField>();
        inputField.onEndEdit.AddListener(delegate { LockInput(inputField); });
    }

    internal void show()
    {
        this.gameObject.SetActive(true);
    }

    public void LockInput(InputField input)
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

                if (arr.Length == 6)
                {
                    for (int i = 0; i < 6; ++i)
                        float.Parse(arr[i]);
                }
                else
                {
                    throw new System.Exception();
                }
            }
            else
            {
                throw new System.Exception();
            }
        }
        catch (System.Exception)
        {
            Debug.Log("Wrong string");
            input.text = "Wrong string";
        }
    }

    public void DoCommand()
    {
        throw new System.NotImplementedException();
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
        if (inputField.text != "Wrong string" && inputField.text.Length != 0)
        {

            command.CommandToSend = RobotCommands.FanucMoving(inputField.text);

            ModeGroup.SetActive(0);
            inputField.text = "";
            GraspGroup.SetActive(2);
            this.gameObject.SetActive(false);
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

}