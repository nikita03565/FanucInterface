using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandFanuc : Command
{
    public int mode; // 0 - joint, 2 - world
    public float[] coord;
    public int grasp; // 1 - grasp, 0 - do nothing, -1 - ungrasp

    public CommandFanuc()
    {
        //name = "f";
    }
    public CommandFanuc(Command parent)
    {
        Command.Copy(this, parent);        
    }

   private string Parse(string command) 
    {
        var x = command.Split();
        return x[1] + " " + x[2] + " " + x[3] + " " + x[4] + " " + x[5] + " " + x[6];
    }
    public override void GetWindow()
    {
        SceneManager.FanucSettingsPanel.command = this;
        if (this.command != "") 
        {
            var res = this.Parse(this.command);
            Debug.Log(res);
            SceneManager.FanucSettingsPanel.coordField.text = res;
        }
        else 
        {
            SceneManager.FanucSettingsPanel.coordField.text = this.command;
        }
        SceneManager.FanucSettingsPanel.show();
    }

    // public override void GetWindow()
    // {
    //     SceneManager.FanucSettingsPanel.command = this;
    //     SceneManager.FanucSettingsPanel.coordField.text = this.command;
    //     SceneManager.FanucSettingsPanel.show();    
    // }

    public override void DoCommand()
    {
      
    }

    public override void Load()
    {
        
    }

    public override void Save()
    {
        
    }
}
