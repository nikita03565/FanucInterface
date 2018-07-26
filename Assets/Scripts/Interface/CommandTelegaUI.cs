﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandTelegaUI : MonoBehaviour
{
    public CommandTelega command;
    protected Button ConfirmButton;
    //public int mode; //0 - directional, 1 - parallel
    //public float[] coord; // x, y, yaw
    public ToggleGroup ModeGroup;
    public InputField inputField;

    public void Start()
    {
        ConfirmButton = this.transform.Find("Confirm").GetComponent<Button>();
        ConfirmButton.onClick.AddListener(() => OnConfirm());
        ModeGroup = this.transform.Find("Mode Toggle Group").GetComponent<ToggleGroup>();
        ModeGroup.SetActive(0);
        inputField = this.transform.Find("SetCoordField").GetComponent<InputField>();
        inputField.onEndEdit.AddListener(delegate { LockInput(inputField); });
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

                if (arr.Length == 3)
                {
                    for (int i = 0; i < 3; ++i)
                    {
                        float.Parse(arr[i]);
                    }
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
            input.text = "Wrong string. Try again";
        }
    }

    internal void show()
    {
        this.gameObject.SetActive(true);
    }

    public void DoCommand()
    {
        throw new System.NotImplementedException();
    }

    public void Save()
    {
        Debug.Log("TELEGASAVED!");
        //Destroy(this.gameObject);
        //throw new System.NotImplementedException();
    }

    public void Load()
    {
        throw new System.NotImplementedException();
    }

    public void OnConfirm()
    {
        var toggle = ModeGroup.GetActive();
        if (toggle.name == "Mode1")
            command.mode = 0;
        else if (toggle.name == "Mode2")
            command.mode = 1;
        if (inputField.text != "Wrong string" && inputField.text.Length != 0)
        {
            //command.CommandToSend = RobotCommands.TelegaMoving(inputField.text);
            ModeGroup.SetActive(0);
            inputField.text = "";
            this.gameObject.SetActive(false);
        }
    }
}