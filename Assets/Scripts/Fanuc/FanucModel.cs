using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class FanucModel : RoboModel
{
    public override float[] JointsToQ(ref float[] j)
    {
        return new float[]{
            j[0] * Mathf.Deg2Rad,
           -j[1] * Mathf.Deg2Rad + Mathf.PI / 2,
           (j[1] + j[2]) * Mathf.Deg2Rad,
           -j[3] * Mathf.Deg2Rad,
            j[4] * Mathf.Deg2Rad,
           -j[5] * Mathf.Deg2Rad 
           };
    }

    public override float[] JointsToQReverse(ref float[] q)
    {
        return new float[]{
            q[0],
           -q[1] + Mathf.PI / 2,
           (q[2] - (-q[1] + Mathf.PI / 2)),
           -q[3],
            q[4],
           -q[5] 
           };

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
        limMin = new float[] { -170f, -70f, -70f, -200f, -140f, -270f };
        limMax = new float[] { 170f, 90f, 200f, 200f, 140f, 270f };
    }

    // coords: x y z w p r
    public override float[] CalculateWrist(ref float[] coords)
    {
        var param = KinematicChain;
        Matrix4x4 rot = FanucModel.rotMatrixDegrees(coords[3], coords[4], coords[5]);
        float[] coord = new float[]
        {
            coords[0] - rot[0, 2] * param[5]._dParam,
            coords[1] - rot[1, 2] * param[5]._dParam,
            coords[2] - rot[2, 2] * param[5]._dParam,
            coords[3],
            coords[4],
            coords[5]
        };
        return coord;
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
   
