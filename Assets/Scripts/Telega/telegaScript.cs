using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class telegaScript : MonoBehaviour {

    public GameObject teleg;
    public GameObject Camera;

    public float step;
    public float rotstep;
    public GameObject bar1, bar2, bar3, wheel1, wheel2, wheel3;
    public float WheelAngleSpeed;
    public float WheelRadius;
    public float telegaRadius;
    public NetConnection Net;
    public float Bar1ForSend, Bar2ForSend, Bar3ForSend, Wheel1ForSend, Wheel2ForSend, Wheel3ForSend;
    public bool ToSend = true;
    

    public GameObject PointA, PointB, PointC, APointBar1, BPointBar1, APointBar2, BPointBar2, APointBar3, BPointBar3;

    enum MoveType
    {
        RotateTeleguAndMove,
        RotateBarsAndMove
    }
    MoveType Type;

    private float rotationwheel = 0;
    private float rot;
    private Vector3 A, B, C, a, b;
    public bool isMoved;
    private int PresentAim = 0;
    private bool isWaited = false;


	// Use this for initialization
	void Start () {
       isMoved = false;
        rotstep = WheelAngleSpeed * WheelRadius / telegaRadius;
        step = WheelAngleSpeed * WheelRadius * Mathf.PI / 180f;

        C = PointA.transform.position;
        Net = SceneManager.Net;

        Camera = GameObject.Find("Camera");
    }

    void TheFirstTypeMove()
    {
        rot = transform.eulerAngles.y * Mathf.PI / 180f;
        if (!isWaited)
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
                 
                             Wheel1ForSend = WheelAngleSpeed * WheelRadius * 1000;
                             Wheel2ForSend = WheelAngleSpeed * WheelRadius * 1000;
                             Wheel3ForSend = WheelAngleSpeed * WheelRadius * 1000;
                 
                             transform.position += a.normalized * WheelAngleSpeed * WheelRadius * Mathf.PI / 180f;
                         }
                         Debug.Log(B.x * WheelAngleSpeed * WheelRadius + " " + B.y * WheelAngleSpeed * WheelRadius + " " + B.z * WheelAngleSpeed * WheelRadius);
                 }
        }
    }

    void TurnToDirection()
    {
     
        TurningBars(90.0f, 320f, 40.0f);
        if (isWaited) return;
        //if (Vector3.Angle(a, b) > rotstep)
        {
            if (Vector3.Cross(a, b).y > 0)
            {
                transform.Rotate(0, 0, rotstep);

                wheel1.transform.Rotate(0, -WheelAngleSpeed, 0);
                wheel2.transform.Rotate(0, -WheelAngleSpeed, 0);
                wheel3.transform.Rotate(0, WheelAngleSpeed, 0);

                Wheel1ForSend = WheelAngleSpeed * WheelRadius * 1000;
                Wheel2ForSend = WheelAngleSpeed * WheelRadius * 1000;
                Wheel3ForSend = - WheelAngleSpeed * WheelRadius * 1000;
            }

            if (Vector3.Cross(a, b).y < 0)
            {
                transform.Rotate(0, 0, -rotstep);

                wheel1.transform.Rotate(0, WheelAngleSpeed, 0);
                wheel2.transform.Rotate(0, WheelAngleSpeed, 0);
                wheel3.transform.Rotate(0, -WheelAngleSpeed, 0);

                Wheel1ForSend = -WheelAngleSpeed * WheelRadius * 1000;
                Wheel2ForSend = -WheelAngleSpeed * WheelRadius * 1000;
                Wheel3ForSend = WheelAngleSpeed * WheelRadius * 1000;
            }
        }
        A = PointA.transform.position;
        B = PointB.transform.position;

        a = B - A;
        b = C - A;

        if (Vector3.Angle(a, b) < rotstep)
        {

            //Debug.Log(transform.localEulerAngles);
            transform.localEulerAngles = new Vector3(-90, 0, Vector3.Angle(Vector3.left, b) * Vector3.Cross(Vector3.left, b).normalized.y);
            //Debug.Log(transform.localEulerAngles);

        }
        Debug.Log("IM HERE");
    }

    void TurningBars(float RotBar1, float RotBar2, float RotBar3)
    {
        if
        (Mathf.Abs(bar3.transform.localEulerAngles.z - RotBar3) < 1e-4 && Mathf.Abs(bar2.transform.localEulerAngles.z - RotBar2) < 1e-4 && Mathf.Abs(bar1.transform.localEulerAngles.z - RotBar1) < 1e-4)
            return;
        else
        {
            Wheel1ForSend = 0;
            Wheel2ForSend = 0;
            Wheel3ForSend = 0;

            bar1.transform.localEulerAngles = new Vector3(bar1.transform.localEulerAngles.x, bar1.transform.localEulerAngles.y, RotBar1);
            bar2.transform.localEulerAngles = new Vector3(bar2.transform.localEulerAngles.x, bar2.transform.localEulerAngles.y, RotBar2);
            bar3.transform.localEulerAngles = new Vector3(bar3.transform.localEulerAngles.x, bar3.transform.localEulerAngles.y, RotBar3);

            if (RotBar1 > 180) Bar1ForSend = RotBar1 - 360; else Bar1ForSend = RotBar1;
            if (RotBar2 > 180) Bar2ForSend = RotBar2 - 360; else Bar2ForSend = RotBar2;
            if (RotBar3 > 180) Bar3ForSend = RotBar3 - 360; else Bar3ForSend = RotBar3;


            StartCoroutine(Wait(2));

           // Debug.Log(bar1.transform.localEulerAngles.z + " " + RotBar1);

          //  Debug.Log(bar2.transform.localEulerAngles.z + " " + RotBar2);
          //  Debug.Log(bar3.transform.localEulerAngles.z + " " + RotBar3);

        }
       // else Debug.Log("else");
    }



    void TheSecondTypeMove()
    {
         Vector3 a1, a2, a3;

         a1 = BPointBar1.transform.position - APointBar1.transform.position;
         a2 = BPointBar2.transform.position - APointBar2.transform.position;
         a3 = BPointBar3.transform.position - APointBar3.transform.position;

         if (!isWaited)
            if (isMoved)
                if (Math.Abs(Vector3.Cross(b, a1).y) > 0.01 || Vector3.Dot(b, a1) < 0)
                {
                    //bar1.transform.localEulerAngles = new Vector3(0, 0, Vector3.Angle(Vector3.left, b) * Vector3.Cross(Vector3.left, b).normalized.y - Vector3.Angle(Vector3.left, a) * Vector3.Cross(Vector3.left, a).normalized.y);
                    //bar2.transform.localEulerAngles = new Vector3(0, 0, Vector3.Angle(Vector3.left, b) * Vector3.Cross(Vector3.left, b).normalized.y - Vector3.Angle(Vector3.left, a) * Vector3.Cross(Vector3.left, a).normalized.y);
                    //bar3.transform.localEulerAngles = new Vector3(0, 0, Vector3.Angle(Vector3.left, b) * Vector3.Cross(Vector3.left, b).normalized.y - Vector3.Angle(Vector3.left, a) * Vector3.Cross(Vector3.left, a).normalized.y);
                    //        StartCoroutine(Wait(2));
                    TurningBars(Vector3.Angle(Vector3.left, b) * Vector3.Cross(Vector3.left, b).normalized.y - Vector3.Angle(Vector3.left, a) * Vector3.Cross(Vector3.left, a).normalized.y, Vector3.Angle(Vector3.left, b) * Vector3.Cross(Vector3.left, b).normalized.y - Vector3.Angle(Vector3.left, a) * Vector3.Cross(Vector3.left, a).normalized.y, Vector3.Angle(Vector3.left, b) * Vector3.Cross(Vector3.left, b).normalized.y - Vector3.Angle(Vector3.left, a) * Vector3.Cross(Vector3.left, a).normalized.y);
                }
                else
                {
                    wheel1.transform.Rotate(0, -WheelAngleSpeed, 0);
                    wheel2.transform.Rotate(0, -WheelAngleSpeed, 0);
                    wheel3.transform.Rotate(0, -WheelAngleSpeed, 0);

                    Wheel1ForSend = WheelAngleSpeed * WheelRadius * 1000;
                    Wheel2ForSend = WheelAngleSpeed * WheelRadius * 1000;
                    Wheel3ForSend = WheelAngleSpeed * WheelRadius * 1000;

                    transform.position += b.normalized * WheelAngleSpeed * WheelRadius * Mathf.PI / 180f;
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
            //  GameObject.Find("Camera").GetComponent<raycast>().aims[0]
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
        
        if (ToSend == true && GameObject.Find("TelegaMode").GetComponent<TelegaModeSwitch>().isTelegaMode == true)
        {
            //   Debug.Log("send");
            // Net.Sender(RobotCommands.TelegaMoving());
            StartCoroutine(WaitToSend(0.1f));
        }

        switch (Type)
        {
            case MoveType.RotateTeleguAndMove:
                TheFirstTypeMove();
                break;
            case MoveType.RotateBarsAndMove:
                TheSecondTypeMove();
                break;
        }

        if (isMoved == false)

        {
            Wheel1ForSend = 0;
            Wheel2ForSend = 0;
            Wheel3ForSend = 0;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.CapsLock))
            switch (Type)
            {
                case MoveType.RotateTeleguAndMove:
                    Type = MoveType.RotateBarsAndMove;
                    break;
                case MoveType.RotateBarsAndMove:
                    Type = MoveType.RotateTeleguAndMove;
                    break;
            }
    } 
}
