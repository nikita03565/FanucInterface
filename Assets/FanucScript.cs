using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets;
using UnityEngine.EventSystems;

public class FanucScript : MonoBehaviour
{
    FanucModel model = new FanucModel();
    
    float speed = 200.0f;
   
    float[] jointAngles = new float[6] { 0f, 0f, 0f, 0f, -90f, 0f };
    float[] jointAnglesInc = new float[6];

    float[] newCoord = new float[6];
    float[] diff = new float[6];

    string[] Axis = new string[]{"First","Second","Third","Fourth", "Fifth","Sixth"};
    //float[] worldPos = new float[6] { 985f, 0f, 940f, -180f, 0f, 0f };
    int mode;
    public Transform first;
    public Transform second;
    public Transform third;
    public Transform thirdCollider;
    public Transform fourth;
    public Transform fourthCollider;
    public Transform fifth;
    public Transform fifthCollider;
    public Transform sixth;
    public Transform sixthCollider;

    public Text modeName;
    public Text jointCoord;
    public Text worldCoord;
    bool FromTheKeyboard;
    bool NoCollisions = true;

    private float normCoef;

    public InputField inputField;

    void Start()
    {
        modeName.text = "Mode: Joints";
        mode = 0;
        fifth.transform.localRotation = Quaternion.Euler(0, jointAngles[4], 0);
        inputField.onEndEdit.AddListener(delegate { LockInput(inputField); });
        inputField.text = "Enter coord to move";
    }

    
    public void IncCoord(int n)
    {
        if (mode == 0)
        {
            for (int i = 0; i < 6; ++i)
            {
                jointAnglesInc[i] = 0;
            }           
            jointAnglesInc[n] = Time.deltaTime * speed;
            FromTheKeyboard = false;
        }  
    }

    public void DecCoord(int n)
    {
        if (mode == 0)
        {
            for (int i = 0; i < 6; ++i)
            {
                jointAnglesInc[i] = 0;
            }       
            jointAnglesInc[n] = -Time.deltaTime * speed;
            FromTheKeyboard = false;
        }
    }

    public void IncSpeed(int n)
    {
        speed += 5;
    }

    public void DecSpeed(int n)
    {
        speed -= 5;
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
            // firstCollider.transform.localRotation = Quaternion.Euler(0, 0, -jointAngles[0]);
            //secondCollider.transform.localRotation = Quaternion.Euler(0, -jointAngles[1], 0);
            thirdCollider.transform.localRotation = third.localRotation;//Quaternion.Euler(0, jointAngles[2] + jointAngles[1], 0);
            fourthCollider.transform.localRotation = fourth.localRotation;// Quaternion.Euler(-jointAngles[3], 0, 0);
            fifthCollider.transform.localRotation = fifth.localRotation;//Quaternion.Euler(0, jointAngles[4], 0);
            sixthCollider.transform.localRotation = sixth.localRotation;//Quaternion.Euler(jointAngles[5], 0, 0);
            NoCollisions = false;
        }
    }

    void LockInput(InputField input)
    {
        try
        {
            if (input.text.Contains(","))
            {
                throw new System.Exception();
            }
            if (input.text.Length > 0)
            {
                var arr = input.text.Split();

                if (arr.Length == 6)
                {
                    for (int i = 0; i < 6; ++i)
                    {
                        newCoord[i] = float.Parse(arr[i]);
                    }
                    for (int i = 0; i < 6; ++i)
                    {
                        diff[i] = newCoord[i] - jointAngles[i];
                    }
                    normCoef = Mathf.Sqrt(diff[0] * diff[0] + diff[1] * diff[1] + diff[2] * diff[2] +
                        diff[3] * diff[3] + diff[4] * diff[4] + diff[5] * diff[5]);

                    StartCoroutine("Move");
                }
                else
                {
                    throw new System.Exception();
                }
            }
            else
            {
                throw new System.Exception();
            };

        }
        catch (System.Exception)
        {
            Debug.Log("Wrong string");
            input.text = "Wrong string. Try again";
        }
    }

    IEnumerator Move()
    {
        Debug.Log(jointAngles[0] - newCoord[0]);
        float[] diff2 = new float[6];

        float error = normCoef;
        while (error > 1 + speed * 0.01)
        {
            for (int i = 0; i < 6; ++i)
            {
                diff2[i] = newCoord[i] - jointAngles[i];
            }
            error = Mathf.Sqrt(diff2[0] * diff2[0] + diff2[1] * diff2[1] + diff2[2] * diff2[2] +
                diff2[3] * diff2[3] + diff2[4] * diff2[4] + diff2[5] * diff2[5]);

            Debug.Log(error);
            for (int i = 0; i < 6; ++i)
            {
                jointAngles[i] += diff[i] * speed * Time.deltaTime / normCoef;
            }
            first.transform.localRotation = Quaternion.Euler(0, 0, -jointAngles[0]);
            second.transform.localRotation = Quaternion.Euler(0, -jointAngles[1], 0);
            third.transform.localRotation = Quaternion.Euler(0, jointAngles[2] + jointAngles[1], 0);
            fourth.transform.localRotation = Quaternion.Euler(-jointAngles[3], 0, 0);
            fifth.transform.localRotation = Quaternion.Euler(0, jointAngles[4], 0);
            sixth.transform.localRotation = Quaternion.Euler(jointAngles[5], 0, 0);
            yield return new WaitForFixedUpdate();
        }
        for (int i = 0; i < 6; ++i)
        {
            jointAngles[i] = newCoord[i];
        }
    }

    void FixedUpdate()
    {

        if (Input.anyKey)
        {
            
           
            for (int i = 0; i < 6; ++i)
            {
                if (FromTheKeyboard)
                {
                    jointAnglesInc[i] = Input.GetAxis(Axis[i]) * speed * Time.deltaTime;
                }
                jointAngles[i] += jointAnglesInc[i];
            }

            thirdCollider.transform.localRotation = Quaternion.Euler(0, jointAngles[2] + jointAngles[1], 0);//Quaternion.Euler(0, jointAngles[2] + jointAngles[1], 0);
            fourthCollider.transform.localRotation = Quaternion.Euler(-jointAngles[3], 0, 0);// Quaternion.Euler(-jointAngles[3], 0, 0);
            fifthCollider.transform.localRotation = Quaternion.Euler(0, jointAngles[4], 0);//Quaternion.Euler(0, jointAngles[4], 0);
            sixthCollider.transform.localRotation = Quaternion.Euler(jointAngles[5], 0, 0);//Quaternion.Euler(jointAngles[5], 0, 0);  
            if (NoCollisions)
            {

                first.transform.localRotation = Quaternion.Euler(0, 0, -jointAngles[0]);
                second.transform.localRotation = Quaternion.Euler(0, -jointAngles[1], 0);
                third.transform.localRotation = Quaternion.Euler(0, jointAngles[2] + jointAngles[1], 0);
                fourth.transform.localRotation = Quaternion.Euler(-jointAngles[3], 0, 0);
                fifth.transform.localRotation = Quaternion.Euler(0, jointAngles[4], 0);
                sixth.transform.localRotation = Quaternion.Euler(jointAngles[5], 0, 0);
            }
           
            FromTheKeyboard = true;
            NoCollisions = true;
            //jointAngles[0] += Input.GetAxis("First") * speed * Time.deltaTime;
            //jointAngles[1] += Input.GetAxis("Second") * speed * Time.deltaTime;
            //jointAngles[2] += Input.GetAxis("Third") * speed * Time.deltaTime;
            //jointAngles[3] += Input.GetAxis("Fourth") * speed * Time.deltaTime;
            //jointAngles[4] += Input.GetAxis("Fifth") * speed * Time.deltaTime;
            //jointAngles[5] += Input.GetAxis("Sixth") * speed * Time.deltaTime;

        }
        
        string outputAngles = "";
        string outputForwardTask = "";
        var res = FanucModel.GetCoordsFromMat(model.fanucForwardTask(ref jointAngles));
        foreach (float j in jointAngles)
        {
            outputAngles += (j.ToString("0.00") + ", ");
        }

        jointCoord.text = outputAngles;
        foreach (float r in res)
        {
            outputForwardTask += (r.ToString("0.00") + ", ");
        }

        worldCoord.text = outputForwardTask;
      //  Debug.Log(outputAngles + "| " + outputForwardTask);
    }

}
