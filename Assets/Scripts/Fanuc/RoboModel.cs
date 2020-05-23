using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public abstract class RoboModel
{
    protected int N;

    public float[] limMin;
    public float[] limMax;

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
    public Matrix4x4 ForwardTask(float[] input)
    {
        float[] inputq = JointsToQ(ref input);
        _kinematicChain[0]._qParam = inputq[0];
        Matrix4x4 transformMatrix = PrevMatTransform(0);
        for (int i = 1; i < inputq.Length; ++i)
        {
            _kinematicChain[i]._qParam = inputq[i];
            transformMatrix = transformMatrix * PrevMatTransform(i);
        }
        return transformMatrix;
    }

    public static bool nearlyEqual(float a, float b, float epsilon=0.01f) {
    float absA = Mathf.Abs(a);
    float absB = Mathf.Abs(b);
    float diff = Mathf.Abs(a - b);

    if (a == b) { // shortcut, handles infinities
        return true;
    } else if (a == 0 || b == 0 || absA + absB < 0.001f) {
        // a or b is zero or both are extremely close to it
        // relative error is less meaningful here
        return diff < (epsilon * 0.001f);
    } else { // use relative error
        return diff / (absA + absB) < epsilon;
    }
}

    // coords: x y z w p r
    public float[,] InverseTask(ref float[] coordIn)
    {
        var param = KinematicChain;
        float[] coord = CalculateWrist(ref coordIn);
        //Debug.Log("check C: " + coord[0] + " " + coord[1] + ", " + (coord[0] * coord[0] + coord[1] * coord[1] < 0.001f));
        float a = 2.0f * param[0]._aParam * coord[0];
        float b = 2.0f * param[0]._aParam * coord[1];
        float c = 2.0f * param[1]._aParam * param[2]._aParam - 2.0f * param[1]._dParam * param[3]._dParam *
            Mathf.Sin(param[1]._alphaParam) * Mathf.Sin(param[2]._alphaParam);
        float d = 2.0f * param[2]._aParam * param[1]._dParam * Mathf.Sin(param[1]._alphaParam) + 2.0f * param[1]._aParam * param[3]._dParam
            * Mathf.Sin(param[2]._alphaParam);
        float e = param[1]._aParam * param[1]._aParam + param[2]._aParam * param[2]._aParam + param[1]._dParam *
            param[1]._dParam + param[2]._dParam * param[2]._dParam + param[3]._dParam * param[3]._dParam -
            param[0]._aParam * param[0]._aParam - coord[0] * coord[0] - coord[1] * coord[1] -
            (coord[2] - param[0]._dParam) * (coord[2] - param[0]._dParam) + 2.0f *
            param[1]._dParam * param[2]._dParam * Mathf.Cos(param[1]._alphaParam) + 2.0f * param[1]._dParam * param[3]._dParam *
            Mathf.Cos(param[1]._alphaParam) * Mathf.Cos(param[2]._alphaParam) + 2.0f * param[2]._dParam * param[3]._dParam * Mathf.Cos(param[2]._alphaParam);
        float f = coord[1] * Mathf.Sin(param[0]._alphaParam);
        float g = -coord[0] * Mathf.Sin(param[0]._alphaParam);
        float h = -param[3]._dParam * Mathf.Sin(param[1]._alphaParam) * Mathf.Sin(param[2]._alphaParam);
        float i = param[2]._aParam * Mathf.Sin(param[1]._alphaParam);
        float j = param[1]._dParam + param[2]._dParam * Mathf.Cos(param[1]._alphaParam) + param[3]._dParam *
            Mathf.Cos(param[1]._alphaParam) * Mathf.Cos(param[2]._alphaParam) - (coord[2] - param[0]._dParam) *
            Mathf.Cos(param[0]._alphaParam);
        float r = 4.0f * param[0]._aParam * param[0]._aParam * (j - h) * (j - h) + Mathf.Sin(param[0]._alphaParam) *
            Mathf.Sin(param[0]._alphaParam) * (e - c) * (e - c)
            - 4.0f * param[0]._aParam * param[0]._aParam * Mathf.Sin(param[0]._alphaParam) * Mathf.Sin(param[0]._alphaParam)
            * (coord[0] * coord[0] + coord[1] * coord[1]);
        float s = 4.0f * (4.0f * param[0]._aParam * param[0]._aParam * i * (j - h) + Mathf.Sin(param[0]._alphaParam) *
            Mathf.Sin(param[0]._alphaParam) * d *
            (e - c));
        float t = 2.0f * (4.0f * param[0]._aParam * param[0]._aParam * (j * j - h * h + 2.0f * i * i) +
            Mathf.Sin(param[0]._alphaParam) * Mathf.Sin(param[0]._alphaParam)
            * (e * e - c * c + 2.0f * d * d) - 4.0f * param[0]._aParam * param[0]._aParam *
            Mathf.Sin(param[0]._alphaParam) * Mathf.Sin(param[0]._alphaParam) *
            (coord[0] * coord[0] + coord[1] * coord[1]));
        float u = 4.0f * (4.0f * param[0]._aParam * param[0]._aParam * i * (j + h) +
            Mathf.Sin(param[0]._alphaParam) * Mathf.Sin(param[0]._alphaParam) * d * (e + c));
        float v = 4.0f * param[0]._aParam * param[0]._aParam * (h + j) * (h + j) + Mathf.Sin(param[0]._alphaParam) *
            Mathf.Sin(param[0]._alphaParam) *
            (e + c) * (e + c) - 4.0f * param[0]._aParam * param[0]._aParam * Mathf.Sin(param[0]._alphaParam) *
            Mathf.Sin(param[0]._alphaParam) *
            (coord[0] * coord[0] + coord[1] * coord[1]);
        float delta1 = a * g - f * b;
        // param[0]._aParam || Mathf.Sin(param[0]._alphaParam)
        //Debug.Log("check delta1" + delta1 + ", " + param[0]._aParam + ", " + Mathf.Sin(param[0]._alphaParam));
        float[] x = new float[4];
        int numberOfRoots = Poly34.SolveP4(ref x, s / r, t / r, u / r, v / r);

        if (numberOfRoots != 2 && numberOfRoots != 4)
        {
            Debug.Log("something is wrong with roots of equation");
            return new float[0, 0];
        }

        float[,] theta = new float[numberOfRoots, 3];

        for (int it = 0; it < numberOfRoots; ++it)
        {
            theta[it, 2] = 2.0f * Mathf.Atan(x[it]);
        }

        float costheta, sintheta;
        for (int it = 0; it < numberOfRoots; ++it)
        {
            costheta = (-g * (c * Mathf.Cos(theta[it, 2]) + d * Mathf.Sin(theta[it, 2]) + e) +
                b * (h * Mathf.Cos(theta[it, 2]) + i * Mathf.Sin(theta[it, 2] + j))) / (a * g - f * b);
            sintheta = (f * (c * Mathf.Cos(theta[it, 2]) + d * Mathf.Sin(theta[it, 2]) + e) -
                a * (h * Mathf.Cos(theta[it, 2]) + i * Mathf.Sin(theta[it, 2] + j))) / (a * g - f * b);

            theta[it, 0] = Mathf.Atan2(sintheta, costheta);
        }

        for (int it = 0; it < numberOfRoots; ++it)
        {
            float a11 = param[1]._aParam + param[2]._aParam * Mathf.Cos(theta[it, 2]) +
                param[3]._dParam * Mathf.Sin(param[2]._alphaParam) * Mathf.Sin(theta[it, 2]);
            float a12 = -param[2]._aParam * Mathf.Cos(param[1]._alphaParam) *
                Mathf.Sin(theta[it, 2]) + param[2]._dParam * Mathf.Sin(param[1]._alphaParam) + param[3]._dParam *
                Mathf.Sin(param[2]._alphaParam) * Mathf.Cos(param[1]._alphaParam) * Mathf.Cos(theta[it, 2]) +
                param[3]._dParam * Mathf.Sin(param[1]._alphaParam) * Mathf.Cos(param[2]._alphaParam);
            float delta2 = (a11 * a11 + a12 * a12);
            costheta = (a11 * (coord[0] * Mathf.Cos(theta[it, 0]) + coord[1] * Mathf.Sin(theta[it, 0]) - param[0]._aParam)
                - a12 * (-coord[0] * Mathf.Cos(param[0]._alphaParam) * Mathf.Sin(theta[it, 0]) + coord[1] *
                    Mathf.Cos(param[0]._alphaParam) * Mathf.Cos(theta[it, 0]) + (coord[2] - param[0]._dParam) * Mathf.Sin(param[0]._alphaParam)))
                / delta2;
            sintheta = (a12 * (coord[0] * Mathf.Cos(theta[it, 0]) + coord[1] * Mathf.Sin(theta[it, 0]) - param[0]._aParam) + a11 *
                (-coord[0] * Mathf.Cos(param[0]._alphaParam) * Mathf.Sin(theta[it, 0]) + coord[1] *
                    Mathf.Cos(param[0]._alphaParam) *
                    Mathf.Cos(theta[it, 0]) + (coord[2] - param[0]._dParam) * Mathf.Sin(param[0]._alphaParam))) / delta2;
            //Debug.Log("delta2: " + delta2);
            theta[it, 1] = Mathf.Atan2(sintheta, costheta);
        }
        
        List<int> ind = new List<int>();

        for (int it = 0; it < numberOfRoots; ++it)
        {
            bool isOk = true;
            float[] q = {theta[it, 0], theta[it, 1], theta[it, 2], 0f, 0f, 0f};
            var joints = JointsToQReverse(ref q);
            for (int jt = 0; jt < 3; ++jt)
            {
                if (joints[jt] > limMax[jt] * Mathf.Deg2Rad || joints[jt] < limMin[jt] * Mathf.Deg2Rad)
                {
                    isOk = false;
                }
            }
            
            if (!float.IsNaN(theta[it, 1]) && isOk)
            {
                ind.Add(it);
            }
        }

        if (ind.Count == 0)
        {
            Debug.Log("No solution for positioning task");
            return new float[0, 0];
        }
        //--------------------------------------------------------------

        int k = 2;
        float[,] thetaPrefinal = new float[ind.Count * k, 6];
        for (int it = 0; it < ind.Count; ++it)
        {
            for (int zt = 0; zt < k; ++zt)
            {
                thetaPrefinal[it * k + zt, 0] = theta[ind[it], 0];
                thetaPrefinal[it * k + zt, 1] = theta[ind[it], 1];
                thetaPrefinal[it * k + zt, 2] = theta[ind[it], 2];
            }
        }
        
        for (int it = 0; it < ind.Count; ++it)
        {    
            Matrix4x4 r03 = FanucModel.qi(param[0]._alphaParam, thetaPrefinal[it * k, 0]) * FanucModel.qi(param[1]._alphaParam, thetaPrefinal[it * k, 1]) * FanucModel.qi(param[2]._alphaParam, thetaPrefinal[it * k, 2]);
            Matrix4x4 r36 = r03.inverse * FanucModel.rotMatrix(coord[3] * Mathf.Deg2Rad, coord[4] * Mathf.Deg2Rad, coord[5] * Mathf.Deg2Rad);
            Matrix4x4 r06 = FanucModel.rotMatrix(coord[3] * Mathf.Deg2Rad, coord[4] * Mathf.Deg2Rad, coord[5] * Mathf.Deg2Rad);
            float xi = r36[0, 2];
            float y = r36[1, 2];
            float z = r36[2, 2];
            float xi1 = r06[0, 2];
            float y1 = r06[1, 2];
            float z1 = r06[2, 2];

            float xi2 = r03[0, 2];
            float y2 = r03[1, 2];
            float z2 = r03[2, 2];
            Vector3 v1 = new Vector3(xi1, y1, z1);
            Vector3 v2 = new Vector3(xi2, y2, z2);
            //Debug.Log("angle " + Vector3.Angle(v1, v2));
            if (Vector3.Angle(v1, v2) < 3f) {
                Debug.Log("ALARM SINGULARITY");
            };
            // Debug.Log("xi eta zeta: " + xi+" " +y+" "+z+"\n"+
            // "xi1 eta1 zeta1: " + xi1+" " +y1+" "+z1+"\n"+
            // "xi2 eta2 zeta2: " + xi2+" " +y2+" "+z2+"\n"
            // );

            float tau1 = (xi * Mathf.Sin(param[3]._alphaParam) + Mathf.Sqrt((xi * xi + y * y) * Mathf.Sin(param[3]._alphaParam) * Mathf.Sin(param[3]._alphaParam)
                - (Mathf.Cos(param[4]._alphaParam) - z * Mathf.Cos(param[3]._alphaParam)) * (Mathf.Cos(param[4]._alphaParam) - z * Mathf.Cos(param[3]._alphaParam))))
                / (Mathf.Cos(param[4]._alphaParam) - z * Mathf.Cos(param[3]._alphaParam) - y * Mathf.Sin(param[3]._alphaParam));
            float tau2 = (xi * Mathf.Sin(param[3]._alphaParam) - Mathf.Sqrt((xi * xi + y * y) * Mathf.Sin(param[3]._alphaParam) * Mathf.Sin(param[3]._alphaParam)
                - (Mathf.Cos(param[4]._alphaParam) - z * Mathf.Cos(param[3]._alphaParam)) * (Mathf.Cos(param[4]._alphaParam) - z * Mathf.Cos(param[3]._alphaParam))))
                / (Mathf.Cos(param[4]._alphaParam) - z * Mathf.Cos(param[3]._alphaParam) - y * Mathf.Sin(param[3]._alphaParam));

            thetaPrefinal[it * k, 3] = 2.0f * Mathf.Atan(tau1);
            thetaPrefinal[it * k + 1, 3] = 2.0f * Mathf.Atan(tau2);

            float s51 = ((Mathf.Sin(param[5]._alphaParam) * r36[0, 1] + Mathf.Cos(param[5]._alphaParam) * r36[0, 2]) * Mathf.Cos(thetaPrefinal[it * k, 3])
                + (Mathf.Sin(param[5]._alphaParam) * r36[1, 1] + Mathf.Cos(param[5]._alphaParam) * r36[1, 2]) * Mathf.Sin(thetaPrefinal[it * k, 3])) / Mathf.Sin(param[4]._alphaParam);

            float s52 = ((Mathf.Sin(param[5]._alphaParam) * r36[0, 1] + Mathf.Cos(param[5]._alphaParam) * r36[0, 2]) * Mathf.Cos(thetaPrefinal[it * k + 1, 3])
                + (Mathf.Sin(param[5]._alphaParam) * r36[1, 1] + Mathf.Cos(param[5]._alphaParam) * r36[1, 2]) * Mathf.Sin(thetaPrefinal[it * k + 1, 3])) / Mathf.Sin(param[4]._alphaParam);

            float c51 = (-Mathf.Cos(param[3]._alphaParam) * (Mathf.Sin(param[5]._alphaParam) * r36[0, 1] + Mathf.Cos(param[5]._alphaParam) * r36[0, 2]) * Mathf.Sin(thetaPrefinal[it * k, 3] = 2.0f * Mathf.Atan(tau1))
                + Mathf.Cos(param[3]._alphaParam) * (Mathf.Sin(param[5]._alphaParam) * r36[1, 1] + Mathf.Cos(param[5]._alphaParam) * r36[1, 2]) * Mathf.Cos(thetaPrefinal[it * k, 3] = 2.0f * Mathf.Atan(tau1))
                + Mathf.Sin(param[3]._alphaParam) * (Mathf.Sin(param[5]._alphaParam) * r36[2, 1] + Mathf.Cos(param[5]._alphaParam) * r36[2, 2])) / (-Mathf.Sin(param[4]._alphaParam));

            float c52 = (-Mathf.Cos(param[3]._alphaParam) * (Mathf.Sin(param[5]._alphaParam) * r36[0, 1] + Mathf.Cos(param[5]._alphaParam) * r36[0, 2]) * Mathf.Sin(thetaPrefinal[it * k + 1, 3])
                + Mathf.Cos(param[3]._alphaParam) * (Mathf.Sin(param[5]._alphaParam) * r36[1, 1] + Mathf.Cos(param[5]._alphaParam) * r36[1, 2]) * Mathf.Cos(thetaPrefinal[it * k + 1, 3])
                + Mathf.Sin(param[3]._alphaParam) * (Mathf.Sin(param[5]._alphaParam) * r36[2, 1] + Mathf.Cos(param[5]._alphaParam) * r36[2, 2])) / (-Mathf.Sin(param[4]._alphaParam));

            thetaPrefinal[it * k, 4] = (s51 >= 0 ? Mathf.Acos(c51) : -Mathf.Acos(c51));
            thetaPrefinal[it * k + 1, 4] = (s52 >= 0 ? Mathf.Acos(c52) : -Mathf.Acos(c52));

            float c61 = (r36[0, 0] * Mathf.Cos(thetaPrefinal[it * k, 3]) + r36[1, 0] * Mathf.Sin(thetaPrefinal[it * k, 3])) * Mathf.Cos(thetaPrefinal[it * k, 4])
                + (-Mathf.Cos(param[3]._alphaParam) * (r36[0, 0] * Mathf.Sin(thetaPrefinal[it * k, 3]) - r36[1, 0] * Mathf.Cos(thetaPrefinal[it * k, 3])) + Mathf.Sin(param[3]._alphaParam) * r36[2, 0])
            * Mathf.Sin(thetaPrefinal[it * k, 4]);

            float c62 = (r36[0, 0] * Mathf.Cos(thetaPrefinal[it * k + 1, 3]) + r36[1, 0] * Mathf.Sin(thetaPrefinal[it * k + 1, 3])) * Mathf.Cos(thetaPrefinal[it * k + 1, 4])
                + (-Mathf.Cos(param[3]._alphaParam) * (r36[0, 0] * Mathf.Sin(thetaPrefinal[it * k + 1, 3]) - r36[1, 0] * Mathf.Cos(thetaPrefinal[it * k + 1, 3])) + Mathf.Sin(param[3]._alphaParam) * r36[2, 0])
                * Mathf.Sin(thetaPrefinal[it * k + 1, 4]);

            float s61 = -Mathf.Cos(param[4]._alphaParam) * (r36[0, 0] * Mathf.Cos(thetaPrefinal[it * k, 3]) + r36[1, 0] * Mathf.Sin(thetaPrefinal[it * k, 3])) * Mathf.Sin(thetaPrefinal[it * k, 4])
                + Mathf.Cos(param[4]._alphaParam) * (-Mathf.Cos(param[3]._alphaParam) * (r36[0, 0] * Mathf.Sin(thetaPrefinal[it * k, 3]) - r36[1, 0] * Mathf.Cos(thetaPrefinal[it * k, 3])) + Mathf.Sin(param[3]._alphaParam) * r36[2, 0])
            * Mathf.Cos(thetaPrefinal[it * k, 4]) + (Mathf.Sin(param[3]._alphaParam) * (r36[0, 0] * Mathf.Sin(thetaPrefinal[it * k, 3]) - r36[1, 0] * Mathf.Cos(thetaPrefinal[it * k, 3])) + Mathf.Cos(param[3]._alphaParam) * r36[2, 0]) * Mathf.Sin(param[4]._alphaParam);

            float s62 = -Mathf.Cos(param[4]._alphaParam) * (r36[0, 0] * Mathf.Cos(thetaPrefinal[it * k + 1, 3]) + r36[1, 0] * Mathf.Sin(thetaPrefinal[it * k + 1, 3])) * Mathf.Sin(thetaPrefinal[it * k + 1, 4])
                + Mathf.Cos(param[4]._alphaParam) * (-Mathf.Cos(param[3]._alphaParam) * (r36[0, 0] * Mathf.Sin(thetaPrefinal[it * k + 1, 3]) - r36[1, 0] * Mathf.Cos(thetaPrefinal[it * k + 1, 3])) + Mathf.Sin(param[3]._alphaParam) * r36[2, 0])
                * Mathf.Cos(thetaPrefinal[it * k + 1, 4]) + (Mathf.Sin(param[3]._alphaParam) * (r36[0, 0] * Mathf.Sin(thetaPrefinal[it * k + 1, 3]) - r36[1, 0]* Mathf.Cos(thetaPrefinal[it * k + 1, 3])) + Mathf.Cos(param[3]._alphaParam) * r36[2, 0]) * Mathf.Sin(param[4]._alphaParam);

            thetaPrefinal[it * k, 5] = Mathf.Atan2(s61, c61);
            thetaPrefinal[it * k + 1, 5] = Mathf.Atan2(s62, c62);
        }
        
        List<int> indFinal = new List<int>();

        for (int it = 0; it < ind.Count * k; ++it)
        {
            bool isOk = true;
            float[] q = {thetaPrefinal[it, 0], thetaPrefinal[it, 1], thetaPrefinal[it, 2], thetaPrefinal[it, 3], thetaPrefinal[it, 4], thetaPrefinal[it, 5]};
            var joints = JointsToQReverse(ref q);
            for (int jt = 0; jt < 6; ++jt)
            {
                thetaPrefinal[it, jt] = joints[jt] * Mathf.Rad2Deg;
            }
            
            for (int jt = 3; jt < 6; ++jt)
            {
                if (thetaPrefinal[it, jt] > limMax[jt] || thetaPrefinal[it, jt] < limMin[jt])
                {
                    isOk = false;
                }
            }
            
            if (isOk && !float.IsNaN(thetaPrefinal[it, 5]))
            {
                indFinal.Add(it);
            }
        }

        if (indFinal.Count == 0)
        {
            Debug.Log("No solution");
            return new float[0, 0];
        }

        float[,] thetaFinal = new float[indFinal.Count, 6];
        for (int it = 0; it < indFinal.Count; ++it)
        {
            for (int jt = 0; jt < 6; ++jt) 
            {
                thetaFinal[it, jt] = thetaPrefinal[indFinal[it], jt];
            }
        }
        return thetaFinal;
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
   
