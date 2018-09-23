using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanucCollision : MonoBehaviour
{
    void OnTriggerStay(Collider col)
    {
       
        if (col.gameObject.tag == "fanuc")
        {
            SceneManager.fanuc.CollisionLimiter();
        }
    } 
}
