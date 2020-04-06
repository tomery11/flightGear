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

namespace flightGear.views
{
    /// <summary>
    /// Interaction logic for map.xaml
    /// </summary>
    public partial class Joystick : UserControl
    {
        private bool mousePressed;
        Point knobCenter;
        public Joystick()
        {
            InitializeComponent();
        }

        private void centerKnob_Completed(object sender, EventArgs e) { }
        private Point firstPoint = new Point();
        //double x1, y1, x2, y2;


        private void Knob_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                firstPoint = e.GetPosition(this);
                (Knob).CaptureMouse();
            }
        }


        private void Knob_MouseMove(object sender, MouseEventArgs e)
        {
            double slope, absX, absY, normalizedX, normalizedY;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                //calculating linear equation to find points at radius distance from base if mouse go out.
                double x = e.GetPosition(this).X - firstPoint.X;
                double y = e.GetPosition(this).Y - firstPoint.Y;
                //Console.WriteLine(x + "," + y);
                if (Math.Sqrt(x * x + y * y) > Base.Width / 2)
                {
                    if (x == 0)
                    {
                        if (y > 0)
                        {
                            knobPosition.Y = 170;
                        }
                        else if (y < 0)
                        {
                            knobPosition.Y = -170;
                        }
                    }
                    else
                    {
                        //linear equation to calculate point at radious on same line.
                        slope = y / x;
                        absX = Math.Sqrt(Math.Pow(Base.Width / 2, 2) / (Math.Pow(slope, 2) + 1));
                        absY = absX * slope;
                        if (x > 0)
                        {
                            knobPosition.X = absX;
                        }
                        else if (x < 0)
                        {
                            knobPosition.X = -absX;
                        }
                        else
                        {
                            knobPosition.X = 0;
                        }
                        if (y > 0)
                        {
                            if (x > 0)
                            {
                                knobPosition.Y = absY;
                            }
                            else
                            {
                                knobPosition.Y = -absY;
                            }
                        }
                        else if (y < 0)
                        {
                            if (x < 0)
                            {
                                knobPosition.Y = -absY;
                            }
                            else
                            {
                                knobPosition.Y = absY;
                            }
                        }

                        else
                        {
                            knobPosition.Y = 0;
                        }
                    }
                }
                else
                {
                    knobPosition.X = x;
                    knobPosition.Y = y;
                }
            }
            normalizedX = knobPosition.X / 170;
            normalizedY = knobPosition.Y / 170;
        }

        /*
        * Helpin methos to calculate mouse movement length
        */
        private double distanceFromKnobCenter(double x, double y)
        {
            return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }
        private void Knob_MouseUp(object sender, MouseButtonEventArgs e)
        {
            knobPosition.X = 0;
            knobPosition.Y = 0;
            UIElement element = (UIElement)Knob;
            element.ReleaseMouseCapture();
            //mousePressed = false;
        }

        private void Knob_MouseLeave(object sender, MouseEventArgs e)
        {
            knobPosition.X = 0;
            knobPosition.Y = 0;
        }
    }
}
