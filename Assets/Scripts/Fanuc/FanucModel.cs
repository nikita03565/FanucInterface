using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class FanucModel : RoboModel
{
    public static readonly float[] limMin = new float[] { -170f, -70f, -70f, -200f, -140f, -270f };
    public static readonly float[] limMax = new float[] { 170f, 90f, 200f, 200f, 140f, 270f };

    public static float[] JointsToQ(ref float[] j)
    {
        return new float[]{j[0] * Mathf.PI / 180.0f, -j[1] * Mathf.PI / 180.0f + Mathf.PI / 2,
         (j[1] + j[2]) * Mathf.PI / 180.0f, -j[3] * Mathf.PI / 180.0f,
          j[4] * Mathf.PI / 180.0f, -j[5] * Mathf.PI / 180.0f };

    }
    
    public FanucModel(int n, ref float[][] input) : base(n, input)
    {
    }

    public FanucModel() : base(6, new float[][] {
    new float[] { 0, 0, 150, Mathf.PI / 2 },
    new float[] { 0, 0, 790, 0 },
    new float[] { 0, 0, 250, Mathf.PI / 2 },
    new float[] { 835, 0, 0, -Mathf.PI / 2 },
    new float[] { 0, 0, 0, Mathf.PI / 2 },
    new float[] { 100, 0, 0, 0 },
    new float[] { 130, Mathf.PI / 2, -90, 0 },
    new float[] { -190, 0, 0, 0 }
    })
    {
    }

    public Matrix4x4 fanucForwardTask(ref float[] inputJoints)
    {
        float[] q = JointsToQ(ref inputJoints);
        return ForwardTask(q);
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
        wprAngles[0] * 180f / Mathf.PI, wprAngles[1] * 180f / Mathf.PI, wprAngles[2] * 180f / Mathf.PI };
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
        Matrix4x4 mx = new Matrix4x4
            (new Vector4(1f, 0f, 0f, 0f), new Vector4(0f, Mathf.Cos(w), Mathf.Sin(w), 0f),
             new Vector4(0f, -Mathf.Sin(w), Mathf.Cos(w), 0f), new Vector4(0f, 0f, 0f, 1f));
        Matrix4x4 my = new Matrix4x4
            (new Vector4(Mathf.Cos(p), 0f, -Mathf.Sin(p), 0f), new Vector4(0f, 1f, 0, 0f),
             new Vector4(Mathf.Sin(p), 0f, Mathf.Cos(p), 0f), new Vector4(0f, 0f, 0f, 1f));
        Matrix4x4 mz = new Matrix4x4
            (new Vector4(Mathf.Cos(r), Mathf.Sin(r), 0f, 0f), new Vector4(-Mathf.Sin(r), Mathf.Cos(r), 0f, 0f),
             new Vector4(0f, 0f, 1f, 0f), new Vector4(0f, 0f, 0f, 1f));
        return mz * my * mx;
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

    public float[,] InverseTask(ref float[] coordIn)//, ref float[] jointCoordCur)
    {
        var param = KinematicChain;

        Matrix4x4 rot = FanucModel.rotMatrix(coordIn[3] * Mathf.Deg2Rad, coordIn[4] * Mathf.Deg2Rad, coordIn[5] * Mathf.Deg2Rad);
        float[] coord = new float[]
        {
            coordIn[0] - rot[0, 2] * 100.0f,
            coordIn[1] - rot[1, 2] * 100.0f,
            coordIn[2] - rot[2, 2] * 100.0f,
            coordIn[3],
            coordIn[4],
            coordIn[5]
        };

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

        float[] x = new float[4];
        int numberOfRoots = Poly34.SolveP4(ref x, s / r, t / r, u / r, v / r);

        if (numberOfRoots != 2 && numberOfRoots != 4)
        {
            Debug.Log("something is wrong with roots of equatation");
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
            costheta = (a11 * (coord[0] * Mathf.Cos(theta[it, 0]) + coord[1] * Mathf.Sin(theta[it, 0]) - param[0]._aParam)
                - a12 * (-coord[0] * Mathf.Cos(param[0]._alphaParam) * Mathf.Sin(theta[it, 0]) + coord[1] *
                    Mathf.Cos(param[0]._alphaParam) * Mathf.Cos(theta[it, 0]) + (coord[2] - param[0]._dParam) * Mathf.Sin(param[0]._alphaParam)))
                / (a11 * a11 + a12 * a12);
            sintheta = (a12 * (coord[0] * Mathf.Cos(theta[it, 0]) + coord[1] * Mathf.Sin(theta[it, 0]) - param[0]._aParam) + a11 *
                (-coord[0] * Mathf.Cos(param[0]._alphaParam) * Mathf.Sin(theta[it, 0]) + coord[1] *
                    Mathf.Cos(param[0]._alphaParam) *
                    Mathf.Cos(theta[it, 0]) + (coord[2] - param[0]._dParam) * Mathf.Sin(param[0]._alphaParam))) / (a11 * a11 + a12 * a12);

            theta[it, 1] = Mathf.Atan2(sintheta, costheta);
        }

        for (int it = 0; it < numberOfRoots; ++it)
        {
            theta[it, 1] = -theta[it, 1] + Mathf.PI / 2;
            theta[it, 2] -= theta[it, 1];
        }
        
        List<int> ind = new List<int>();

        for (int it = 0; it < numberOfRoots; ++it)
        {
            bool isOk = true;
            for (int jt = 0; jt < 3; ++jt)
            {
                if (theta[it, jt] > limMax[jt] * Mathf.Deg2Rad || theta[it, jt] < limMin[jt] * Mathf.Deg2Rad)
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
            Debug.Log("POS!!!!!!!!!!!!!!!!!!!!!!");
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
        
        Debug.Log("ind.Count = " + ind.Count.ToString());
        
        for (int it = 0; it < ind.Count; ++it)
        {

            float[] q = new float[6];
            q[0] = thetaPrefinal[it * k, 0];
            q[1] = -thetaPrefinal[it * k, 1] + Mathf.PI / 2;
            q[2] = thetaPrefinal[it * k, 2] + thetaPrefinal[it * k, 1];
            
            Matrix4x4 r03 = FanucModel.qi(param[0]._alphaParam, q[0]) * FanucModel.qi(param[1]._alphaParam, q[1]) * FanucModel.qi(param[2]._alphaParam, q[2]);
            Matrix4x4 r36 = r03.inverse * FanucModel.rotMatrix(coord[3] * Mathf.Deg2Rad, coord[4] * Mathf.Deg2Rad, coord[5] * Mathf.Deg2Rad);

            float xi = r36[0, 2];
            float y = r36[1, 2];
            float z = r36[2, 2];

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

            thetaPrefinal[it * k, 5] = -Mathf.Atan2(s61, c61);
            thetaPrefinal[it * k + 1, 5] = -Mathf.Atan2(s62, c62);

            thetaPrefinal[it * k, 3] = -thetaPrefinal[it * k, 3];
            thetaPrefinal[it * k + 1, 3] = -thetaPrefinal[it * k + 1, 3];
        }
        
        List<int> indFinal = new List<int>();
       
        for (int it = 0; it < ind.Count * k; ++it)
        {
            bool isOk = true;

            for (int jt = 3; jt < 6; ++jt)
            {
                if (Mathf.Abs(thetaPrefinal[it, jt]) > limMax[jt] * Mathf.Deg2Rad)
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
            Debug.Log("OR!!!!!!!!!!!!!!!!!!!!!");
            return new float[0, 0];
        }

        float[,] thetaFinal = new float[indFinal.Count, 6];
        //Debug.Log("indFinal.Count = " + indFinal.Count.ToString());
        for (int it = 0; it < indFinal.Count; ++it)
        {
            thetaFinal[it, 0] = thetaPrefinal[indFinal[it], 0] * Mathf.Rad2Deg;
            thetaFinal[it, 1] = thetaPrefinal[indFinal[it], 1] * Mathf.Rad2Deg;
            thetaFinal[it, 2] = thetaPrefinal[indFinal[it], 2] * Mathf.Rad2Deg;
            thetaFinal[it, 3] = thetaPrefinal[indFinal[it], 3] * Mathf.Rad2Deg;
            thetaFinal[it, 4] = thetaPrefinal[indFinal[it], 4] * Mathf.Rad2Deg;
            thetaFinal[it, 5] = thetaPrefinal[indFinal[it], 5] * Mathf.Rad2Deg;
        }

        //System.String str = System.String.Empty;
        //int k1 = 0;
        //foreach (float temp in thetaFinal)
        //{
        //    str += ((temp).ToString() + " ");
        //    ++k1;
        //    if (k1 % 6 == 0) str += "\n";
        //    //Debug.Log(temp);
        //}
        //Debug.Log(str);
        return thetaFinal;
    }

    public static float[] chooseNearestPose(float[,] res, ref float[] prevPos)
    {
        if (res.Length > 0)
        {
            List<float> delta = new List<float>();
            for (int j = 0; j < res.Length / 6; ++j)
            {
                float deltaTmp = 0;
                for (int t = 0; t < 6; ++t)
                {
                    deltaTmp += Mathf.Abs(res[j, t] - prevPos[t]);
                }
                //Debug.Log("deltaTmp = " + deltaTmp.ToString());
                delta.Add(deltaTmp);
            }
            if (delta.Count > 0)
            {
                int num = 0;
                float min = delta[0];
                for (int j = 0; j < delta.Count; ++j)
                {
                    if (delta[j] < min)
                    {
                        min = delta[j];
                        num = j;
                    }
                }
                return new float[] { res[num, 0], res[num, 1], res[num, 2], res[num, 3], res[num, 4], res[num, 5] };   
            }
            return prevPos;
        }
        return prevPos;
    }
}
   
