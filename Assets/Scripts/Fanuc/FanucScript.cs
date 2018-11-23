using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FanucScript : MonoBehaviour
{
    FanucModel model = new FanucModel();
    public NetConnection net;
    public AddPoint addPoint;

    //need to be changed for cartesian coords
    public float  standartSpeed=30;
    public float speed = 30;
    public float desyncSpeed = 300;

    public float[] jointAngles = new float[6] { 0f, 0f, 0f, 0f, -90f, 0f };
    public float[] worldPos = new float[6] { 985f, 0f, 940f, -180f, 0f, 0f };
    float[] jointAnglesInc = new float[6];
    float[] worldPosInc = new float[6];


    //move to another class?
    //public float[] newCoord = new float[6];
    
    string[] Axis = new string[] { "First", "Second", "Third", "Fourth", "Fifth", "Sixth" };
    
    public int mode;

    float[] ControllerJointUpdate = new float[6] { 0f, 0f, 0f, 0f, 0f, 0f };
    float[] ControllerWorldUpdate = new float[6] { 0f, 0f, 0f, 0f, 0f, 0f };
    public Transform[] Fanuc;
    public Transform[] FanucColliders;

    public int semafor = 0;
    //move to FanucScriptUI
    public Text speedText;
    public Text hideshow;
    public GameObject panel;

    public Text modeName;
    public Text jointCoord;
    public Text worldCoord;

    public InputField inputField;

    public InputField InputSpeedField;


    bool NoCollisions = true;
    bool ReadytoSend = true;

    void Start()
    {
        //move to FanucScriptUI
        modeName.text = "Mode: Joints";
        speedText.text = speed.ToString();
        net = SceneManager.Net;
        mode = 0;
        Fanuc[4].transform.localRotation = Quaternion.Euler(0, jointAngles[4], 0);
        inputField.onEndEdit.AddListener(delegate { LockInput(inputField); });
        CoordDisplayAndSave();
        addPoint = FindObjectOfType<AddPoint>();
    }

    public void ChangeMode()
    {
        if (mode == 2)
        {
            mode = 0;
            //move to FanucScriptUI
            modeName.text = "Mode: Joints";
        }
        else if (mode == 0)
        {
            mode = 2;
            //move to FanucScriptUI
            modeName.text = "Mode: World";
        }
    }


    public void IncSpeed(int n)
    {
        speed += 5;
        //move to FanucScriptUI
        speedText.text = speed.ToString();
    }

    public void DecSpeed(int n)
    {
        speed -= 5;
        if (speed < 0)
        {
            speed = 0;
        }
        //move to FanucScriptUI
        speedText.text = speed.ToString();
    }

    //move to FanucScriptUI
    public void InputSpeed(InputField s)
    {
        speedText.text = s.text;
        speed = float.Parse(s.text);
    }

    //move to FanucScriptUI
    public void Visibility(GameObject panel)
    {
        panel.SetActive(!panel.activeSelf);
        ChangeTextShow(hideshow);
    }

    //move to FanucScriptUI
    public void ChangeTextShow(Text text)
    {

        if (text.text == "Show")
        {
            text.text = "Hide";
        }
        else text.text = "Show";
    }

    public void UpdateCoord(int n)
    {
        if (mode == 0)
        {
            jointAnglesInc[n] = Input.GetAxis(Axis[n]) * Time.deltaTime * speed + ControllerJointUpdate[n];
            ControllerJointUpdate[n] = 0;
        }
        if (mode == 2)
        {
            worldPosInc[n] = Input.GetAxis(Axis[n]) * Time.deltaTime * speed + ControllerWorldUpdate[n];
            ControllerWorldUpdate[n] = 0;
        }
    }

   public void IncCoord(int n)
   {
       if (mode == 0)
           ControllerJointUpdate[n] = Time.deltaTime * speed;
       else if (mode == 2)
           ControllerWorldUpdate[n] = Time.deltaTime * speed;
   }

   public void DecCoord(int n)
   {
       if (mode == 0)
           ControllerJointUpdate[n] = -Time.deltaTime * speed;
       else if (mode == 2)
           ControllerWorldUpdate[n] = -Time.deltaTime * speed;
   }

    //move to FanucScriptUI
    public void LockInput(InputField input)
    {
        try
        {
            if (input.text.Contains(","))
            {
                input.text = "Don't use commas";
                throw new System.Exception();
            }
            if (input.text.Length > 0)
            {
                var arr = input.text.Split();

                if (arr.Length == 6)
                {
                    float[] newCoord = new float[6];
                    if (mode == 0)
                    {
                        for (int i = 0; i < 6; ++i)
                        {
                            newCoord[i] = float.Parse(arr[i]);
                            if (newCoord[i] > FanucModel.limMax[i] || newCoord[i] < FanucModel.limMin[i])
                            {
                                input.text = "Out of limits";
                                throw new System.Exception();
                            }
                        }
                        StartCoroutine(Move(newCoord));
                    } else
                    {
                        for (int i = 0; i < 6; ++i)
                        {
                            newCoord[i] = float.Parse(arr[i]);
                        }

                        StartCoroutine(Move(newCoord));
                    }
                }
                else
                {
                    input.text = "6 coords are required";
                    throw new System.Exception();
                }
            }
            else
            {
                input.text = "You must write something";
                throw new System.Exception();
            };

        }
        catch (System.Exception)
        {
            Debug.Log("Wrong string");
        }
    }

    IEnumerator CoordtoServer()
    {
        yield return new WaitForSeconds(0.25f);
        if (mode == 0) SceneManager.Net.Sender(RobotCommands.FanucMoving());
        else SceneManager.Net.Sender(RobotCommands.FanucMoving(false));
        ReadytoSend = true;
    }
    public IEnumerator Stopper()
    {
        StopCoroutine("Move");
        yield return null;
    }
    public IEnumerator Move(float[] newCoord)
    {
        
        semafor += 1;
        SceneManager.UserControlLock = true;
        Debug.Log(newCoord[0] + " " + newCoord[1] + " " + newCoord[2] + " " +
            newCoord[3] + " " + newCoord[4] + " " + newCoord[5] + " " + mode + "--------------------------------------------------------");
        //Debug.Log(jointAngles[0] - newCoord[0]);
        float[] diff = new float[6];
        float normCoef;
        if (mode == 0)
        {
            for (int i = 0; i < 6; ++i)
            {
                diff[i] = newCoord[i] - jointAngles[i];
            }
            normCoef = Mathf.Sqrt(diff[0] * diff[0] + diff[1] * diff[1] + diff[2] * diff[2] +
                diff[3] * diff[3] + diff[4] * diff[4] + diff[5] * diff[5]);
        }
        else
        {
            float[] newCoordTmp = FanucModel.chooseNearestPose(model.InverseTask(ref newCoord), ref jointAngles);
            newCoord = newCoordTmp;
            for (int i = 0; i < 6; ++i)
            {
                diff[i] = newCoord[i] - jointAngles[i];
            }
            normCoef = Mathf.Sqrt(diff[0] * diff[0] + diff[1] * diff[1] + diff[2] * diff[2] +
                diff[3] * diff[3] + diff[4] * diff[4] + diff[5] * diff[5]);
        }

        float[] diff2 = new float[6];

        float error = normCoef;

        while (error > 1 + speed * 0.02 && NoCollisions)
        {
            for (int i = 0; i < 6; ++i)
            {
                diff2[i] = newCoord[i] - jointAngles[i];
            }
            error = Mathf.Sqrt(diff2[0] * diff2[0] + diff2[1] * diff2[1] + diff2[2] * diff2[2] +
                diff2[3] * diff2[3] + diff2[4] * diff2[4] + diff2[5] * diff2[5]);

            //Debug.Log(error);
            for (int i = 0; i < 6; ++i)
            {
                jointAngles[i] += diff[i] * speed * Time.deltaTime / normCoef;
            }
            RotateFanuc(FanucColliders, jointAngles);
            //Debug.Log(NoCollisions);

            yield return new WaitForFixedUpdate();

            RotateFanuc(Fanuc, jointAngles);
            CoordDisplayAndSave();
            if (ReadytoSend)
            {
                StartCoroutine(CoordtoServer());
                ReadytoSend = false;
            }

        }

        if (NoCollisions)
        {
            for (int i = 0; i < 6; ++i)
            {
                jointAngles[i] = newCoord[i];
            }
            CoordDisplayAndSave();
        }
        if(!SceneManager.ObserverMode) SceneManager.UserControlLock = false;
        semafor -= 1;
    }

    public void CollisionLimiter()
    {
        if (NoCollisions)
        {
            Debug.Log("Collision!");
            for (int i = 0; i < 6; ++i)
            {
                jointAngles[i] -= jointAnglesInc[i];
            }

            RotateFanuc(FanucColliders, jointAngles);

            NoCollisions = false;
        }
    }

    void RotateFanuc(Transform[] Joints, float[] jointAngles)
    {

        if (Joints[0]) Joints[0].localRotation = Quaternion.Euler(0, 0, -jointAngles[0]);
        if (Joints[1]) Joints[1].localRotation = Quaternion.Euler(0, -jointAngles[1], 0);
        if (Joints[2]) Joints[2].localRotation = Quaternion.Euler(0, jointAngles[2] + jointAngles[1], 0);
        if (Joints[3]) Joints[3].localRotation = Quaternion.Euler(-jointAngles[3], 0, 0);
        if (Joints[4]) Joints[4].localRotation = Quaternion.Euler(0, jointAngles[4], 0);
        if (Joints[5]) Joints[5].localRotation = Quaternion.Euler(-jointAngles[5], 0, 0);
    }

    //move to FanucScriptUI
    void CoordDisplayAndSave()
    {
        string outputAngles = "";
        string outputForwardTask = "";
        worldPos = FanucModel.GetCoordsFromMat(model.fanucForwardTask(ref jointAngles));
        foreach (float j in jointAngles)
        {
            outputAngles += (j.ToString("0.00") + ", ");
        }

        jointCoord.text = outputAngles;
        foreach (float r in worldPos)
        {
            outputForwardTask += (r.ToString("0.00") + ", ");
        }

        worldCoord.text = outputForwardTask;
        //  Debug.Log(outputAngles + "| " + outputForwardTask);
    }

    void FixedUpdate()
    {
        Debug.Log(semafor);
        if (Input.anyKey && !Input.GetMouseButton(0)&&!Input.GetMouseButton(1)&&!SceneManager.ObserverMode&&!SceneManager.UserControlLock&&!inputField.isFocused && !InputSpeedField.isFocused && !addPoint.input.isFocused)
        {
            Debug.Log("im in update");   
            for (int i = 0; i < 6; ++i)
            {
                UpdateCoord(i);
                if (mode == 0)
                    if (jointAngles[i] + jointAnglesInc[i] > FanucModel.limMin[i] && jointAngles[i] + jointAnglesInc[i] < FanucModel.limMax[i])
                        jointAngles[i] += jointAnglesInc[i];
                if (mode == 2)
                    worldPos[i] += worldPosInc[i];
            }

            RotateFanuc(FanucColliders, jointAngles);
            if (NoCollisions)
            {
                RotateFanuc(Fanuc, jointAngles);

            }
            else StopCoroutine("Move");

            if (ReadytoSend)
            {

                StartCoroutine(CoordtoServer());
                ReadytoSend = false;
            }

            NoCollisions = true;

            if (mode == 2)
            {
                try
                {
                    var tmp = model.InverseTask(ref worldPos);
                    
                    if (tmp.Length == 0)
                    {
                        Debug.Log("ZEROOOOOOOO");
                        float[] worldPosTmp = new float[] {
                        worldPos[0] - (worldPosInc[0] / 2.0f),
                        worldPos[1] - (worldPosInc[1] / 2.0f),
                        worldPos[2] - (worldPosInc[2] / 2.0f),
                        worldPos[3] - (worldPosInc[3] / 2.0f),
                        worldPos[4] - (worldPosInc[4] / 2.0f),
                        worldPos[5] - (worldPosInc[5] / 2.0f)
                        };
                        tmp = model.InverseTask(ref worldPosTmp);
                    }
                    jointAngles = FanucModel.chooseNearestPose(tmp, ref jointAngles);
                }
                catch (System.Exception)
                {
                    Debug.Log("GOT IT");
                }
                //RotateFanuc(Fanuc, jointAngles);
            }

            RotateFanuc(FanucColliders, jointAngles);
            if (NoCollisions)
            {
                RotateFanuc(Fanuc, jointAngles);
            }
            else StopCoroutine("Move");

            if (ReadytoSend)
            {
                StartCoroutine(CoordtoServer());
                ReadytoSend = false;
            }
        
            NoCollisions = true;

            //realize what the hell is going on here
            
            //move to FanucScriptUI
            CoordDisplayAndSave();
        }
    }
}
