using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class UICommand : MonoBehaviour
{
    protected CommandBuilder CommandBuilder;
    public  Button SettingsButton;
    public  Button DeleteButton;
    public Command command;
    public string CommandName;

    public static int UISize = 0;
    public static float UIScale = 0;
    public bool isOriginal= true;
    public  bool isComplex = false;

    // Use this for initialization
    public virtual void Start()
    {
        //Debug.Log("Staaart "+this.gameObject.name);
        CommandName = this.GetComponentInChildren<Text>().text;
        CommandBuilder = GameObject.Find("CommandBuilder").GetComponent<CommandBuilder>();
        if (isOriginal)
        {
            this.GetComponent<Button>().onClick.AddListener(() => Add());
        }
        if (UISize == 0)
        {
            UISize = Mathf.FloorToInt(this.gameObject.GetComponent<RectTransform>().rect.height);
        }

        if (UIScale == 0)
        {
            UIScale = this.gameObject.GetComponent<RectTransform>().localScale.y;
        }

        DeleteButton = this.transform.Find("DeleteButton").GetComponent<Button>();
        DeleteButton.onClick.RemoveAllListeners();
        DeleteButton.onClick.AddListener(()=>Delete());
        
        SettingsButton = this.transform.Find("Settings").GetComponent<Button>();
        SettingsButton.onClick.RemoveAllListeners();
        SettingsButton.onClick.AddListener(() => Settings()); 
    }

    protected virtual void Settings()
    {

        command.GetWindow();
    }

    public virtual int GetNumberofCommands()
    {
        return 1;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    public virtual void Add()
    {
        UICommand NewCommand= Instantiate (this.gameObject,this.transform.parent).GetComponent<UICommand>();
        

        NewCommand.name = this.name;
        switch (this.gameObject.name)
        {
            case "1":
                {
                    NewCommand.command = new CommandFanuc() as CommandFanuc;
                    break;
                }
            case "2":
                {
                    NewCommand.command = new CommandCamera() as CommandCamera;
                    break;
                }
            case "3":
                {
                    NewCommand.command = new CommandTelega() as CommandTelega;
                    break;
                }
        }
        Command.Copy(NewCommand.command, this.command);
        NewCommand.transform.SetParent(CommandBuilder.transform);
        NewCommand.gameObject.AddComponent<DragDrop>();
        NewCommand.isOriginal = false;
       
        NewCommand.SettingsButton.interactable = true;
        NewCommand.DeleteButton.interactable = true;
        CommandBuilder.AddUIElementToGroup(NewCommand.GetComponent<UICommand>());
    }
    public virtual void Copy(UIComplexCommand com)
    {
        Debug.Log("there is NO copy");
    }
    public virtual void Delete()
    {
        CommandBuilder.UIElementRemoveFromGroup(this);
        Destroy(this.gameObject);
    }
}
