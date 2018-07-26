using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoordTransformation : MonoBehaviour {
    Matrix4x4 FromRobotToUnity = new Matrix4x4 
        (new Vector4(-1f, 0f, 0f, 0f), new Vector4(0f, 0f, -1f, 0f),
         new Vector4(0f, 1f, 0f, 0f), new Vector4(0f, 525f, 0f, 1f));
    Matrix4x4 FromUnityToRobot;
	
	void Start () {
        FromUnityToRobot = FromRobotToUnity.inverse;
        FromUnityToRobot[2, 3] /= 1000f;
        //Debug.Log(RobotToUnityPosOnly(new Vector4(838f, 75f, 300f, 1f)));
        //Debug.Log(UnityToRobotPosOnly(new Vector4(-0.838f, 0.825f, -0.075f, 1f)));
        //Matrix4x4 RobotCoord = new Matrix4x4(new Vector4(1f, 0f, 0f, 0f), new Vector4(0f, 1f, 0f, 0f),
        //    new Vector4(0f, 0f, 1f, 0f), new Vector4(838f, 75f, 300f, 1f));
        //Debug.Log(RobotCoord);
        //Debug.Log(RobotToUnity(RobotCoord));
        //Matrix4x4 UnityCoord = new Matrix4x4(new Vector4(1f, 0f, 0f, 0f), new Vector4(0f, 1f, 0f, 0f),
        //    new Vector4(0f, 0f, 1f, 0f), new Vector4(-0.838f, 0.825f, 0.075f, 1f));
        //Debug.Log(UnityCoord);
        //Debug.Log(UnityToRobot(UnityCoord));
    }
	
    Vector4 RobotToUnityPosOnly(Vector4 vec)
    {
        Vector4 tmp = FromRobotToUnity * vec;
        tmp[0] /= 1000f;
        tmp[1] /= 1000f;
        tmp[2] /= 1000f;
        return tmp;
    }

    Matrix4x4 RobotToUnity(Matrix4x4 input)
    {
        Matrix4x4 tmp = FromRobotToUnity * input;
        tmp[0, 3] /= 1000f;
        tmp[1, 3] /= 1000f;
        tmp[2, 3] /= 1000f;
        return tmp;
    }

    Vector4 UnityToRobotPosOnly(Vector4 vec)
    {
        Vector4 tmp = FromUnityToRobot * vec;
        tmp[0] *= 1000f;
        tmp[1] *= 1000f;
        tmp[2] *= 1000f;
        return tmp;
    }

    Matrix4x4 UnityToRobot(Matrix4x4 input)
    {
        Matrix4x4 tmp = FromUnityToRobot * input;
        tmp[0, 3] *= 1000f;
        tmp[1, 3] *= 1000f;
        tmp[2, 3] *= 1000f;
        return tmp;
    }
}
