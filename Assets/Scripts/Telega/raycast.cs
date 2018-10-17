using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class raycast : MonoBehaviour {

    public Camera cam;
    public GameObject point;
    public Plane plane;

  
    public GameObject BallPr;
    public GameObject BallSpirit;
    public Text PointList;

    public List<Vector3> aims;
    public List<GameObject> balls;

    private Ray ray;
    private RaycastHit hit;

	// Use this for initialization
	void Start ()
    {
        
    }
	
    IEnumerator DropScrollbar()
    {
        yield return new WaitForSeconds(0.02f);
        GameObject.Find("ScrollbarTelega").GetComponent<Scrollbar>().value = 0f;
    }
	// Update is called once per frame
	void Update ()
    {
        if (!SceneManager.telega.telega.isMoved)
        {
            if (Input.GetMouseButton(0))
            {
                ray = cam.ScreenPointToRay(Input.mousePosition);
                if ((Physics.Raycast(ray, out hit)) &&
                    (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false ||
                    hit.collider.gameObject.name == "BallSpirit") &&
                   (hit.collider.gameObject.name == "Plane"))
                {
                    BallSpirit.transform.position = new Vector3(hit.point.x, point.transform.position.y, hit.point.z);
                    BallSpirit.GetComponent<MeshRenderer>().enabled = true;

                    //   point.transform.position = new Vector3(hit.point.x, point.transform.position.y, hit.point.z);
                }
                else BallSpirit.GetComponent<MeshRenderer>().enabled = false;
            }

            if (Input.GetMouseButtonUp(0))
            {
                ray = cam.ScreenPointToRay(Input.mousePosition);

                //Physics.RaycastAll(ray);

                if ((UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false) &&
                   (Physics.Raycast(ray, out hit)))
                {
                    BallSpirit.GetComponent<MeshRenderer>().enabled = false;
                    aims.Add(new Vector3(hit.point.x, point.transform.position.y, hit.point.z));

                    balls.Add(Instantiate(BallPr, aims[aims.Count - 1], Quaternion.identity));

                    if (balls.Count >= 10)
                    {
                        GameObject.Find("PointList").GetComponent<ContentSizeFitter>().enabled = true;
                        StartCoroutine("DropScrollbar");
                    }
                }
            }
            if (balls.Count < 10)
            {
                GameObject.Find("PointList").GetComponent<ContentSizeFitter>().enabled = false;
                GameObject.Find("PointList").transform.position = PointList.transform.parent.position;
                GameObject.Find("PointList").GetComponent<RectTransform>().sizeDelta = PointList.transform.parent.GetComponent<RectTransform>().sizeDelta; 
            }
        }
        PointList.text = "";
        for (int j = 0; j <= aims.Count - 1; ++j)
        {
            var aimsRobot = CoordTransformation.UnityToRobotPosOnly(new Vector4(aims[j].x, aims[j].y, aims[j].z, 1));
            PointList.text += aimsRobot[0].ToString("0.00") + "  " + aimsRobot[1].ToString("0.00") + "\r\n";
        }
    }
}
