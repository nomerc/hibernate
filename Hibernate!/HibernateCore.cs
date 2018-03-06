using System;
using System.Windows;
using System.ComponentModel;
using System.Threading;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
/// <summary>
/// New commit
/// </summary>
namespace Hibernate_
{
    class HibernateCore : DependencyObject, INotifyPropertyChanged
    {
        public static readonly DependencyProperty HoursProperty;
        public static readonly DependencyProperty MinutesProperty;

        public event PropertyChangedEventHandler PropertyChanged;

        private static DispatcherTimer countTimer;

        static HibernateCore()
        {
            HoursProperty = DependencyProperty.Register("Hours", typeof(int), typeof(HibernateCore),
                new FrameworkPropertyMetadata(01, FrameworkPropertyMetadataOptions.AffectsRender, PropertyChangedCallback));
            MinutesProperty = DependencyProperty.Register("Minutes", typeof(int), typeof(HibernateCore),
                new FrameworkPropertyMetadata(30, FrameworkPropertyMetadataOptions.AffectsRender, PropertyChangedCallback));

        }

        public HibernateCore()
        {
            countTimer = new DispatcherTimer();
            countTimer.Tick += On_Tick;
            countTimer.Interval = new TimeSpan(0, 0, 1);
            countTimer.Start();
        }

        private void On_Tick(object sender, EventArgs e)
        {
            Minutes--;
        }


        public int Hours
        {

            set
            {
                SetValue(HoursProperty, value);
                NotifyPropertyChanged("Hours");
            }
            get { return (int)GetValue(HoursProperty); }
        }

        public int Minutes
        {
            set
            {
                SetValue(MinutesProperty, value);
                NotifyPropertyChanged("Minutes");
            }
            get { return (int)GetValue(MinutesProperty); }
        }

        private void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            //converting from static object to get access to instance properties
            HibernateCore obj = (HibernateCore)dependencyObject;//memory
            //color
            //obj.br.Color = obj.Clr;
            //obj.textFrame.Fill = obj.br;

            //obj.AdaptFrame();
        }
    }

}
