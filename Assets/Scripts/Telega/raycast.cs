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
	
	// Update is called once per frame
	void Update ()
    {

        if (Input.GetMouseButton(0))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if ((Physics.Raycast(ray, out hit))&&
                (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false ||
                hit.collider.gameObject.name == "BallSpirit")&&
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

            if ((UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() == false)&&
               (Physics.Raycast(ray, out hit)))
            {       
                if (hit.collider.gameObject.name == "Plane")
                {
                    BallSpirit.GetComponent<MeshRenderer>().enabled = false;
                    aims.Add(new Vector3(hit.point.x, point.transform.position.y, hit.point.z));

                    balls.Add(Instantiate(BallPr,aims[aims.Count-1],Quaternion.identity));

                    if (balls.Count >= 10)
                    {
                        GameObject.Find("PointList").GetComponent<ContentSizeFitter>().enabled = true;
                        // GameObject.Find("PointList").transform.position = new Vector3(GameObject.Find("PointList").transform.position.x, GameObject.Find("RectObject").transform.position.y, GameObject.Find("PointList").transform.position.z);
                        GameObject.Find("Scrollbar").GetComponent<Scrollbar>().value = 0.00000f;
                    }
                }
            }
        }

        
        //PointList.text = "";
        for (int j = 0; j <= aims.Count-1; j++)
        {
            //PointList.text += aims[j].x.ToString("N2") + "  " + aims[j].z.ToString("N2") + "\r\n";
        }
    }
}
