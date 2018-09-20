using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandTelega : Command
{
    public int mode; //0 - directional, 1 - parallel
    public float[] coord; // x, y, yaw
    CommandTelegaUI window;
    //private Command command;

    public CommandTelega()
    {
    }

    public CommandTelega(Command parent)
    {
        Command.Copy(this, parent);
    }

    public override void DoCommand()
    {
        
    }

    public override void GetWindow()
    {
        if (!window) window = SceneManager.TelegaSettingsPanel;
        window.command = this;
        window.show();
    }

    public override void Load()
    {
      
    }

    public override void Save()
    {
      
    }
}
