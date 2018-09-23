using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandCameraUI : MonoBehaviour
{
    public CommandCamera command;
    protected Button ConfirmButton;
    //public int mode; //0 - all scene, 1 - specific object
    //public string id;
    public ToggleGroup ModeGroup;
    public InputField inputField;

    public void Start()
    {
        ConfirmButton = this.transform.Find("Confirm").GetComponent<Button>();
        ConfirmButton.onClick.AddListener(() => OnConfirm());

        ModeGroup = this.transform.Find("Mode Toggle Group").GetComponent<ToggleGroup>();
        ModeGroup.SetActive(0);

        inputField = this.transform.Find("SetIdField").GetComponent<InputField>();
        inputField.onEndEdit.AddListener(delegate { LockInput(inputField); });
    }

    public void LockInput(InputField input)
    {
        command.id = input.text;
    }

    internal void show()
    {
        
        this.gameObject.SetActive(true);
    }

    public void DoCommand()
    {
        throw new System.NotImplementedException();
    }

    public void Load()
    {
        throw new System.NotImplementedException();
    }

    public void Save()
    {
        Debug.Log("CAMERASAVED!");
        Destroy(this.gameObject);
        //throw new System.NotImplementedException();
    }

    public void OnConfirm()
    {
        var toggle = ModeGroup.GetActive();
        if (toggle.name == "Mode1")
            command.mode = 0;
        else if (toggle.name == "Mode2")
            command.mode = 1;
        SceneManager.Pull.Add(new GameObject("CamResult"));
        if (inputField.text != "Wrong string" && (!(toggle.name == "Mode2") || inputField.text.Length != 0))
        {
            //command.CommandToSend = RobotCommands.TelegaMoving(inputField.text);
            ModeGroup.SetActive(0);
            inputField.text = "";
            this.gameObject.SetActive(false);
        }
        //throw new System.NotImplementedException();
    }

}
