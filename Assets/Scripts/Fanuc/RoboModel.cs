using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class RoboModel
    {
        protected int N;

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
    }
}
