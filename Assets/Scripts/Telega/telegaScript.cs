using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class telegaScript : MonoBehaviour {

    public GameObject teleg;
    public GameObject Camera;

    public float step;
    public float rotstep;
    public GameObject bar1, bar2, bar3, wheel1, wheel2, wheel3;
    public float WheelAngleSpeed;
    public float WheelRadius;
    public float telegaRadius;
    public NetConnection net = SceneManager.Net;

    public bool ToSend = true;

    Text modeText;

    public GameObject PointA, PointB, PointC, APointBar1, BPointBar1, APointBar2, BPointBar2, APointBar3, BPointBar3;

    public enum MoveType
    {
        RotateTeleguAndMove,
        RotateBarsAndMove
    }
    public MoveType Type;

    private float rotationwheel = 0;
    private float rot;
    public Vector3 A, B, C, a, b;
    public bool isMoved;
    private int PresentAim = 0;
    private bool isWaited = false;


	// Use this for initialization
	void Start () {
       isMoved = false;
        rotstep = WheelAngleSpeed * WheelRadius / telegaRadius;
        step = WheelAngleSpeed * WheelRadius * Mathf.Deg2Rad;

        C = PointA.transform.position;
        //Net = SceneManager.Net;
        //Net.gameObject.SetActive(true);
        Camera = GameObject.Find("Camera");
        modeText = GameObject.Find("Telega Mode Text").GetComponent<Text>();
        modeText.text = "Directional";
    }

    void Directional()
    {
        rot = transform.eulerAngles.y * Mathf.Deg2Rad;
        if (!isWaited)
        {
            if (isMoved)
            {

                if (Vector3.Angle(a, b) > rotstep)

                    TurnToDirection();
                else
                {
                    TurningBars(0, 0, 0);
                    if (!isWaited)
                    {
                        wheel1.transform.Rotate(0, -WheelAngleSpeed, 0);
                        wheel2.transform.Rotate(0, -WheelAngleSpeed, 0);
                        wheel3.transform.Rotate(0, -WheelAngleSpeed, 0);

                        transform.position += a.normalized * WheelAngleSpeed * WheelRadius * Mathf.Deg2Rad;
                    }
                }
            }
        }
    }

    void TurnToDirection()
    {
     
        TurningBars(90.0f, 320f, 40.0f);
        if (isWaited) return;
        
        if (Vector3.Cross(a, b).y > 0)
        {
            transform.Rotate(0, 0, rotstep);

            wheel1.transform.Rotate(0, -WheelAngleSpeed, 0);
            wheel2.transform.Rotate(0, -WheelAngleSpeed, 0);
            wheel3.transform.Rotate(0, WheelAngleSpeed, 0);
        }

        if (Vector3.Cross(a, b).y < 0)
        {
            transform.Rotate(0, 0, -rotstep);

            wheel1.transform.Rotate(0, WheelAngleSpeed, 0);
            wheel2.transform.Rotate(0, WheelAngleSpeed, 0);
            wheel3.transform.Rotate(0, -WheelAngleSpeed, 0);
        }
        
        A = PointA.transform.position;
        B = PointB.transform.position;

        a = B - A;
        b = C - A;

        if (Vector3.Angle(a, b) < rotstep)
        {
            transform.localEulerAngles = new Vector3(-90, 0, Vector3.Angle(Vector3.left, b) * Vector3.Cross(Vector3.left, b).normalized.y);
        }
    }

    void TurningBars(float RotBar1, float RotBar2, float RotBar3)
    {
        if
        (Mathf.Abs(bar3.transform.localEulerAngles.z - RotBar3) < 1e-4 && Mathf.Abs(bar2.transform.localEulerAngles.z - RotBar2) < 1e-4 && Mathf.Abs(bar1.transform.localEulerAngles.z - RotBar1) < 1e-4)
            return;
        else
        {
            bar1.transform.localEulerAngles = new Vector3(bar1.transform.localEulerAngles.x, bar1.transform.localEulerAngles.y, RotBar1);
            bar2.transform.localEulerAngles = new Vector3(bar2.transform.localEulerAngles.x, bar2.transform.localEulerAngles.y, RotBar2);
            bar3.transform.localEulerAngles = new Vector3(bar3.transform.localEulerAngles.x, bar3.transform.localEulerAngles.y, RotBar3);

            StartCoroutine(Wait(2));
        }
    }

    void Parallel()
    {
         Vector3 a1, a2, a3;

         a1 = BPointBar1.transform.position - APointBar1.transform.position;
         a2 = BPointBar2.transform.position - APointBar2.transform.position;
         a3 = BPointBar3.transform.position - APointBar3.transform.position;

         if (!isWaited)
            if (isMoved)
                if (Math.Abs(Vector3.Cross(b, a1).y) > 0.01 || Vector3.Dot(b, a1) < 0)
                {
                    TurningBars(
                        Vector3.Angle(Vector3.left, b) * Vector3.Cross(Vector3.left, b).normalized.y - Vector3.Angle(Vector3.left, a) * Vector3.Cross(Vector3.left, a).normalized.y,
                        Vector3.Angle(Vector3.left, b) * Vector3.Cross(Vector3.left, b).normalized.y - Vector3.Angle(Vector3.left, a) * Vector3.Cross(Vector3.left, a).normalized.y,
                        Vector3.Angle(Vector3.left, b) * Vector3.Cross(Vector3.left, b).normalized.y - Vector3.Angle(Vector3.left, a) * Vector3.Cross(Vector3.left, a).normalized.y);
                }
                else
                {
                    wheel1.transform.Rotate(0, -WheelAngleSpeed, 0);
                    wheel2.transform.Rotate(0, -WheelAngleSpeed, 0);
                    wheel3.transform.Rotate(0, -WheelAngleSpeed, 0);

                    transform.position += b.normalized * WheelAngleSpeed * WheelRadius * Mathf.Deg2Rad;
                }
    }

    IEnumerator Wait(int a)
    {
        isWaited = true;
        yield return new WaitForSeconds(a);
        isWaited = false;
    }

    IEnumerator WaitToSend(float a)
        {
        ToSend = false;
        yield return new WaitForSeconds(a);
        ToSend = true;
        }
    
    void FixedUpdate ()
    {
        A = PointA.transform.position;
        B = PointB.transform.position;
        if (Camera.GetComponent<raycast>().aims.Count > 0)
        {
            C = Camera.GetComponent<raycast>().aims[0];
            C.y = teleg.transform.position.y;
        }
        else
        {
            C = A;
            C.y = teleg.transform.position.y;
        }

        a = B - A;
        b = C - A;

        if (Math.Abs(A.x - C.x) < step && (Math.Abs(A.z - C.z) < step))
        {
            if (Camera.GetComponent<raycast>().aims.Count > 0)
            {
                isMoved = true;
                Camera.GetComponent<raycast>().aims.RemoveAt(0);
                Destroy(Camera.GetComponent<raycast>().balls[0]);
                Camera.GetComponent<raycast>().balls.RemoveAt(0);
                if (Camera.GetComponent<raycast>().aims.Count > 0)
                {
                    C = Camera.GetComponent<raycast>().aims[0];
                    C.y = teleg.transform.position.y;
                }
                    
            }
            else isMoved = false;
        }   
        
        switch (Type)
        {
            case MoveType.RotateTeleguAndMove:
                Directional();
                break;
            case MoveType.RotateBarsAndMove:
                Parallel();
                break;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            switch (Type)
            {
                case MoveType.RotateTeleguAndMove:
                    Type = MoveType.RotateBarsAndMove;
                    modeText.text = "Parallel";
                    break;
                case MoveType.RotateBarsAndMove:
                    Type = MoveType.RotateTeleguAndMove;
                    modeText.text = "Directional";
                    break;
            }
        }
    } 
}
