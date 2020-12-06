using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class VectorHelper
    {
        /// <summary>
        /// 定义一个与XZ面平行的面,且平面延法向量到原点距离为1
        /// </summary>
        /// <returns></returns>
        public Plane MakeParallelXZ(float d = 10)
        {
            Vector3 vectorOnYPoint = new Vector3(0, d, 0);
            Vector3 vectorOnYXPoint = new Vector3(1, d, 0);
            Vector3 vectorOnYZPoint = new Vector3(0, d, 1);
            return  Plane.CreateFromVertices(vectorOnYPoint, vectorOnYXPoint, vectorOnYZPoint);
        }
        public Plane MakeXY()
        { 
            Vector3 vectorOnYPoint = new Vector3(0, 1, 0);
            Vector3 vectorOnYXPoint = new Vector3(1, 0, 0);
            Vector3 vectorOnYZPoint = new Vector3(0, 0, 0);
            return Plane.CreateFromVertices(vectorOnYPoint, vectorOnYXPoint, vectorOnYZPoint);
        }
        public Plane MakeZX()
        {
            Vector3 vectorOnYPoint = new Vector3(0, 0, 0);
            Vector3 vectorOnYXPoint = new Vector3(1, 0, 0);
            Vector3 vectorOnYZPoint = new Vector3(0, 0, 1);
            return Plane.CreateFromVertices(vectorOnYPoint, vectorOnYXPoint, vectorOnYZPoint);
        }
      
        public Plane MakeYZ()
        {
            Vector3 vectorOnYPoint = new Vector3(0, 0, 0);
            Vector3 vectorOnYXPoint = new Vector3(0, 1, 0);
            Vector3 vectorOnYZPoint = new Vector3(0, 0, 1);
            return Plane.CreateFromVertices(vectorOnYPoint, vectorOnYXPoint, vectorOnYZPoint);
        }
        /// <summary>
        /// 定义一个与参考平面平行的面
        /// </summary>
        /// <returns></returns>
        public Plane MakeParallePlaneByOtherPlane(Plane plane,float d)
        {
            var p = Plane.Normalize(plane);
            return new Plane(p.Normal.X, p.Normal.Y, p.Normal.Z, plane.D + d);
            //return new Plane(plane., plane.D + d);
        }
        public bool CheckHasLine(Plane a, Plane b,out Vector3 p3_normal,ref Vector3 r_point)
        {
            p3_normal = Vector3.Cross(a.Normal, b.Normal);
            float det = p3_normal.LengthSquared();
            if (det != 0.0)
            {
                // calculate the final (point, normal)
                 r_point = ((Vector3.Cross( p3_normal,b.Normal) * a.D) +
                           (Vector3.Cross( a.Normal, p3_normal)* b.D)) / det;
                return true;
            }
            else
            {
                return false;
            }
        }
       
    }
}

