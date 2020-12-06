using System;
using System.Collections.Generic;
using System.Linq;
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
    /// hd.xaml 的交互逻辑
    /// </summary>
    public partial class hd : UserControl
    {
        VectorHelper vector = new VectorHelper();
        public hd()
        {
            InitializeComponent();
        }
        BitmapImage bimg = null;
        BitmapImage bkimg = null;
        public void setZB(string a, string b,string imgpath)
        {
            Canvas1.ClipToBounds = true;
            drawCoordinateX(Canvas1);
            drawCoordinateY(Canvas1);
            lb1.Content = a;
            lb2.Content = b;
            Uri uri = new Uri(imgpath, UriKind.Relative);
            bimg = new BitmapImage(uri);
            bkimg = new BitmapImage(new Uri("bk.png", UriKind.Relative));
            //Canvas1.Background = new ImageBrush(bimg);
        }
        public void showimg(bool show)
        {
            if (show)
                Canvas1.Background = new ImageBrush(bimg);
            else
                Canvas1.Background = new ImageBrush(bkimg );
        }
        public void Clear()
        {
            while (uIElements1.Count > 0)
            {
                Canvas1.Children.Remove(uIElements1[0]);
                uIElements1.Remove(uIElements1[0]);
            }
        }
        List<UIElement> uIElements1 = new List<UIElement>();
        public void Draw(float ix1, float iy1, float ix2, float iy2,
            float bix1, float biy1, float bix2, float biy2)
        {
            while (uIElements1.Count > 0)
            {
                Canvas1.Children.Remove(uIElements1[0]);
                uIElements1.Remove(uIElements1[0]);
            }
            drawline(ix1, iy1, ix2, iy2);
            drawline(bix1, biy1, bix2, biy2);
        }
        private UIElement drawline(float ix1, float iy1, float ix2, float iy2)
        {
            //将坐标系原点移到中心
            var y1 = Canvas1.Height / 2 - iy1;
            var x1 = Canvas1.Width / 2 + ix1;
            var y2 = Canvas1.Height / 2 - iy2;
            var x2 = Canvas1.Width / 2 + ix2;
            Line myLine = new Line();
            myLine.Stroke = System.Windows.Media.Brushes.OrangeRed;
            myLine.X1 = x1;
            myLine.Y1 = y1;
            myLine.X2 = x2;
            myLine.Y2 = y2;
            //myLine.HorizontalAlignment = HorizontalAlignment.Left;
            //myLine.VerticalAlignment = VerticalAlignment.Center;
            Canvas1.Children.Add(myLine);
            uIElements1.Add(myLine);
            return myLine;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
           
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
            myLine.Y1 = 0;
            myLine.X2 = canvas.Width / 2;
            myLine.Y2 = canvas.Height;
            //myLine.HorizontalAlignment = HorizontalAlignment.Left;
            //myLine.VerticalAlignment = VerticalAlignment.Center;
            canvas.Children.Add(myLine);
        }
    }
}
