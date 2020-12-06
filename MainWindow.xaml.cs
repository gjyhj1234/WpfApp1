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
        List<Plane> stdPlanes = new List<Plane>();
        Plane stdPlane ;
        Plane plane;
        Plane plane2;
        Plane planexy;
        Plane planeyz;
        Plane planezx;

        private void makeStdPlanes()
        {
            float start = 1.5F;
            int pre = 3;
            stdPlane = vector.MakeParallelXZ(start);
            createotherLines(start, pre);
        }

        private void createotherLines(float start, float pre)
        {
            clearELL(TLEll);
            clearELL(TREll);
            clearELL(BLEll);
            clearELL(BREll);
            stdPlanes.Clear();
            for (int i = 5; i > 0; i--)
            {
                stdPlanes.Add(vector.MakeParallePlaneByOtherPlane(stdPlane,   i * pre));
            }
            stdPlanes.Add(stdPlane);
            for (int i = 1; i < 6; i++)
            {
                stdPlanes.Add(vector.MakeParallePlaneByOtherPlane(stdPlane,  - i * pre));
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var ag = -(float)Math.PI / 180F * 1F;
            var qu = Quaternion.CreateFromYawPitchRoll(0, 0, ag);
            var d = stdPlane.D;
            stdPlane = Plane.Transform(Plane.Normalize(stdPlane), qu);
            stdPlane = new Plane(stdPlane.Normal, d);
            createotherLines(0, 3);
            DrawLinesXY();
            //plane =  Plane.Transform(Plane.Normalize(plane), qu);
            //plane = new Plane(plane.Normal, d);

            //plane2 = new Plane(plane.Normal, d + 20);
            //reDrawPlanes();
        }
        Ellipse TLEll = null;
        Ellipse TREll = null;
        Ellipse BLEll = null;
        Ellipse BREll = null;
        private void clearELL(Ellipse ell)
        {
            if (ell != null)
            {
                Canvas1.Children.Remove(ell);
                ell = null;
            }
        }
        private void DrawLinesXY()
        {
            float ex1 = 0, ey1 = 0, ex2 = 0, ey2 = 0,k = 0,b = 0;
            while (uIElements1.Count > 0)
            {
                Canvas1.Children.Remove(uIElements1[0]);
                uIElements1.Remove(uIElements1[0]);
            }
            foreach (var pl in stdPlanes)
            {
                var ele = drawPlaneCrossXY(pl, ref ex1, ref ey1, ref ex2, ref ey2,ref k,ref b);
                if (ele != null)
                {
                    if (pl == stdPlanes[0])
                    {
                        var line = ele.Tag as StdLine;
                        TLEll = CreateEll(ele.X1, ele.Y1, line.x1,line.y1,1);
                        TREll = CreateEll(ele.X2, ele.Y2, line.x2, line.y2,2);
                    }
                    if (pl == stdPlanes[stdPlanes.Count - 1])
                    {
                        var line = ele.Tag as StdLine;
                        BLEll = CreateEll(ele.X1, ele.Y1, line.x1, line.y1,3);
                        BREll = CreateEll(ele.X2, ele.Y2, line.x2, line.y2,4);
                    }
                    uIElements1.Add(ele);
                }
            }
            Canvas1AddELL(TLEll);
            Canvas1AddELL(TREll);
            Canvas1AddELL(BLEll);
            Canvas1AddELL(BREll);
        }
        private Ellipse CreateEll(double x,double y,float orgx, float orgy,int pos )
        {
            var ell = new Ellipse();
            ell.Width = 6;
            ell.Height = 6;
            ell.Tag = new StdPoint { x1 = orgx, y1 = orgy,pos = pos };
            ell.Fill = Brushes.Blue;
            ell.SetValue(Canvas.LeftProperty, x - 3);
            ell.SetValue(Canvas.TopProperty, y - 3);
            makeevent(ell);
            return ell;
        }
        private void Canvas1AddELL(Ellipse ell)
        {
            if (ell != null)
                Canvas1.Children.Add(ell);
        }
        private void makeevent(Ellipse ell)
        {
            ell.MouseLeftButtonDown += Ell_MouseLeftButtonDown;
            ell.MouseLeftButtonUp += Ell_MouseLeftButtonUp;
        }
        
        Ellipse CurrentELL = null;
        private void Ell_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var ell = sender as Ellipse;
            ell.Fill = Brushes.Blue;
            if(CurrentELL!=null)
                CurrentELL.Fill = Brushes.Blue;
            CurrentELL = null;
        }
        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (CurrentELL != null)
                CurrentELL.Fill = Brushes.Blue;
            CurrentELL = null;
        }
        private void Ell_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var ell = sender as Ellipse;
            ell.Fill = Brushes.GreenYellow;
            CurrentELL = ell;
        }
        private void Canvas1_MouseLeave(object sender, MouseEventArgs e)
        {
            if (CurrentELL != null)
            {
                CurrentELL.Fill = Brushes.Blue;
            }
            CurrentELL = null;
        }
        private void Canvas1_MouseMove(object sender, MouseEventArgs e)
        {
            if (CurrentELL != null)
            {
                var std = CurrentELL.Tag as StdPoint;
                if (std.x1 != 0)
                {
                    var sin0 = std.y1 / std.x1;
                    var jds =  Math.Asin(sin0);
                    if (double.IsNaN(jds))
                        return;
                    var p = e.GetPosition(Canvas1);
                    var x2 = p.X - Canvas1.Width / 2;
                    var y2 =   Canvas1.Height / 2 - p.Y ;
                    if (x2 != 0)
                    {
                        var jde = Math.Asin(y2 / x2);
                        if (jde != jds && !double.IsNaN(jde))
                        {
                            int pos = std.pos;
                            var ag = -(float) (Math.PI / 180F * ( -jde + jds));
                            var qu = Quaternion.CreateFromYawPitchRoll(0, 0, ag);
                            var d = stdPlane.D;
                            stdPlane = Plane.Transform(Plane.Normalize(stdPlane), qu);
                            if (float.IsNaN(stdPlane.Normal.X) || float.IsNaN(stdPlane.Normal.Y) || float.IsNaN(stdPlane.Normal.Z))
                            {
                                var dddd = 1;
                            }
                            stdPlane = new Plane(stdPlane.Normal, d);
                            createotherLines(0, 3);
                            DrawLinesXY();
                            switch (pos)
                            {
                                case 1:
                                    CurrentELL = TLEll;
                                    break;
                                case 2:
                                    CurrentELL = TREll;
                                    break;
                                case 3:
                                    CurrentELL = BLEll;
                                    break;
                                case 4:
                                    CurrentELL = BREll;
                                    break;
                            }
                            CurrentELL.Fill = Brushes.GreenYellow;
                        }
                    }
                }
              
            }
        }
        private void getnewpoint2(float k,float b,float L,Point curPoint,ref Point p1,ref Point p2)
        {
            double A = Math.Pow(k, 2) + 1;// A=k^2+1;
            double B = 2 * ((b - curPoint.Y) * k - curPoint.X);// B=2[(b-y0)k-x0];
            // C=(b-y0)^2+x0^2-L^2
            double C = Math.Pow(b - curPoint.Y, 2) + Math.Pow(curPoint.X, 2)
                    - Math.Pow(L, 2);
            // 两根x1,x2= [-B±√(B^2-4AC)]/2A
            if (A == 0)
            {
                var aaaa = 1;
            }
            double x1 = (-B + Math.Sqrt(Math.Pow(B, 2) - 4 * A * C)) / (2 * A);
            double x2 = (-B - Math.Sqrt(Math.Pow(B, 2) - 4 * A * C)) / (2 * A);
            double y1 = k * x1 + b;
            double y2 = k * x2 + b;
            p1 = new Point(x1, y1);
            p2 = new Point(x2, y2);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            drawCoordinate();
            //drawline(-500,1,500,1);
            planexy = vector.MakeXY();
            planeyz = vector.MakeYZ();
            planezx = vector.MakeZX();

            makeStdPlanes();
            DrawLinesXY();
           


            // resetlines();


            //var d = Vector4.Distance(new Vector4(plane.Normal.X, plane.Normal.Y, plane.Normal.Z, plane.D),
            
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
           
        }
        private void reDrawPlanes()
        {

            clearlines();

            float ez1 = 0, ex1 = 0, ez2 =0 , ex2 = 0,ey1 = 0,ey2 = 0,ek = 0, eb = 0;
            float fz1 = 0, fx1 = 0, fz2 = 0, fx2 = 0, fy1 = 0, fy2 = 0, fk = 0, fb = 0;
            var e1 = drawPlaneCrossXY(plane,ref ex1,ref ey1,ref ex2,ref ey2,ref ek,ref eb);
            var e2 = drawPlaneCrossXY(plane2, ref fx1, ref fy1, ref fx2, ref fy2,ref fk,ref fb);

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
                
            }
            var e5 = drawPlaneCrossZX(plane, ref ez1, ref ex1, ref ez2, ref ex2);
            var e6 = drawPlaneCrossZX(plane2, ref fz1, ref fx1, ref fz2, ref fx2);

            if (e5 != null)
            {
                uIElements3.Add(e5);
                uIElements3.Add(e6);
                //hd21.setZB("-z", "-x", "p3.png");
             
            }
        }

        /// <summary>
        /// 画一条平面与xy平面的交线
        /// </summary>
        /// <param name="plane"></param>
        private Line drawPlaneCrossXY(Plane plane, ref float x1, ref float y1, ref float x2, ref float y2, ref float k, ref float b, float len  =180)
        {
            if (float.IsNaN(plane.Normal.X) || float.IsNaN(plane.Normal.Y) || float.IsNaN(plane.Normal.Z))
                return null;
            Vector3 point = new Vector3(0, 0, 0);
            if (vector.CheckHasLine(plane, planexy, out Vector3 vector3, ref point))
            {
                //表达式 Ax+By+Cz+d=0
                //A、B都为0
                if (plane.Normal.Y == 0 && plane.Normal.X == 0)
                {
                    x1 = -len / 2;
                    x2 = len / 2;
                    y1 = y2 = 0;
                    k = 0;
                    b = 0;
                }
                //A = 0、B!=0
                else if (plane.Normal.Y != 0 && plane.Normal.X == 0)
                {
                    x1 = -len / 2;
                    x2 = len / 2;
                    y2 = y1 = -plane.D / plane.Normal.Y;
                    k = 0;
                    b = y2;
                }
                //A != 0、B = 0
                else if (plane.Normal.Y == 0 && plane.Normal.X != 0)
                {
                    y1 = -len / 2;
                    y2 = len / 2;
                    x2 = x1 = -plane.D / plane.Normal.X;
                    k = x2;
                    b = 0;
                }
                //A != 0、B != 0
                else
                {
                    x1 = 0;
                    y1 = -plane.D / plane.Normal.Y;

                    y2 = 0;
                    x2 = -plane.D / plane.Normal.X;
                    if (x1 == x2)
                    {

                        var aaa = 1;
                    }
                    k = (y1 - y2) / (x1 - x2);
                    if (float.IsNaN(k))
                    {
                        var aaa = 1;
                    }
                    b = y1 - k * x1;

                    Point p1 = new Point();
                    Point p2 = new Point();
                    getnewpoint2(k, b, len / 2, new Point(0, 0),ref p1,ref p2);

                    x1 = (float)p1.X;
                    y1 = k * x1 + b;
                    x2 = (float)p2.X;
                    y2 = k * x2 + b;

                    //x1 = -500;
                    //y1 = k * x1 + b;
                    //x2 = 500;
                    //y2 = k * x2 + b;
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
        private Line drawline(float ix1, float iy1, float ix2, float iy2,Canvas canvas, System.Windows.Media.Brush brushes)
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
            StdLine stdLine = new StdLine { x1 = ix1, x2 = ix2, y1 = iy1, y2 = iy2 };
            myLine.Tag = stdLine;
            //myLine.HorizontalAlignment = HorizontalAlignment.Left;
            //myLine.VerticalAlignment = VerticalAlignment.Center;
            canvas.Children.Add(myLine);
            return myLine;
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

           

        }

        
    }
}
