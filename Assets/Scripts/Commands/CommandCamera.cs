using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCamera : Command
{
    public int mode; //0 - all scene, 1 - specific object
    public string id;
<<<<<<< HEAD
   
=======
    
    

>>>>>>> temporary-artem
    public CommandCamera()
    {
    }

    public CommandCamera(Command parent)
    {
        Command.Copy(this, parent);
    }

    //public CommandCamera(Command command) : base(command)
    //{
    //}

    public override void DoCommand()
    {
       
    }

    public override void GetWindow()
    {
<<<<<<< HEAD
=======

>>>>>>> temporary-artem
        SceneManager.CameraSettingsPanel.command = this;
        SceneManager.CameraSettingsPanel.show();
    }

    public override void Load()
    {
        
    }

    public override void Save()
    {
       
    }
}
