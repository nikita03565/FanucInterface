using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class BuilderInterface : MonoBehaviour
{
    public static UIComplexCommand activeCommand;
    public UIComplexCommand savedCommand;
    CommandBuilder commandBuilder;
    static Button rewriteButton;
    static Button cancelButton;
    //3 here is number of standart default childs(text, sett, del)
    const int DefaulChilds = 3;

    // Use this for initialization
    
    bool CheckName(InputField name)
    {
        
        if (commandBuilder.UICommandElements.Count == 0)
        {
            commandBuilder.CommandName.GetComponentInChildren<Text>().text = "List is empty!";
            commandBuilder.CommandName.GetComponent<Image>().color = new Color(1f, 0, 0, 0.5f);
            return false;
        }
        if (commandBuilder.CommandName.text == "")
        {
            commandBuilder.CommandName.GetComponentInChildren<Text>().text = "Name is empty!";
            commandBuilder.CommandName.GetComponent<Image>().color = new Color(1f, 0, 0, 0.5f);
           
            return false;
        }
        if (SceneManager.avalaibleCommands.Names.IndexOf(name.text) != -1)
        {
            commandBuilder.CommandName.GetComponentInChildren<Text>().text = "Name already exist!";
            commandBuilder.CommandName.GetComponent<Image>().color = new Color(1f, 0, 0, 0.5f);
            return false;
        }
        Regex nameRegExp = new Regex("[^A-Za-z0-9_-]+");
        if (nameRegExp.IsMatch(name.text)||name.text.IndexOf("flag")!=-1)
        {
            commandBuilder.CommandName.GetComponentInChildren<Text>().text = "Uncorrect symbol in the name!";
            commandBuilder.CommandName.GetComponent<Image>().color = new Color(1f, 0, 0, 0.5f);

            return false;
        }
            
        return true;
    }
    public void SaveNewComplexCommand()
    {
        if (!CheckName(commandBuilder.CommandName))
            return;
        UIComplexCommand newCommand = Instantiate(commandBuilder.ComplexCommandPrefab, SceneManager.avalaibleCommands.transform).GetComponent<UIComplexCommand>();
        
        for (int i = 0; i < commandBuilder.UICommandElements.Count; ++i)
        {
            newCommand.UICommandElements.Add(Instantiate<UICommand>(commandBuilder.UICommandElements[i], newCommand.transform) as UICommand);
            if (newCommand.UICommandElements[i].isComplex)
            {
                newCommand.UICommandElements[i].GetComponent<UIComplexCommand>().UICommandElements = commandBuilder.UICommandElements[i].GetComponent<UIComplexCommand>().UICommandElements;
                newCommand.UICommandElements[i].GetComponent<UIComplexCommand>().CommandsSet = commandBuilder.UICommandElements[i].GetComponent<UIComplexCommand>().CommandsSet;
            }
            else newCommand.UICommandElements[i].command = commandBuilder.UICommandElements[i].command;
            newCommand.UICommandElements[i].name = commandBuilder.UICommandElements[i].name;
            newCommand.UICommandElements[i].CommandName = commandBuilder.UICommandElements[i].CommandName;
            newCommand.UICommandElements[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < newCommand.UICommandElements.Count; ++i)
            if ((newCommand.UICommandElements[i].isComplex))
            { 
                for (int j = 0; j < newCommand.UICommandElements[i].GetComponent<UIComplexCommand>().CommandsSet.Count; ++j)
                    newCommand.CommandsSet.Add(newCommand.UICommandElements[i].GetComponent<UIComplexCommand>().CommandsSet[j]);

            }
            else newCommand.CommandsSet.Add(newCommand.UICommandElements[i].command);
        newCommand.GetComponentInChildren<Text>().text = commandBuilder.CommandName.text;
        newCommand.CommandName = commandBuilder.CommandName.text;
        
        savedCommand = newCommand;
        SerializeToJson(savedCommand);
        commandBuilder.ResetBuilder();
        SceneManager.avalaibleCommands.AvailableCommandsSet.Add(savedCommand);
        SceneManager.avalaibleCommands.Names.Add(savedCommand.CommandName);

    }
    public static void RewriteMode()
    {
        cancelButton.gameObject.SetActive(true);
        rewriteButton.gameObject.SetActive(true);
    }
    public void cancelRewriting()
    {
        cancelButton.gameObject.SetActive(false);
        rewriteButton.gameObject.SetActive(false);
        commandBuilder.ResetBuilder();
    }
    public void Rewrite()
    {
        SaveNewComplexCommand();
        SceneManager.avalaibleCommands.RewriteAllrefs(activeCommand, savedCommand);
        savedCommand.localSaveIndex = activeCommand.localSaveIndex;
        cancelRewriting();
    }

    public void Execute()
    {
        
        if(!CheckName(commandBuilder.CommandName))
            return;
        ComplexScenario ListToSend = new ComplexScenario();
        ListToSend.Scenario = new List<Command>(commandBuilder.CommandsSet);
        Debug.Log(JsonUtility.ToJson(ListToSend));
        ListToSend.flag = "0";
        ListToSend.name = commandBuilder.CommandName.text;
        SceneManager.Net.Sender(JsonUtility.ToJson(ListToSend)) ;
       // File.WriteAllText("C:/Users/virri/Desktop/1.json", JsonUtility.ToJson(ListToSend));
        
        for (int i = 0; i < commandBuilder.CommandsSet.Count; ++i)
        {     
            commandBuilder.CommandsSet[i].DoCommand();
        }
    }

    void Start()
    {
        cancelButton = GameObject.Find("CancelButton").GetComponent<Button>();
        rewriteButton = GameObject.Find("RewriteButton").GetComponent<Button>();
        commandBuilder = GameObject.Find("CommandBuilder").GetComponent<CommandBuilder>();


        rewriteButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(false);
        string[] Files= Directory.GetFiles(Application.persistentDataPath,"*.json");

        for (int i = 0;i < Files.Length; ++i)
        {
            Debug.Log(Files[i]);
            DeserializeJson(Files[i]);
        }
    }
    public void RewritetoJson(UIComplexCommand com)
    {
        Directory.Delete(Application.persistentDataPath + "/" + com.CommandName, true);
        File.Delete(Application.persistentDataPath + "/" + com.CommandName + ".json");
        System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/" + com.GetComponentInChildren<Text>().text);
        Debug.Log("ss-s-s-s-s-SAVED");
        File.WriteAllText(Application.persistentDataPath + "/" + com.GetComponentInChildren<Text>().text + ".json", JsonUtility.ToJson(com));

        for (int i = 0; i < com.transform.childCount - DefaulChilds; ++i)
        {
            File.WriteAllText(Application.persistentDataPath + "/" + com.GetComponentInChildren<Text>().text + "/" + i + ".json", JsonUtility.ToJson(com.transform.GetChild(i + DefaulChilds).GetComponent<UICommand>()));
        }


    }
    public void SerializeToJson(UIComplexCommand com)
    {
        System.IO.Directory.CreateDirectory( Application.persistentDataPath + "/" + com.GetComponentInChildren<Text>().text);
        Debug.Log("ss-s-s-s-s-SAVED");
        com.IndexUp();
        File.WriteAllText(Application.persistentDataPath+"/" +com.GetComponentInChildren<Text>().text+".json", JsonUtility.ToJson(com));
        
        for (int i = 0; i < com.transform.childCount-DefaulChilds; ++i)
        {
            File.WriteAllText(Application.persistentDataPath + "/" + com.GetComponentInChildren<Text>().text + "/" + i+ ".json", JsonUtility.ToJson(com.transform.GetChild(i+DefaulChilds).GetComponent<UICommand>()));
        }
    }
   
    public void DeserializeJson(string JsonPath)
    { 
        UIComplexCommand newCommand = Instantiate(commandBuilder.ComplexCommandPrefab.gameObject, GameObject.Find("AvailableCommandsField").transform).GetComponent<UIComplexCommand>();
        string str = File.ReadAllText(JsonPath);
        //Debug.Log(str);
        JsonUtility.FromJsonOverwrite(str, newCommand);
        string name = JsonPath.Substring(Application.persistentDataPath.Length+1);
        name = name.Remove(name.Length - 5, 5);
        newCommand.GetComponentInChildren<Text>().text = name;
        string[] SubFiles = Directory.GetFiles(Application.persistentDataPath+"/"+name, "*.json");
        Debug.Log("    " + "Saveindex: " + newCommand.localSaveIndex);
       
        for (int i = 0; i < SubFiles.Length; ++i)
        {
            //Read from json string which prefab will be used
            string CommandinJson = File.ReadAllText(SubFiles[i]);
          
            if(CommandinJson.Substring(CommandinJson.IndexOf("isComplex")+11, 4) == "true")
                newCommand.UICommandElements[i] = Instantiate(commandBuilder.ComplexCommandPrefab.gameObject, newCommand.transform).GetComponent<UIComplexCommand>();
            else
                newCommand.UICommandElements[i] = Instantiate(commandBuilder.CommandPrefab.gameObject, newCommand.transform).GetComponent<UICommand>();


            JsonUtility.FromJsonOverwrite(File.ReadAllText(SubFiles[i]), newCommand.UICommandElements[i]);
            newCommand.UICommandElements[i].gameObject.name = newCommand.UICommandElements[i].CommandName;
            newCommand.UICommandElements[i].GetComponentInChildren<Text>().text= newCommand.UICommandElements[i].CommandName;
            newCommand.UICommandElements[i].gameObject.SetActive(false);
        }

      
       
    }
}

[System.Serializable]
public  class ComplexScenario
{
    public string flag;
    public string name;
    public List<Command> Scenario;
}
