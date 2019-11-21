using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public abstract class RoboModel
{
    protected int N;

    public float[] _limMin;
    public float[] _limMax;

    public float[] limMin
    {
        get
        {
            return _limMin;
        }
        set 
        {
            _limMin = value;
        }
    }

    public float[] limMax
    {
        get
        {
            return _limMax;
        }
        set 
        {
            _limMax = value;
        }
    }

    public abstract float[] JointsToQ(ref float[] j);
    public abstract float[] JointsToQReverse(ref float[] q);
    public abstract float[] CalculateWrist(ref float[] coords);

    /**
    * \brief Struct of Denavit-Hartenberg parameters.
    */
    public struct DhParameters
    {
        /**
         * \brief Offset along previous z to the common normal.
         */
        public float _dParam;

        /**
        * \brief Angle about previous  z, from old  x to new  x.
        */
        public float _qParam;

        /**
        * \brief Offset along x in current frame.
        */
        public float _aParam;

        /**
        * \brief Angle about x in current frame.
        */
        public float _alphaParam;

        /**
         * \brief Constructor with parameters.
         * \param[in] d D-H parameter.
         * \param[in] q D-H parameter.
         * \param[in] a D-H parameter.
         * \param[in] alpha D-H parameter.
         */
        public DhParameters(float d, float q, float a, float alpha)
        {
            _dParam = d;
            _qParam = q;
            _aParam = a;
            _alphaParam = alpha;
        }

        public DhParameters(float[] input)
        {
            _dParam = input[0];
            _qParam = input[1];
            _aParam = input[2];
            _alphaParam = input[3];
        }
    };

    /**
    * \brief Vector of parameters for each joint.
    */
    public DhParameters[] _kinematicChain;

    public DhParameters[] KinematicChain
    {
        get
        {
            return _kinematicChain;
        }
    }

    /**
    * \brief Function to calculate a transform matrix from i-th frame to (i-1)-th.
    * \param[in] i Number of coordinate frame.
    * \return Transform matrix (4x4).
    */
    protected Matrix4x4 PrevMatTransform(int i)
    {
        Matrix4x4 result = new Matrix4x4();
        result[0, 0] = Mathf.Cos(_kinematicChain[i]._qParam);
        result[0, 1] = -Mathf.Cos(_kinematicChain[i]._alphaParam) * Mathf.Sin(_kinematicChain[i]._qParam);
        result[0, 2] = Mathf.Sin(_kinematicChain[i]._alphaParam) * Mathf.Sin(_kinematicChain[i]._qParam);
        result[0, 3] = _kinematicChain[i]._aParam * Mathf.Cos(_kinematicChain[i]._qParam);

        result[1, 0] = Mathf.Sin(_kinematicChain[i]._qParam);
        result[1, 1] = Mathf.Cos(_kinematicChain[i]._alphaParam) * Mathf.Cos(_kinematicChain[i]._qParam);
        result[1, 2] = -Mathf.Sin(_kinematicChain[i]._alphaParam) * Mathf.Cos(_kinematicChain[i]._qParam);
        result[1, 3] = _kinematicChain[i]._aParam * Mathf.Sin(_kinematicChain[i]._qParam);

        result[2, 0] = 0;
        result[2, 1] = Mathf.Sin(_kinematicChain[i]._alphaParam);
        result[2, 2] = Mathf.Cos(_kinematicChain[i]._alphaParam);
        result[2, 3] = _kinematicChain[i]._dParam;

        result[3, 0] = result[3, 1] = result[3, 2] = 0;
        result[3, 3] = 1;

        return result;
    }

    /**
    * \brief Constructor with parameters for any robot.
    * \param[in] input Vector of secuences d, q, a, alpha.
    */
    protected RoboModel(int n, float[][] input)
    {
        _kinematicChain = new DhParameters[n];
        N = n;
        for (int i = 0; i < n; ++i)
        {
            _kinematicChain[i] = new DhParameters(input[i]);
        }
    }

    protected RoboModel()
    {
    }
    /**
    * \brief Function for solving forward kinematic task.
    * \param[in] inputq Generalized D-H coordinates.
    * \return Coordinates of end-effector in world frame: x, y, z in mm and w, p, r in radians.
    */
    protected Matrix4x4 ForwardTask(float[] inputq)
    {
        _kinematicChain[0]._qParam = inputq[0];
        Matrix4x4 transformMatrix = PrevMatTransform(0);
        for (int i = 1; i < inputq.Length; ++i)
        {
            _kinematicChain[i]._qParam = inputq[i];
            transformMatrix = transformMatrix * PrevMatTransform(i);
        }
        return transformMatrix;
    }

    public static float[] AnglesFromMat(Matrix4x4 p6)
    {
        float[] angleVector = new float[3];
        angleVector[0] = Mathf.Atan2(p6[2, 1], p6[2, 2]);
        angleVector[1] = Mathf.Atan2(-p6[2, 0], Mathf.Sqrt(p6[2, 1] * p6[2, 1] + p6[2, 2] * p6[2, 2]));
        angleVector[2] = Mathf.Atan2(p6[1, 0], p6[0, 0]);
        return angleVector;
    }

    public static float[] GetCoordsFromMat(Matrix4x4 transformMatrix)
    {
        float[] wprAngles = AnglesFromMat(transformMatrix);

        return new float[] {transformMatrix[0, 3], transformMatrix[1, 3], transformMatrix[2, 3],
        wprAngles[0] * Mathf.Rad2Deg, wprAngles[1] * Mathf.Rad2Deg, wprAngles[2] * Mathf.Rad2Deg };
    }
    
    public static Matrix4x4 rotMatrix(float w, float p, float r)
    {
        Matrix4x4 mx = new Matrix4x4
            (new Vector4(1f, 0f, 0f, 0f), new Vector4(0f, Mathf.Cos(w), Mathf.Sin(w), 0f),
             new Vector4(0f, -Mathf.Sin(w), Mathf.Cos(w), 0f), new Vector4(0f, 0f, 0f, 1f));
        Matrix4x4 my = new Matrix4x4
            (new Vector4(Mathf.Cos(p), 0f, -Mathf.Sin(p), 0f), new Vector4(0f, 1f, 0, 0f),
             new Vector4(Mathf.Sin(p), 0f , Mathf.Cos(p), 0f), new Vector4(0f, 0f, 0f, 1f));
        Matrix4x4 mz = new Matrix4x4
            (new Vector4(Mathf.Cos(r), Mathf.Sin(r), 0f, 0f), new Vector4(-Mathf.Sin(r), Mathf.Cos(r),0f, 0f),
             new Vector4(0f, 0f, 1f, 0f), new Vector4(0f, 0f, 0f, 1f));
        return mz * my * mx;
    }

    public static Matrix4x4 rotMatrixDegrees(float _w, float _p, float _r)
    {
        float w = _w * Mathf.Deg2Rad;
        float p = _p * Mathf.Deg2Rad;
        float r = _r * Mathf.Deg2Rad;
        return RoboModel.rotMatrix(w, p, r);
    }

    public static Matrix4x4 qi(float alpha, float q)
    {
        Matrix4x4 result = new Matrix4x4();
        result[0, 0] = Mathf.Cos(q);
        result[0, 1] = -Mathf.Cos(alpha) * Mathf.Sin(q);
        result[0, 2] = Mathf.Sin(alpha) * Mathf.Sin(q);
        result[0, 3] =  0;

        result[1, 0] = Mathf.Sin(q);
        result[1, 1] = Mathf.Cos(alpha) * Mathf.Cos(q);
        result[1, 2] = -Mathf.Sin(alpha) * Mathf.Cos(q);
        result[1, 3] =  0;

        result[2, 0] = 0;
        result[2, 1] = Mathf.Sin(alpha);
        result[2, 2] = Mathf.Cos(alpha);
        result[2, 3] = 0 ;

        result[3, 0] = result[3, 1] = result[3, 2] = 0;
        result[3, 3] = 1;

        return result;
    }

    public static Matrix4x4 coordMatrixDegrees(Vector3 pos, Vector3 rot)
    {
        Matrix4x4 res = rotMatrixDegrees(rot[0], rot[1], rot[2]);
        res[0, 3] = pos[0];
        res[1, 3] = pos[1];
        res[2, 3] = pos[2];
        res[3, 3] = 1;
        return res;
    }
}
   
