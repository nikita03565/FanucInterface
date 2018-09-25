using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public  class Command
{
    public bool parallel;
    public string name;
    public int time;
    public int energy;
    public string сommand="";

    public Command()
    {

    }

    public Command(Command com)
    {
        name = com.name;
        energy = com.energy;
        time = com.time;
        parallel = com.parallel;
        сommand = com.сommand;
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
        if ((com1.parallel == com2.parallel) && (com1.name == com2.name)&&
        (com1.time == com2.time) && 
            (com1.сommand==com2.сommand) && 
            (com1.energy == com2.energy))
            return true;
        else return false;
    }

    public static void Copy(Command com1, Command com2)
    {
        com1.name = com2.name;
        com1.energy = com2.energy;
        com1.time = com2.time;
        com1.parallel = com2.parallel;
        com1.сommand = com2.сommand;
    }
}
