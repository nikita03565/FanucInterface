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
        UIComplexCommand newCommand = Instantiate(CommandBuilder.Commandprefab, GameObject.Find("AvaibleCommandsField").transform).GetComponent<UIComplexCommand>();
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
            newCommand.UICommandElements[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < CommandBuilder.CommandsSet.Count; ++i)
            newCommand.CommandsSet.Add(CommandBuilder.CommandsSet[i]);
        newCommand.GetComponentInChildren<Text>().text = CommandBuilder.CommandName.text;
        
        SavedCommand = newCommand;
        SerializeToJson(SavedCommand);
        CommandBuilder.ResetBuilder();
        SceneManager.avaibleCommands.AvaibleCommandsSet.Add(SavedCommand);

    }
    public void Rewrite()
    {
        SaveNewComplexCommand();
        Debug.Log(ActiveCommand.UICommandElements.Count + "   " + SavedCommand.UICommandElements.Count);
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
        Debug.Log(Files.Length);
        for (int i=Files.Length-1;i>=0;--i)
        {
            Debug.Log(Files[i]);
            DeserializeJson(Files[i]);
        }
    }


    public void SerializeToJson(UIComplexCommand com)
    {
        Debug.Log("ss-s-s-s-s-SAVED");
      //  File.Create(Application.persistentDataPath +"/" +com.GetComponentInChildren<Text>() + ".json");

        File.WriteAllText(Application.persistentDataPath+"/" +com.GetComponentInChildren<Text>().text+".json", JsonUtility.ToJson(com));
    }
   
    public void DeserializeJson(string JsonPath)
    {
        UIComplexCommand newCommand = Instantiate(CommandBuilder.Commandprefab.gameObject, GameObject.Find("AvaibleCommandsField").transform).GetComponent<UIComplexCommand>();
        string str = File.ReadAllText(JsonPath);
        JsonUtility.FromJsonOverwrite(str, newCommand);
        //Debug.Log("deser " + newCommand.UICommandElements.Count);
        string name = JsonPath.Substring(Application.persistentDataPath.Length+1);
        name = name.Remove(name.Length - 5, 5);
        newCommand.GetComponentInChildren<Text>().text = name;
        for (int i = 0; i < newCommand.UICommandElements.Count; ++i)
        {

           // newCommand.UICommandElements[i] = Instantiate<UICommand>(ActiveCommand.UICommandElements[i], newCommand.transform) as UICommand;
            //newCommand.UICommandElements[i].gameObject.SetActive(false);
        }
    }
}
[System.Serializable]
public  class ComplexScenario
{
    public List<Command> Scenario;
}