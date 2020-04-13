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


        public double NormalizedX
        {
            get { return (double)GetValue(NormalizedXProperty); }
            set { SetValue(NormalizedXProperty, value); }
        }


        public static readonly DependencyProperty NormalizedXProperty =
            DependencyProperty.Register("NormalizedX", typeof(double), typeof(Joystick));



        public double NormalizedY
        {
            get { return (double)GetValue(NormalizedYProperty); }
            set { SetValue(NormalizedYProperty, value); }
        }


        public static readonly DependencyProperty NormalizedYProperty =
            DependencyProperty.Register("NormalizedY", typeof(double), typeof(Joystick));




        public Joystick()
        {
            InitializeComponent();
        }

        private void centerKnob_Completed(object sender, EventArgs e) { }
        private Point firstPoint = new Point();



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
            double gradient, absX, absY;
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
                        gradient = y / x;
                        absX = Math.Sqrt(Math.Pow(Base.Width / 2, 2) / (Math.Pow(gradient, 2) + 1));
                        absY = absX * gradient;
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
            NormalizedX = knobPosition.X / 170;
            NormalizedY = (knobPosition.Y / 170) * -1;
        }

        /*
        * Auxilary method that calculates the mouse movement length vector.
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
            
        }

        private void Knob_MouseLeave(object sender, MouseEventArgs e)
        {
            knobPosition.X = 0;
            knobPosition.Y = 0;
        }
    }
}
