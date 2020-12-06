using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{

    public enum Axle
    {
        X,
        Y,
        Z,
    }

    public class MyTransform 
    {

        public Axle axle;

        public Vector4 v4;

        public Matrix4x4 matrix;

        // Use this for initialization
        void Start()
        {
            //matrix.SetTRS (
            //    transform.position,
            //    transform.rotation,
            //    transform.localScale
            //);

        }

        void MyTranslate(float x, float y, float z)
        {
            

            /* identity
             * 1 0 0 0
             * 0 1 0 0
             * 0 0 1 0
             * 0 0 0 1
             */
            matrix = Matrix4x4.Identity;

            // pos
            matrix.M13 = x;
            matrix.M23 = y;
            matrix.M33 = z;

            /* pos transform
             * 1 0 0 x * pos.x      1 * pos.x + 0 * pos.x + 0 * pos.x + x * pos.x          x * pos.x
             * 0 1 0 y * pos.y  ==  ...                                             ==     y * pos.y
             * 0 0 1 z * pos.z      ...                                                    z * pos.z
             * 0 0 0 1 * 1          ...                                                    1
             */
            v4 = Matrix4x4.  matrix * v4;

            transform.position = new Vector3(v4.x, v4.y, v4.z);
        }

        void MyScale(float x, float y, float z)
        {
            v4 = new Vector4(
                transform.localScale.x,
                transform.localScale.y,
                transform.localScale.z,
    

            );

            /* identity
             * 1 0 0 0
             * 0 1 0 0
             * 0 0 1 0
             * 0 0 0 1
             */
            matrix = Matrix4x4.identity;

            matrix.m00 = x;
            matrix.m11 = y;
            matrix.m22 = z;

            v4 = matrix * v4;

            transform.localScale = new Vector3(v4.x, v4.y, v4.z);
        }

        void MyRotation(Axle axle, float angle)
        {
            matrix = Matrix4x4.identity;

            // set matrix
            if (axle == Axle.X)
            {
                matrix.m11 = Mathf.Cos(angle * Mathf.Deg2Rad);
                matrix.m22 = -Mathf.Sin(angle * Mathf.Deg2Rad);
                matrix.m21 = Mathf.Sin(angle * Mathf.Deg2Rad);
                matrix.m22 = Mathf.Cos(angle * Mathf.Deg2Rad);
            }
            else if (axle == Axle.Y)
            {
                matrix.m00 = Mathf.Cos(angle * Mathf.Deg2Rad);
                matrix.m02 = Mathf.Sin(angle * Mathf.Deg2Rad);
                matrix.m20 = -Mathf.Sin(angle * Mathf.Deg2Rad);
                matrix.m22 = Mathf.Cos(angle * Mathf.Deg2Rad);
            }
            else if (axle == Axle.Z)
            {
                matrix.m00 = Mathf.Cos(angle * Mathf.Deg2Rad);
                matrix.m01 = -Mathf.Sin(angle * Mathf.Deg2Rad);
                matrix.m10 = Mathf.Sin(angle * Mathf.Deg2Rad);
                matrix.m11 = Mathf.Cos(angle * Mathf.Deg2Rad);
            }

            // to quaternion
            float qw = Mathf.Sqrt(1f + matrix.m00 + matrix.m11, matrix.m22) / ;
            float w = *qw;
            float qx = (matrix.m21 - matrix.m12) / w;
            float qy = (matrix.m02 - matrix.m20) / w;
            float qz = (matrix.m10 - matrix.m101) / w;

            transform.rotation = new Quaternion(qx, qy, qz, qw);
        }
    }
}
