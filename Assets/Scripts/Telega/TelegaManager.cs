using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TelegaManager : MonoBehaviour
{
    telegaScript telega;
    raycast rc;
    public Text textOnButton;
    public GameObject panel;

    public bool isTelegaMode;
    private new GameObject camera;

    public Vector3 A, B, a, b;

    public float[] velocity;
    public float[] isReversed;
    public float[] dist;
    public float[] angle;


    // Use this for initialization
    void Start()
    {
        velocity = new float[3] { 200f, 200f, 200f };
        angle = new float[3];
        dist = new float[3];
        isReversed = new float[3];

        rc = GameObject.Find("Camera").GetComponent<raycast>();
        telega = GameObject.Find("telega").GetComponent<telegaScript>();
        camera = GameObject.Find("Camera");
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Click()
    {
        if (!isTelegaMode)
        {
            isTelegaMode = true;
            camera.GetComponent<raycast>().enabled = true;
            camera.GetComponent<CameraOrbit>().SwitchMode();

            camera.GetComponent<Camera>().orthographic = true;
            textOnButton.text = "Normal mode";
            panel.transform.position = new Vector3(88, panel.transform.position.y, panel.transform.position.z);
            camera.GetComponent<CameraOrbit>().SwitchMode();
        }
        else
        {
            isTelegaMode = false;
            camera.GetComponent<raycast>().enabled = false;
            camera.GetComponent<Camera>().orthographic = false;
            textOnButton.text = "Telega Mode";
            panel.transform.position = new Vector3(-100, panel.transform.position.y, panel.transform.position.z);
        }
    }

    IEnumerator doThisShit()
    {

        rc.aims.Insert(0, telega.PointA.transform.position);
        rc.aims.Insert(0, telega.PointA.transform.position + telega.A - telega.B);

        for (int i = 0; i < rc.aims.Count; ++i)
        {
            rc.aims[i] = new Vector3(rc.aims[i].x, telega.transform.position.y, rc.aims[i].z);
        }

        for (int j = 1; j < rc.aims.Count - 1; ++j)
        {
            a = rc.aims[j] - rc.aims[j - 1];
            b = rc.aims[j + 1] - rc.aims[j];

            for (int i = 0; i < 3; ++i)
            {
                angle[i] = Vector3.Angle(Vector3.left, b) * Vector3.Cross(Vector3.left, b).normalized.y - Vector3.Angle(Vector3.left, a) * Vector3.Cross(Vector3.left, a).normalized.y;

                if (angle[i] > 180f) angle[i] = angle[i] - 360f;

                if (angle[i] > 120f)
                {
                    angle[i] -= 180f;
                    isReversed[i] = -1f;
                }
                else if (angle[i] < -120f)
                {
                    angle[i] += 180f;
                    isReversed[i] = -1f;
                }
                else
                {
                    isReversed[i] = 1f;
                }

                dist[i] = Mathf.Sqrt((rc.aims[j + 1].x - rc.aims[j].x) * (rc.aims[j + 1].x - rc.aims[j].x) +
                    (rc.aims[j + 1].z - rc.aims[j].z) * (rc.aims[j + 1].z - rc.aims[j].z)) * 1000f;
            }

            telega.net.Sender(RobotCommands.TelegaMoving());
            yield return new WaitForSeconds(0.5f);
            GameObject.Find("telega").GetComponent<telegaScript>().isMoved = true;
        }
        rc.aims.RemoveAt(0);
        rc.aims.RemoveAt(0);
        SceneManager.Net.Sender("end");

    }


    public void Synchronize()
    {
        StartCoroutine("doThisShit");
    }
}