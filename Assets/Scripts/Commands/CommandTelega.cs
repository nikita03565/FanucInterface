﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandTelega : Command
{
    public int mode; //0 - directional, 1 - parallel
    public float[] coord; // x, y, yaw
<<<<<<< HEAD
=======

>>>>>>> temporary-artem
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
<<<<<<< HEAD
=======
       
>>>>>>> temporary-artem
        SceneManager.TelegaSettingsPanel.command = this;
        SceneManager.TelegaSettingsPanel.show();
    }

    public override void Load()
    {
      
    }

    public override void Save()
    {
      
    }
}
