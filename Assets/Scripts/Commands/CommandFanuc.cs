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
        name = "f";
    }
    public CommandFanuc(Command parent)
    {
        Command.Copy(this, parent);        
    }

   

    public override void GetWindow()
    {
        SceneManager.FanucSettingsPanel.command = this;
        SceneManager.FanucSettingsPanel.show();    
    }

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
