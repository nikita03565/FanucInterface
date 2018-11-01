using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class InstantiateFromCam : MonoBehaviour
{
    int NumberofObjects;
    string[] Names;
    string[] StringCoordArr;
    float[] floatCoordArr;
    Vector3[] PositionCoords;
    bool Synchro = false;
    // Use this for initialization

    public void Syncronization(string message)//string ObjectList)
    {
        //-------------------------------------Here should be parser--------------------------------------------------------
        //string message = "{\"fanuc\":\"12 32 1 34 65 -90\",\"telega\":\"100 -2000 300 -1 -2 -3\"}";
        Debug.Log("synchro...");
        try
        {
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
            Debug.Log(dict);
            NumberofObjects = dict.Count; //what's the magic number???
            Debug.Log(NumberofObjects);
            Names = new string[NumberofObjects]; //IDs
            PositionCoords = new Vector3[NumberofObjects];
            //Quaternion[] RotationCoords = new Quaternion[NumberofObjects];
            int index = 0;
            foreach (var i in dict.Keys)
            {

                Debug.Log(i + ": " + dict[i]);
                Names[index] = i;
                StringCoordArr = dict[i].Split();
                floatCoordArr = new float[StringCoordArr.Length];
                Debug.Log("length of parsed array " + StringCoordArr.Length);
                for (int j = 0; j < StringCoordArr.Length; ++j)
                    floatCoordArr[j] = float.Parse(StringCoordArr[j]);
                PositionCoords[index] = CoordTransformation.RobotToUnityPosOnly(new Vector4(float.Parse(StringCoordArr[0]), float.Parse(StringCoordArr[1]), float.Parse(StringCoordArr[2]), 0));
                Debug.Log(Names[index]);
                //Debug.Log("Position: "+PositionCoords[index]); 
                Debug.Log(floatCoordArr);
                ++index;
            }
            //-------------------------------------Here he ends------------------------------------------------------------------      
            Synchro = true;
        }
        catch(UnityException ex)
        {
            Debug.Log(ex);
        }
    }
    // Update is called once per frame
    void Update () {
        if (Synchro)
        {
            for (int i = 0; i < NumberofObjects; ++i)
            {

                GameObject obj = SceneManager.Pull.Find(Names[i]);
                // obj.transform.rotation = RotationCoords[i];
                if (obj)
                {
                    if (obj.name == "fanuc")
                    {
                        Debug.Log("FANUUUUC");
                        StopCoroutine("Move");
                        StartCoroutine(SceneManager.fanuc.Move(floatCoordArr));
                        
                    }
                    else
                    {
                        obj.transform.position = PositionCoords[i];
                        obj.SetActive(true);
                    }
                    Debug.Log(Names[i] + " found");
                }
                else Debug.Log(i + "   " + Names[i] + "Not Found");
            }
            Synchro = false;
        }
    }
   //IEnumerator Instantiation()
   // {
   //     for (int i = 0; i < NumberofObjects; ++i)
   //     {

   //         GameObject obj = SceneManager.Pull.Find(Names[i]);
   //         // obj.transform.rotation = RotationCoords[i];
   //         if (obj)
   //         {
   //             obj.transform.position = PositionCoords[i];
   //             obj.SetActive(true);
   //             Debug.Log(Names[i] + " found");
   //         }
   //         else Debug.Log(i + "   " + Names[i] + "Not Found");
   //     }
   //     yield return null;
   // }
}
