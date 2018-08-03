using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public  class Command
{
   
    public int energy;
    public int time;
    public  bool parallel;
    public string CommandToSend;
    public Command()
    {

    }
    public Command(Command com)
    {
        energy = com.energy;
        time = com.time;
        parallel = com.parallel;
        CommandToSend = com.CommandToSend;
    }

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

    public static bool IsEQ(Command com1, Command com2)
    {
        if (com1.parallel == com2.parallel && 
            com1.time == com2.time && 
            com1.CommandToSend == com2.CommandToSend && 
            com1.energy == com2.energy)
            return true;
        else return false;

    }
    public static void Copy(Command com1, Command com2)
    {
        com1.energy = com2.energy;
        com1.time = com2.time;
        com1.parallel = com2.parallel;
        com1.CommandToSend = com2.CommandToSend;
    }

}
