using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        VectorHelper vector = new VectorHelper();
        public MainWindow()
        {
            InitializeComponent();

        }
        Plane plane;
        Plane plane2;
        Plane planexy;
        Plane planeyz;
        Plane planezx;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            drawCoordinate();
            //drawline(-500,1,500,1);
            planexy = vector.MakeXY();
            planeyz = vector.MakeYZ();
            planezx = vector.MakeZX();
            hd1.setZB("-y", "-z","p2.png");
            hd2.setZB("-y", "z", "p2.png");
            hd3.setZB("y", "-z", "p2.png");
            hd4.setZB("z", "y", "p2.png");
            hd5.setZB("-z", "-y", "p2.png");
            hd6.setZB("z", "-y", "p2.png");
            hd7.setZB("-z", "y", "p2.png");

            hd21.setZB("-z", "-x", "p3.png");
            hd22.setZB("z", "x", "p3.png");
            hd23.setZB("z", "-x", "p3.png");
            hd24.setZB("x", "z", "p3.png");
            hd25.setZB("-x", "-z", "p3.png");
            hd26.setZB("x", "-z", "p3.png");
            hd27.setZB("-x", "z", "p3.png");


            resetlines();


            //var d = Vector4.Distance(new Vector4(plane.Normal.X, plane.Normal.Y, plane.Normal.Z, plane.D),
            //    new Vector4(plane2.Normal.X, plane2.Normal.Y, plane2.Normal.Z, plane2.D));
            ////TxShow.Text = $"place:D = {plane.D},Normal:{plane.Normal};place2:D = {plane2.D},Normal:{plane2.Normal};d = {d}";
            //var planexy = vector.MakeXY();
            //var planexz = vector.MakeXZ();
            //var vector4XY = new Vector4(planexy.Normal.X, planexy.Normal.Y, planexy.Normal.Z, planexy.D);
            //var vector4XZ = new Vector4(1, 0, 1, 0);
            //var vector4YZ = new Vector4(0, 1, 1, 0);
            ////TxShow.Text = $"{plane.Normal.X } * x + {plane.Normal.Y } * y + {plane.Normal.Z } * z + {plane.D} = 0 ";
            ////TxShow_Copy.Text = $"{vector4XY.X } * x + {vector4XY.Y } * y + {vector4XY.Z } * z  + {vector4XY.W} = 0 ";
            // Vector3 point = new Vector3(0, 0, 0);
            ////var rls = vector.CheckHasLine(planexy, plane, out Vector3 vector3, ref point);
            ////if (rls)
            ////{
            ////    TxShow_Copy.Text = $" {vector3.X } * x + {vector3.Y } * y + {vector3.Z } = 0 ";
            ////    TxShow_Copy1.Text = $"( {point.X } ,{point.Y }, {point.Z }) ";
            ////}
            ////旋转
            ////y,x,z
            //var ag = (float)Math.PI / 180F * 45F;
            //var qu = Quaternion.CreateFromYawPitchRoll(ag, ag, ag);
            //var newp = Plane.Transform(plane, qu);
            //var rls = vector.CheckHasLine(newp, planexz, out Vector3 vector3, ref point);
            //if (rls)
            //{
            //    TxShow_Copy.Text = $" {vector3.X } * x + {vector3.Y } * y + {vector3.Z } = 0 ";
            //    TxShow_Copy1.Text = $"( {point.X } ,{point.Y }, {point.Z }) ";
            //}
        }
        private void resetLines()
        {
            plane = vector.MakeParallelXZ();
            plane2 = vector.MakeParallePlaneByOtherPlane(plane, 20);
            reDrawPlanes();
        }
        private void clearlines()
        {
            while (uIElements1.Count > 0)
            {
                Canvas1.Children.Remove(uIElements1[0]);
                uIElements1.Remove(uIElements1[0]);
            }
            while (uIElements2.Count > 0)
            {
                Canvas2.Children.Remove(uIElements2[0]);
                uIElements2.Remove(uIElements2[0]);
            }
            while (uIElements3.Count > 0)
            {
                Canvas3.Children.Remove(uIElements3[0]);
                uIElements3.Remove(uIElements3[0]);
            }
            hd1.Clear();
            hd2.Clear();
            hd3.Clear();
            hd4.Clear();
            hd5.Clear();
            hd6.Clear();
            hd7.Clear();
            hd21.Clear();
            hd22.Clear();
            hd23.Clear();
            hd24.Clear();
            hd25.Clear();
            hd26.Clear();
            hd27.Clear();
        }
        private void reDrawPlanes()
        {

            clearlines();

            float ez1 = 0, ex1 = 0, ez2 =0 , ex2 = 0,ey1 = 0,ey2 = 0;
            float fz1 = 0, fx1 = 0, fz2 = 0, fx2 = 0, fy1 = 0, fy2 = 0;
            var e1 = drawPlaneCrossXY(plane,ref ex1,ref ey1,ref ex2,ref ey2);
            var e2 = drawPlaneCrossXY(plane2, ref fx1, ref fy1, ref fx2, ref fy2);

            if (e1 != null)
            {
                uIElements1.Add(e1);
                uIElements1.Add(e2);
               
            }
            var e3 = drawPlaneCrossYZ(plane, ref ez1, ref ey1, ref ez2, ref ey2);
            var e4 = drawPlaneCrossYZ(plane2, ref fz1, ref fy1, ref fz2, ref fy2);

            if (e3 != null)
            {
                uIElements2.Add(e3);
                uIElements2.Add(e4);
                //hd1.setZB("-y", "-z", "p2.png");
                hd1.Draw(-ez1, -ey1, -ez2, -ey2, -fz1, -fy1, -fz2, -fy2);
                //hd2.setZB("-y", "z", "p2.png");
                hd2.Draw(ez1, -ey1, ez2, -ey2, fz1, -fy1, fz2, -fy2);
                //hd3.setZB("y", "-z", "p2.png");
                hd3.Draw(-ez1, ey1, -ez2, ey2, -fz1, fy1, -fz2, fy2);
                //hd4.setZB("z", "y", "p2.png");
                hd4.Draw(ey1, ez1, ey2, ez2, fy1, fz1, fy2, fz2);
                //hd5.setZB("-z", "-y", "p2.png");
                hd5.Draw(-ey1, -ez1, -ey2, -ez2, -fy1, -fz1, -fy2, -fz2);
                //hd6.setZB("z", "-y", "p2.png");
                hd6.Draw(-ey1, ez1, -ey2, ez2, -fy1, fz1, -fy2, fz2);
                //hd7.setZB("-z", "y", "p2.png");
                hd7.Draw(ey1, -ez1, ey2, -ez2, fy1, -fz1, fy2, -fz2);
            }
            var e5 = drawPlaneCrossZX(plane, ref ez1, ref ex1, ref ez2, ref ex2);
            var e6 = drawPlaneCrossZX(plane2, ref fz1, ref fx1, ref fz2, ref fx2);

            if (e5 != null)
            {
                uIElements3.Add(e5);
                uIElements3.Add(e6);
                //hd21.setZB("-z", "-x", "p3.png");
                hd21.Draw(-ex1, -ez1, -ex2, -ez2, -fx1, -fz1, -fx2, -fz2);
                //hd22.setZB("z", "x", "p3.png");
                hd22.Draw(ex1, ez1, ex2, ez2, fx1, fz1, fx2, fz2);
                //hd23.setZB("z", "-x", "p3.png");
                hd23.Draw(-ex1, ez1, -ex2, ez2, -fx1, fz1, -fx2, fz2);
                //hd24.setZB("x", "z", "p3.png");
                hd24.Draw(ez1, ex1, ez2, ex2, fz1, fx1, fz2, fx2);
                //hd25.setZB("-x", "-z", "p3.png");
                hd25.Draw(-ez1, -ex1, -ez2, -ex2, -fz1, -fx1, -fz2, -fx2);
                //hd26.setZB("x", "-z", "p3.png");
                hd26.Draw(-ez1, ex1, -ez2, ex2, -fz1, fx1, -fz2, fx2);
                //hd27.setZB("-x", "z", "p3.png");
                hd27.Draw(ez1, -ex1, ez2, -ex2, fz1, -fx1, fz2, -fx2);
            }
        }

        /// <summary>
        /// 画一条平面与xy平面的交线
        /// </summary>
        /// <param name="plane"></param>
        private UIElement drawPlaneCrossXY(Plane plane,ref float x1, ref float y1, ref float x2, ref float y2)
        {
            Vector3 point = new Vector3(0, 0, 0);
            if (vector.CheckHasLine(plane, planexy, out Vector3 vector3, ref point))
            {
                //表达式 Ax+By+Cz+d=0
                //A、B都为0
                if (plane.Normal.Y == 0 && plane.Normal.X == 0)
                {
                    x1 = -500;
                    x2 = 500;
                    y1 = y2 = 0;
                }
                //A = 0、B!=0
                else if (plane.Normal.Y != 0 && plane.Normal.X == 0)
                {
                    x1 = -500;
                    x2 = 500;
                    y2 = y1 = -plane.D / plane.Normal.Y;
                }
                //A != 0、B = 0
                else if (plane.Normal.Y == 0 && plane.Normal.X != 0)
                {
                    y1 = -500;
                    y2 = 500;
                    x2 = x1 = -plane.D / plane.Normal.X;
                }
                //A != 0、B != 0
                else
                {
                    x1 = 0;
                    y1 = -plane.D / plane.Normal.Y;

                    y2 = 0;
                    x2 = -plane.D / plane.Normal.X;

                    float k = (y1 - y2) / (x1 - x2);
                    float b = y1 - k * x1;
                    x1 = -500;
                    y1 = k * x1 + b;
                    x2 = 500;
                    y2 = k * x2 + b;
                }
                return drawline(x1, y1, x2, y2, Canvas1, Brushes.Red);
            }
            return null;
        }
        private UIElement drawPlaneCrossYZ(Plane plane, ref float z1, ref float y1, ref float z2, ref float y2)
        {
            Vector3 point = new Vector3(0, 0, 0);
            if (vector.CheckHasLine(plane, planeyz, out Vector3 vector3, ref point))
            {
                //表达式 Ax+By+Cz+d=0
                //B、Z都为0
                if (plane.Normal.Y == 0 && plane.Normal.Z == 0)
                {
                    z1 = -500;
                    z2 = 500;
                    y1 = y2 = 0;
                }
                //A = 0、B!=0
                else if (plane.Normal.Y != 0 && plane.Normal.Z == 0)
                {
                    z1 = -500;
                    z2 = 500;
                    y2 = y1 = -plane.D / plane.Normal.Y;
                }
                //A != 0、B = 0
                else if (plane.Normal.Y == 0 && plane.Normal.Z != 0)
                {
                    y1 = -500;
                    y2 = 500;
                    z2 = z1 = -plane.D / plane.Normal.Z;
                }
                //A != 0、B != 0
                else
                {
                    z1 = 0;
                    y1 = -plane.D / plane.Normal.Y;

                    y2 = 0;
                    z2 = -plane.D / plane.Normal.Z;

                    float k = (y1 - y2) / (z1 - z2);
                    float b = y1 - k * z1;
                    z1 = -500;
                    y1 = k * z1 + b;
                    z2 = 500;
                    y2 = k * z2 + b;
                }
                //注意这里是先z后y
                return drawline(z1, y1, z2, y2, Canvas2, Brushes.Orange);
            }
            return null;
        }
        private UIElement drawPlaneCrossZX(Plane plane, ref float z1, ref float x1, ref float z2, ref float x2)
        {
            Vector3 point = new Vector3(0, 0, 0);
            if (vector.CheckHasLine(plane, planezx, out Vector3 vector3, ref point))
            {
                //表达式 Ax+By+Cz+d=0
                //A、C都为0
                
                if (plane.Normal.X == 0 && plane.Normal.Z == 0)
                {
                    z1 = -500;
                    z2 = 500;
                    x1 = x2 = 0;
                }
                //A = 0、B!=0
                else if (plane.Normal.X != 0 && plane.Normal.Z == 0)
                {
                    z1 = -500;
                    z2 = 500;
                    x2 = x1 = -plane.D / plane.Normal.X;
                }
                //A != 0、B = 0
                else if (plane.Normal.X == 0 && plane.Normal.Z != 0)
                {
                    x1 = -500;
                    x2 = 500;
                    z2 = z1 = -plane.D / plane.Normal.Z;
                }
                //A != 0、B != 0
                else
                {
                    z1 = 0;
                    x1 = -plane.D / plane.Normal.X;

                    x2 = 0;
                    z2 = -plane.D / plane.Normal.Z;

                    float k = (z1 - z2) / (x1 - x2);
                    float b = z1 - k * x1;
                    x1 = -500;
                    z1 = k * x1 + b;
                    x2 = 500;
                    z2 = k * x2 + b;
                }
                //注意这里是先x后z
                return drawline(x1, - z1, x2,  - z2, Canvas3, Brushes.Blue);
            }
            return null;
        }
        private void drawCoordinate()
        {
            Canvas1.ClipToBounds = true;
            Canvas2.ClipToBounds = true;
            Canvas3.ClipToBounds = true;
            drawCoordinateX(Canvas1);
            drawCoordinateY(Canvas1);

            drawCoordinateX(Canvas2);
            drawCoordinateY(Canvas2);
           
            drawCoordinateX(Canvas3);
            drawCoordinateY(Canvas3);
        }
        private void drawCoordinateX(Canvas canvas)
        {
            //将坐标系原点移到中心
            Line myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.Gray;
            myLine.StrokeThickness = 0.5;
            myLine.X1 = 0;
            myLine.Y1 = canvas.Height / 2;
            myLine.X2 = canvas.Width;
            myLine.Y2 = canvas.Height / 2;
            //myLine.HorizontalAlignment = HorizontalAlignment.Left;
            //myLine.VerticalAlignment = VerticalAlignment.Center;
            canvas.Children.Add(myLine);
        }
        private void drawCoordinateY(Canvas canvas)
        {
            //将坐标系原点移到中心
            Line myLine = new Line();
            myLine.StrokeThickness = 0.5;
            myLine.Stroke = System.Windows.Media.Brushes.Gray;
            myLine.X1 = canvas.Width / 2;
            myLine.Y1 =0;
            myLine.X2 = canvas.Width / 2;
            myLine.Y2 = canvas.Height;
            //myLine.HorizontalAlignment = HorizontalAlignment.Left;
            //myLine.VerticalAlignment = VerticalAlignment.Center;
            canvas.Children.Add(myLine);
        }
        List<UIElement> uIElements1 = new List<UIElement>();
        List<UIElement> uIElements2 = new List<UIElement>();
        List<UIElement> uIElements3 = new List<UIElement>();
        private UIElement drawline(float ix1, float iy1, float ix2, float iy2,Canvas canvas, System.Windows.Media.Brush brushes)
        {
            //将坐标系原点移到中心
            var y1 = canvas.Height / 2 -  iy1;
            var x1 = canvas.Width / 2 + ix1;
            var y2 = canvas.Height / 2 - iy2;
            var x2 = canvas.Width / 2 + ix2;
            Line myLine = new Line();
            myLine.Stroke = brushes;
            myLine.X1 = x1;
            myLine.Y1 = y1;
            myLine.X2 = x2;
            myLine.Y2 = y2;
            //myLine.HorizontalAlignment = HorizontalAlignment.Left;
            //myLine.VerticalAlignment = VerticalAlignment.Center;
            canvas.Children.Add(myLine);
            return myLine;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var ag = - (float)Math.PI / 180F * 1F;
            var qu = Quaternion.CreateFromYawPitchRoll(0, 0, ag);
            var d = plane.D;
            plane =  Plane.Transform(Plane.Normalize(plane), qu);
            plane = new Plane(plane.Normal, d);
            plane2 = new Plane(plane.Normal, d + 20);
            reDrawPlanes();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var ag =  (float)Math.PI / 180F * 1F;
            var qu = Quaternion.CreateFromYawPitchRoll(0, 0, ag);
            var d = plane.D;
            plane = Plane.Transform(Plane.Normalize(plane), qu);
            plane = new Plane(plane.Normal, d);
            plane2 = new Plane(plane.Normal, d + 20);
            reDrawPlanes();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            clearlines();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            resetlines();
        }

        private void resetlines()
        {
            plane = vector.MakeParallelXZ();
            plane2 = vector.MakeParallePlaneByOtherPlane(plane, 20);
            reDrawPlanes();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var ag = -(float)Math.PI / 180F * 1F;
            var qu = Quaternion.CreateFromYawPitchRoll(0, ag, 0);
            var d = plane.D;
            plane = Plane.Transform(Plane.Normalize(plane), qu);
            plane = new Plane(plane.Normal, d);
            plane2 = new Plane(plane.Normal, d + 20);
            reDrawPlanes();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            var ag = (float)Math.PI / 180F * 1F;
            var qu = Quaternion.CreateFromYawPitchRoll(0, ag, 0);
            var d = plane.D;
            plane = Plane.Transform(Plane.Normalize(plane), qu);
            plane = new Plane(plane.Normal, d);
            plane2 = new Plane(plane.Normal, d + 20);
            reDrawPlanes();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            var ag = -(float)Math.PI / 180F * 1F;
            var qu = Quaternion.CreateFromYawPitchRoll(ag, 0 , 0);
            var d = plane.D;
            plane = Plane.Transform(Plane.Normalize(plane), qu);
            plane = new Plane(plane.Normal, d);
            plane2 = new Plane(plane.Normal, d + 20);
            reDrawPlanes();
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            var ag = (float)Math.PI / 180F * 1F;
            var qu = Quaternion.CreateFromYawPitchRoll(ag, 0, 0);
            var d = plane.D;
            plane = Plane.Transform(Plane.Normalize(plane), qu);
            plane = new Plane(plane.Normal, d);
            plane2 = new Plane(plane.Normal, d + 20);
            reDrawPlanes();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void ck_Click(object sender, RoutedEventArgs e)
        {

            hd1.showimg(ck.IsChecked.Value);
            hd2.showimg(ck.IsChecked.Value);
            hd3.showimg(ck.IsChecked.Value);
            hd4.showimg(ck.IsChecked.Value);
            hd5.showimg(ck.IsChecked.Value);
            hd6.showimg(ck.IsChecked.Value);
            hd7.showimg(ck.IsChecked.Value);
            hd21.showimg(ck.IsChecked.Value);
            hd22.showimg(ck.IsChecked.Value);
            hd23.showimg(ck.IsChecked.Value);
            hd24.showimg(ck.IsChecked.Value);
            hd25.showimg(ck.IsChecked.Value);
            hd26.showimg(ck.IsChecked.Value);
            hd27.showimg(ck.IsChecked.Value);

        }
    }
}
