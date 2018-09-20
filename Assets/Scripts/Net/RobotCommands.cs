using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class RobotCommands:MonoBehaviour{

    // Use this for initialization

    static public string FanucMoving(bool Joints = true)
    {
        //FanucScript Fanuc = SceneManager.fanuc; //FindObjectOfType<FanucScript>();
        string MessageToServer = "{\"flag\": \"0\",\"Scenario\": [{\"parallel\":\"False\", \"name\": \"fanuc\",\"time\":\"0\",\"energy\":\"0\", \"command\": \"m";
        //string MessageToServer = "m";
        for (int i = 0; i < 6; ++i)
        {
           if(Joints) MessageToServer += " " + SceneManager.fanuc.jointAngles[i].ToString();
           else MessageToServer += " " + SceneManager.fanuc.worldPos[i].ToString();
        }
        MessageToServer += " 0\"}]}";
        return MessageToServer;
    }

    static public string FanucMoving(string coord)
    {
        return "{'Flag':0;'name':'fanuc';'command':'m " + coord + " 0'}";
    }
    //WIP
    static public string TelegaMoving()
    {
        TelegaManager Telega = FindObjectOfType<TelegaManager>();
        //telegaScript Telega = FindObjectOfType<telegaScript>();
        //string MessageToServer = "{'flag':0;'name':'t';'command':'m";
        string MessageToServer = "{\"flag\": \"0\",\"Scenario\": [{\"parallel\":\"False\", \"name\": \"telega\",\"time\":\"0\",\"energy\":\"0\", \"command\": \"m";
        MessageToServer += " " + (-Telega.angle[1]).ToString("0.0") + " " + (Telega.isReversed[1] * Telega.velocity[1]).ToString("0.0") + " " + Telega.dist[1].ToString("0.0") + 
                           " " + (-Telega.angle[0]).ToString("0.0") + " " + (Telega.isReversed[0] * Telega.velocity[0]).ToString("0.0") + " " + Telega.dist[0].ToString("0.0") +
                           " " + (-Telega.angle[2]).ToString("0.0") + " " + (Telega.isReversed[2] * Telega.velocity[2]).ToString("0.0") + " " + Telega.dist[2].ToString("0.0") + "\"}]}";
        Debug.Log(MessageToServer); 

        return MessageToServer;
    }

    static public string GetSceneInf()
    {
        string MessageToServer = "'Flag':1;'name';'command'";

        return MessageToServer;
    }
    
}
