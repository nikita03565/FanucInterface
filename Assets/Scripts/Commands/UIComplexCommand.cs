using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UIComplexCommand :UICommand
{
    private static int saveIndex = 0;
    public int localSaveIndex = 0;
    public List<UICommand> UICommandElements = new List<UICommand>();
    public List<Command> CommandsSet = new List<Command>();
    
    public int IndexUp()
    {
        ++saveIndex;
        return localSaveIndex = saveIndex;
    }

    public UIComplexCommand(UIComplexCommand com)
    {
        this.UICommandElements = new List<UICommand>(com.UICommandElements);
        this.CommandsSet = new List<Command>(com.CommandsSet);
    }

    // Use this for initialization
    override public void Start()
    {
        CommandBuilder = GameObject.Find("CommandBuilder").GetComponent<SlotScript>();
        if (isOriginal)
        {
            this.GetComponent<Button>().onClick.AddListener(() => Add());
        }
        DeleteButton = this.transform.Find("DeleteButton").GetComponent<Button>();
        DeleteButton.onClick.AddListener(() => Delete());
        SettingsButton = this.transform.Find("Settings").GetComponent<Button>();
        SettingsButton.onClick.AddListener(() => Settings());        
        isComplex = true;
    }

    protected override void Settings()
    {
        //CommandBuilder.ResetBuilder();
        BuilderInterface.ActiveCommand = this;
      
        UICommand[] newObjs = new UICommand[UICommandElements.Count];
        for (int i = 0; i < UICommandElements.Count; ++i)
        {
            newObjs[i] = Instantiate(UICommandElements[i]);
            newObjs[i].name = UICommandElements[i].name;
            switch (newObjs[i].name)
            {
                case "1":
                    {
                        newObjs[i].command = new CommandFanuc(UICommandElements[i].command);
                        break;
                    }
                case "2":
                    {
                        newObjs[i].command = new CommandCamera(UICommandElements[i].command);
                        break;
                    }
                case "3":
                    {
                        newObjs[i].command = new CommandTelega(UICommandElements[i].command);
                        break;
                    }
            }
            newObjs[i].Start();
            newObjs[i].gameObject.SetActive(true);
        }
        CommandBuilder.ResetBuilder();
        for (int i = 0; i < UICommandElements.Count; ++i)
        {
            newObjs[i].transform.SetParent(CommandBuilder.transform);
            newObjs[i].transform.localScale = new Vector3(UICommand.UIScale, UICommand.UIScale);
            CommandBuilder.AddUIElementToGroup(newObjs[i]);
        }
        BuilderInterface.RewriteButton.gameObject.SetActive(true);

    }

    public override int GetNumberofCommands()
    {
        return CommandsSet.Count;
    }

    public Command GetCommandFromSet(int index)
    {
        return CommandsSet[index];
    }
    
    public override void Add()
    {
        UIComplexCommand NewCommand = Instantiate(CommandBuilder.ComplexCommandPrefab, CommandBuilder.transform).GetComponent<UIComplexCommand>();
        NewCommand.CommandsSet = this.CommandsSet;        
        NewCommand.transform.SetParent(CommandBuilder.transform);
        NewCommand.transform.localScale = new Vector3(UIScale, UIScale);
        CommandBuilder.AddUIElementToGroup(NewCommand.GetComponent<UICommand>());
        NewCommand.gameObject.AddComponent<DragDrop>();
        NewCommand.isOriginal = false;
        NewCommand.SettingsButton.interactable = false;
        NewCommand.GetComponentInChildren<Text>().text = this.GetComponentInChildren<Text>().text;
        NewCommand.CommandName = this.CommandName;
    }
   
    public override void Delete()
    {
        CommandBuilder.UIElementRemoveFromGroup(this, true);
        Destroy(this.gameObject);
    }
}
