using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    class FanucModel : RoboModel
    {
        /**
        * \brief Function for conversion joints angles to Denavit-Hartenberg generalized angles.
        * \param[in] j Joints angles.
        * \return Denavit-Hartenberg generalized angles.
        */
        public static float[] JointsToQ(ref float[] j)
        {
            return new float[]{j[0] * Mathf.PI / 180.0f, -j[1] * Mathf.PI / 180.0f + Mathf.PI / 2,
             (j[1] + j[2]) * Mathf.PI / 180.0f, -j[3] * Mathf.PI / 180.0f,
              j[4] * Mathf.PI / 180.0f, -j[5] * Mathf.PI / 180.0f };

        }

        /**
        * \brief Default constructor with Fanuc M20ia parameters.
        */
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

        /**
        * \brief Function for solving forward kinematic task for Fanuc M20ia.
        * \param[in] inputjoints Joints angles.
        * \return Coordinates of end-effector in world frame: x, y, z in mm and w, p, r in radians.
        */
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
        /**
        * \brief Calculates rotation matrix of end-effector. input angles given in radians.
        * \param[in] w Angle of rotation around x axis.
        * \param[in] p Angle of rotation around y axis.
        * \param[in] r Angle of rotation around z axis.
        * \return Rotation matrix 3*3.
        */
        // public static cv::Mat rotMatrix(const double& w, const double& p, const double& r);
    }
}
