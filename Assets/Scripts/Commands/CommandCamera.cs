using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCamera : Command
{
    public int mode; //0 - all scene, 1 - specific object
    public string id;
    CommandCameraUI window;
    

    public CommandCamera()
    {
    }

    public CommandCamera(Command parent)
    {
        this.CommandToSend = parent.CommandToSend;
        this.parallel = parent.parallel;
    }

    //public CommandCamera(Command command) : base(command)
    //{
    //}

    public override void DoCommand()
    {
       
    }

    public override void GetWindow()
    {
       
        if (!window) window = SceneManager.CameraSettingsPanel;
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
