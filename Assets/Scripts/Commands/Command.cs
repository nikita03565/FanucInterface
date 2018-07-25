using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public  class Command
{
    public Command()
    {

    }
    Command(Command com)
    {
        parallel = com.parallel;
        CommandToSend = com.CommandToSend;
    }
    public int time;
    public  bool parallel;
    public string CommandToSend;
  
    
    public virtual void DoCommand()
    {

    }

    public virtual void GetWindow()
    {
        Debug.Log("WHAAAAT");
    }



    public virtual void Save()
    {

    }


    public virtual void Load()
    {

    }
    

}
