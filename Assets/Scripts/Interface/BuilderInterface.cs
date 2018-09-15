using System.Collections;
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
        UIComplexCommand newCommand = Instantiate(CommandBuilder.ComplexCommandPrefab, GameObject.Find("AvaibleCommandsField").transform).GetComponent<UIComplexCommand>();
        
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
            { //newCommand.CommandsSet.AddRange(newCommand.UICommandElements[i].GetComponent<UIComplexCommand>().CommandsSet);
                for (int j = 0; j < newCommand.UICommandElements[i].GetComponent<UIComplexCommand>().CommandsSet.Count; ++j)
                    newCommand.CommandsSet.Add(newCommand.UICommandElements[i].GetComponent<UIComplexCommand>().CommandsSet[j]);

            }


            else newCommand.CommandsSet.Add(newCommand.UICommandElements[i].command);
        // for (int i = 0; i < CommandBuilder.CommandsSet.Count; ++i)
        //newCommand.CommandsSet.Add(CommandBuilder.CommandsSet[i]);
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
       
            
        //for(int i=0;i<CommandBuilder.CommandsSet.Count;++i)
        //{
        //    Command.Copy(ActiveCommand.CommandsSet[i], CommandBuilder.CommandsSet[i]);
        //}
        CommandBuilder.ResetBuilder();
        //if(CommandBuilder.UICommandElements.Count<ActiveCommand.UICommandElements.Count)
        //{
        //    ActiveCommand.UICommandElements.Remove(ActiveCommand.UICommandElements[2]);
        //    ActiveCommand.CommandsSet.Remove(ActiveCommand.CommandsSet[2]);
        //}
        SceneManager.avaibleCommands.RewriteAllrefs(ActiveCommand, SavedCommand);
        
        
        //for (int i = 0, j = 0; i + j <= ActiveCommand.UICommandElements.Count + SavedCommand.UICommandElements.Count - 2;)
        //{

        //    if (i == j)
        //    {
        //        ActiveCommand.UICommandElements[i].command.CommandToSend = SavedCommand.UICommandElements[i].command.CommandToSend;
        //        ActiveCommand.UICommandElements[i].command.parallel = SavedCommand.UICommandElements[i].command.parallel;
        //        ActiveCommand.UICommandElements[i].command.time = SavedCommand.UICommandElements[i].command.time;
        //        ActiveCommand.UICommandElements[i].command.energy = SavedCommand.UICommandElements[i].command.energy;
        //    }
        //    else if (i < j) ActiveCommand.UICommandElements.Add(Instantiate(SavedCommand.UICommandElements[j]));
        //    else if (i > j) ActiveCommand.UICommandElements.RemoveAt(i);
        //    Debug.Log(i + "  " + j);
        //    if (i < ActiveCommand.UICommandElements.Count - 1) ++i;
        //    if (j < SavedCommand.UICommandElements.Count - 1) ++j;

        //    ActiveCommand.
        // }
        //Destroy(ActiveCommand.gameObject);


        RewriteButton.gameObject.SetActive(false);
    }
    public void Execute()
    {
        ComplexScenario ListToSend=new ComplexScenario();
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

        for (int i=0;i<Files.Length;++i)
        {
            Debug.Log(Files[i]);
            DeserializeJson(Files[i]);
        }
       // SceneManager.avaibleCommands.RewriteRefs();
    }


    public void SerializeToJson(UIComplexCommand com)
    {
        System.IO.Directory.CreateDirectory( Application.persistentDataPath + "/" + com.GetComponentInChildren<Text>().text);
        Debug.Log("ss-s-s-s-s-SAVED");
        com.IndexUp();
        File.WriteAllText(Application.persistentDataPath+"/" +com.GetComponentInChildren<Text>().text+".json", JsonUtility.ToJson(com));
        //3 here is number of standart defolt childs(text, sett, del)
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
          
            if(CommandinJson.Substring(CommandinJson.IndexOf("isComplex")+11,4)=="true")
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
