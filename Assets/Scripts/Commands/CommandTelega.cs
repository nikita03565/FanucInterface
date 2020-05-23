using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandTelega : Command
{
    public int mode; //0 - directional, 1 - parallel
    public float[] coord; // x, y, yaw

    //private Command command;

    public CommandTelega()
    {
         //name = "t";
    }

    public CommandTelega(Command parent)
    {
        Command.Copy(this, parent);
    }

    public override void DoCommand()
    {
        
    }

    private string Parse(string command) 
    {
        var x = command.Split();
        return x[1] + " "+ x[2];
    }

    public override void GetWindow()
    {
        SceneManager.TelegaSettingsPanel.command = this;
        if (this.command != "") 
        {
            var res = this.Parse(this.command);
            Debug.Log(res);
            SceneManager.TelegaSettingsPanel.coordField.text = res;
        }
        else {
            SceneManager.TelegaSettingsPanel.coordField.text = this.command;
        }
        SceneManager.TelegaSettingsPanel.show();
    }

    public override void Load()
    {
      
    }

    public override void Save()
    {
      
    }
}
