using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class BuilderInterface : MonoBehaviour
{
 
    public static UIComplexCommand ActiveCommand;
    SlotScript CommandBuilder;


    // Use this for initialization

    public void SaveNewComplexCommand()
    {
        if (CommandBuilder.UICommandElements.Count == 0)
            return;
        UIComplexCommand newCommand = Instantiate(CommandBuilder.Commandprefab, GameObject.Find("AvaibleCommandsField").transform).GetComponent<UIComplexCommand>();
        for (int i = 0; i < CommandBuilder.UICommandElements.Count; ++i)
        {

            newCommand.UICommandElements.Add(Instantiate<UICommand>(CommandBuilder.UICommandElements[i], newCommand.transform) as UICommand);
            newCommand.UICommandElements[i].name = CommandBuilder.UICommandElements[i].name;
            newCommand.UICommandElements[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < CommandBuilder.CommandsSet.Count; ++i)
            newCommand.CommandsSet.Add(CommandBuilder.CommandsSet[i]);
        newCommand.GetComponentInChildren<Text>().text = CommandBuilder.CommandName.text;
        
        ActiveCommand = newCommand;
        SerializeToJson(ActiveCommand);
        CommandBuilder.ResetBuilder();

    }
    public void Rewrite()
    {
        // ActiveCommand = GameObject.Find(item.ToString()).GetComponent<UIComplexCommand>(); ;
        // SerializeToJson(ActiveCommand);
        // ++item;
        DeserializeJson();

        //SaveNewComplexCommand();

        // SaveNewComplexCommand().transform.SetSiblingIndex(ActiveCommand.transform.GetSiblingIndex());
        // Destroy(ActiveCommand.gameObject);

        //Command newO= SerializeExtension.DeserializeString();
        //Instantiate(newO);
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
        CommandBuilder = GameObject.Find("CommandBuilder").GetComponent<SlotScript>();
    }


    public void SerializeToJson(UIComplexCommand com)
    {

        //FileStream file = File.Create(Application.persistentDataPath + "/save.json");


        File.WriteAllText(Application.persistentDataPath + "/save.json", JsonUtility.ToJson(com));

    }
    //public void SerializeToJson(List<Command> set)
    //{
    //    JsonUtility.ToJson(set);
    //    File.WriteAllText(Application.persistentDataPath + "/save.json", JsonUtility.ToJson(BuilderInterface.ActiveCommand));

    //}
    public void DeserializeJson()
    {
        UIComplexCommand newCommand = Instantiate(CommandBuilder.Commandprefab.gameObject, GameObject.Find("AvaibleCommandsField").transform).GetComponent<UIComplexCommand>();
        string str = File.ReadAllText(Application.persistentDataPath + "/save.json");
        JsonUtility.FromJsonOverwrite(str, newCommand);
        Debug.Log("deser " + newCommand.UICommandElements.Count);
        for (int i = 0; i < newCommand.UICommandElements.Count; ++i)
        {

            newCommand.UICommandElements[i] = Instantiate<UICommand>(ActiveCommand.UICommandElements[i], newCommand.transform) as UICommand;
            newCommand.UICommandElements[i].gameObject.SetActive(false);
        }


    }
}
[System.Serializable]
public  class ComplexScenario
{
    public List<Command> Scenario;
}