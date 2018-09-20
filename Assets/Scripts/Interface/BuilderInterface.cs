﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class BuilderInterface : MonoBehaviour
{
    public static UIComplexCommand ActiveCommand;
    public UIComplexCommand SavedCommand;
    SlotScript CommandBuilder;
    public static Button RewriteButton;

    // Use this for initialization

    public void SaveNewComplexCommand()
    {
        if (CommandBuilder.UICommandElements.Count == 0)
            return;
        UIComplexCommand newCommand = Instantiate(CommandBuilder.ComplexCommandPrefab, SceneManager.avaibleCommands.transform).GetComponent<UIComplexCommand>();
        
        for (int i = 0; i < CommandBuilder.UICommandElements.Count; ++i)
        {
            newCommand.UICommandElements.Add(Instantiate<UICommand>(CommandBuilder.UICommandElements[i], newCommand.transform) as UICommand);
            if (newCommand.UICommandElements[i].isComplex)
            {
                newCommand.UICommandElements[i].GetComponent<UIComplexCommand>().UICommandElements = CommandBuilder.UICommandElements[i].GetComponent<UIComplexCommand>().UICommandElements;
                newCommand.UICommandElements[i].GetComponent<UIComplexCommand>().CommandsSet = CommandBuilder.UICommandElements[i].GetComponent<UIComplexCommand>().CommandsSet;
            }
            else newCommand.UICommandElements[i].command = CommandBuilder.UICommandElements[i].command;
            newCommand.UICommandElements[i].name = CommandBuilder.UICommandElements[i].name;
            newCommand.UICommandElements[i].CommandName = CommandBuilder.UICommandElements[i].CommandName;
            newCommand.UICommandElements[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < newCommand.UICommandElements.Count; ++i)
            if ((newCommand.UICommandElements[i].isComplex))
            { 
                for (int j = 0; j < newCommand.UICommandElements[i].GetComponent<UIComplexCommand>().CommandsSet.Count; ++j)
                    newCommand.CommandsSet.Add(newCommand.UICommandElements[i].GetComponent<UIComplexCommand>().CommandsSet[j]);

            }
            else newCommand.CommandsSet.Add(newCommand.UICommandElements[i].command);
        newCommand.GetComponentInChildren<Text>().text = CommandBuilder.CommandName.text;
        newCommand.CommandName = CommandBuilder.CommandName.text;
        
        SavedCommand = newCommand;
        SerializeToJson(SavedCommand);
        CommandBuilder.ResetBuilder();
        SceneManager.avaibleCommands.AvaibleCommandsSet.Add(SavedCommand);

    }

    public void Rewrite()
    {
        SaveNewComplexCommand();
        CommandBuilder.ResetBuilder();
        SceneManager.avaibleCommands.RewriteAllrefs(ActiveCommand, SavedCommand);
        RewriteButton.gameObject.SetActive(false);
    }

    public void Execute()
    {
        ComplexScenario ListToSend = new ComplexScenario();
        ListToSend.Scenario = new List<Command>(CommandBuilder.CommandsSet);
        Debug.Log(JsonUtility.ToJson(ListToSend));
        
        SceneManager.Net.Sender(JsonUtility.ToJson(ListToSend)) ;
        
        for (int i = 0; i < CommandBuilder.CommandsSet.Count; ++i)
        {     
            CommandBuilder.CommandsSet[i].DoCommand();
        }
    }

    void Start()
    {
        RewriteButton = GameObject.Find("RewriteButton").GetComponent<Button>();
        CommandBuilder = GameObject.Find("CommandBuilder").GetComponent<SlotScript>();
        RewriteButton.gameObject.SetActive(false);
        string[] Files= Directory.GetFiles(Application.persistentDataPath,"*.json");

        for (int i = 0;i < Files.Length; ++i)
        {
            Debug.Log(Files[i]);
            DeserializeJson(Files[i]);
        }
    }
    
    public void SerializeToJson(UIComplexCommand com)
    {
        System.IO.Directory.CreateDirectory( Application.persistentDataPath + "/" + com.GetComponentInChildren<Text>().text);
        Debug.Log("ss-s-s-s-s-SAVED");
        com.IndexUp();
        File.WriteAllText(Application.persistentDataPath+"/" +com.GetComponentInChildren<Text>().text+".json", JsonUtility.ToJson(com));
        //3 here is number of standart default childs(text, sett, del)
        for (int i = 0; i < com.transform.childCount-3; ++i)
        {
            File.WriteAllText(Application.persistentDataPath + "/" + com.GetComponentInChildren<Text>().text + "/" + i+ ".json", JsonUtility.ToJson(com.transform.GetChild(i+3).GetComponent<UICommand>()));
        }
    }
   
    public void DeserializeJson(string JsonPath)
    { 
        UIComplexCommand newCommand = Instantiate(CommandBuilder.ComplexCommandPrefab.gameObject, GameObject.Find("AvaibleCommandsField").transform).GetComponent<UIComplexCommand>();
        string str = File.ReadAllText(JsonPath);
        //Debug.Log(str);
        JsonUtility.FromJsonOverwrite(str, newCommand);
        string name = JsonPath.Substring(Application.persistentDataPath.Length+1);
        name = name.Remove(name.Length - 5, 5);
        newCommand.GetComponentInChildren<Text>().text = name;
        string[] SubFiles = Directory.GetFiles(Application.persistentDataPath+"/"+name, "*.json");
       
        for (int i = 0; i < SubFiles.Length; ++i)
        {
            //Read from json string which prefab will be used
            string CommandinJson = File.ReadAllText(SubFiles[i]);
          
            if(CommandinJson.Substring(CommandinJson.IndexOf("isComplex")+11, 4) == "true")
                newCommand.UICommandElements[i] = Instantiate(CommandBuilder.ComplexCommandPrefab.gameObject, newCommand.transform).GetComponent<UIComplexCommand>();
            else
                newCommand.UICommandElements[i] = Instantiate(CommandBuilder.CommandPrefab.gameObject, newCommand.transform).GetComponent<UICommand>();


            JsonUtility.FromJsonOverwrite(File.ReadAllText(SubFiles[i]), newCommand.UICommandElements[i]);
            newCommand.UICommandElements[i].gameObject.name = newCommand.UICommandElements[i].CommandName;
            newCommand.UICommandElements[i].GetComponentInChildren<Text>().text= newCommand.UICommandElements[i].CommandName;
            newCommand.UICommandElements[i].gameObject.SetActive(false);
        }
        if(newCommand.localSaveIndex>SceneManager.avaibleCommands.AvaibleCommandsSet.Count)
            SceneManager.avaibleCommands.AvaibleCommandsSet.Insert(SceneManager.avaibleCommands.AvaibleCommandsSet.Count, newCommand);
        else
            SceneManager.avaibleCommands.AvaibleCommandsSet.Insert(newCommand.localSaveIndex, newCommand);

        SceneManager.avaibleCommands.SetSavedOrderIndex(newCommand);
    }
}

[System.Serializable]
public  class ComplexScenario
{
    public List<Command> Scenario;
}