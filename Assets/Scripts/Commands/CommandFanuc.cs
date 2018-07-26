﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandFanuc : Command
{
    public int mode; // 0 - joint, 2 - world
    public float[] coord;
    public int grasp; // 1 - grasp, 0 - do nothing, -1 - ungrasp
    CommandFanucUI window;

    public CommandFanuc(Command parent)
    {
        this.CommandToSend = parent.CommandToSend;
        this.parallel = parent.parallel;
    }

    public CommandFanuc()
    {
    }

    public override void GetWindow()
    {
        

        if (!window) window = SceneManager.FanucSettingsPanel;
        window.command = this;
        window.show(); 
        
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