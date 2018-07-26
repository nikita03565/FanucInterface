using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class RobotCommands:MonoBehaviour{

    // Use this for initialization

    static public string FanucMoving(bool Joints = true)
    {
        FanucScript Fanuc = FindObjectOfType<FanucScript>();
        string MessageToServer = "{\"flag\": \"0\",\"Scenario\": [{\"parallel\":\"False\", \"name\": \"fanuc\",\"time\":\"0\",\"energy\":\"0\", \"command\": \"m";
        //string MessageToServer = "m";
        for (int i = 0; i < 6; ++i)
        {
           if(Joints) MessageToServer += " " + Fanuc.jointAngles[i].ToString();
           else MessageToServer += " " + Fanuc.worldPos[i].ToString();
        }
        MessageToServer += " 0\"}]}";
        //MessageToServer += " 0 ";
        return MessageToServer;
    }

    static public string FanucMoving(string coord)
    {
        return "{'Flag':0;'name':'fanuc';'command':'m " + coord + " 0'}";
    }
    //WIP
    static public string TelegaMoving()
    {
        telegaScript Telega = FindObjectOfType<telegaScript>();
        string MessageToServer = "{'Flag':0;'name':'t';'command':'m";
        MessageToServer += " " + Telega.Bar2ForSend + " " + Telega.Wheel2ForSend + " " + Telega.Bar1ForSend + " " + Telega.Wheel1ForSend + " " + Telega.Bar3ForSend + " " + Telega.Wheel3ForSend+ " '}";
        Debug.Log(MessageToServer); 

        return MessageToServer;
    }

    static public string GetSceneInf()
    {
        string MessageToServer = "'Flag':1;'name';'command'";

        return MessageToServer;
    }
    
}
