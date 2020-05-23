using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public  class Command
{
    public bool parallel;
    public string name;
    public int time;
    public int energy;
    public string command="";

    public Command()
    {

    }

    public Command(Command com)
    {
        name = com.name;
        energy = com.energy;
        time = com.time;
        parallel = com.parallel;
        command = com.command;
    }

    public virtual void DoCommand()
    {

    }

    private string ParseTelega(string command) 
    {
        var x = command.Split();
        return x[1] + " "+ x[2];
    }

    private string ParseFanuc(string command) 
    {
        var x = command.Split();
        return x[1] + " " + x[2] + " " + x[3] + " " + x[4] + " " + x[5] + " " + x[6];
    }

    public virtual void GetWindow()
    {
        if (name == "f" && command != "f") {
            //SceneManager.FanucSettingsPanel.command = this;
            SceneManager.FanucSettingsPanel.coordField.text = this.ParseFanuc(this.command);
            SceneManager.FanucSettingsPanel.show();
        } else if (name == "t") {
            // SceneManager.TelegaSettingsPanel.command = this;
            SceneManager.TelegaSettingsPanel.coordField.text = this.ParseTelega(this.command);
            SceneManager.TelegaSettingsPanel.show();
        } else {
            // SceneManager.CameraSettingsPanel.command = this;
            SceneManager.CameraSettingsPanel.show();
        }
        //Debug.Log("WHAAAAT");
    }


    public virtual void Save()
    {

    }


    public virtual void Load()
    {

    }

    public static bool IsEQ(Command com1, Command com2)
    {
        if ((com1.parallel == com2.parallel) && (com1.name == com2.name)&&
        (com1.time == com2.time) && 
            (com1.command==com2.command) && 
            (com1.energy == com2.energy))
            return true;
        else return false;
    }

    public static void Copy(Command com1, Command com2)
    {
        com1.name = com2.name;
        com1.energy = com2.energy;
        com1.time = com2.time;
        com1.parallel = com2.parallel;
        com1.command = com2.command;
    }
}
