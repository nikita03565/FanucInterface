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
    float[] FanucCoords;
    float[] TelegaCoord=new float[6];
    Vector3[] PositionCoords;
    bool Synchro = false;

    public static bool stopCoroutine=false;
    // Use this for initialization

    public void Syncronization(string message)//string ObjectList)
    {
        //-------------------------------------Here should be parser--------------------------------------------------------
        //string message = "{\"fanuc\":\"12 32 1 34 65 -90\",\"telega\":\"100 -2000 300 -1 -2 -3\"}";
        //Debug.Log("synchro...");
        try
        {
            var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
           // Debug.Log(dict);
            NumberofObjects = dict.Count; //what's the magic number???
            //Debug.Log(NumberofObjects);
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
                
                //switch (i)
                //{
                //    case "fanuc":
                //        {
                //            FanucCoords = floatCoordArr;
                //            break;
                //        }
                //    case "telega":
                //        {
                //            Debug.Log("here telega " + StringCoordArr);
                //            TelegaCoord = floatCoordArr;
                //            Debug.Log(floatCoordArr[0]+" "+ floatCoordArr[1]);
                //            break;
                //        }

                   
                //}
                if(i== "fanuc")
                {
                     FanucCoords = floatCoordArr;
                    
               }
                if (i == "telega")
                {
                    TelegaCoord[0] = float.Parse(StringCoordArr[0]);
                    TelegaCoord[1] = float.Parse(StringCoordArr[1]);
                }else
                if (SceneManager.Pull.Find(Names[index]))
                {
                    for (int j = 0; j < StringCoordArr.Length; ++j)
                        floatCoordArr[j] = float.Parse(StringCoordArr[j]);
                    PositionCoords[index] = CoordTransformation.RobotToUnityPosOnly(new Vector4(float.Parse(StringCoordArr[0]), float.Parse(StringCoordArr[1]), float.Parse(StringCoordArr[2]) - 190f, 1));
                    Debug.Log(Names[index] + " object found, coords added");
                }
               // Debug.Log("length of parsed array " + StringCoordArr.Length);
                
                //Debug.Log("Position: "+PositionCoords[index]); 
                //Debug.Log(floatCoordArr);
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
    //IEnumerator MoveManager()
    //{
    //    yield return new WaitUntil(() => stopCoroutine == true);
    //    StartCoroutine(SceneManager.fanuc.Move(FanucCoords));
    //}
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

                        if (SceneManager.fanuc.semafor == 0)
                        {
                            SceneManager.fanuc.speed = SceneManager.fanuc.standartSpeed;
                            StartCoroutine(SceneManager.fanuc.Move(FanucCoords));
                        }
                        else
                        {
                            SceneManager.fanuc.speed = SceneManager.fanuc.desyncSpeed;
                        }
                        
                    }
                    else 
                    if(obj.name =="telega")
                    {
                        Debug.Log(TelegaCoord[0] + " " + TelegaCoord[1] + "------------------------");
                        AddPoint.AddFromObserver(TelegaCoord[0], TelegaCoord[1]);
                        SceneManager.telega.Synchronize();
                      
                    }
                    else
                    {
                        obj.transform.position = PositionCoords[i];
                        obj.SetActive(true);
                    }
                    //Debug.Log(Names[i] + " found");
                }
                //else Debug.Log(i + "   " + Names[i] + "Not Found");
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
