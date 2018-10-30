using System;
using System.Windows;
using System.ComponentModel;
using System.Management;
using System.Threading;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Threading;

namespace Hibernate_
{

    class HibernateCore : DependencyObject, INotifyPropertyChanged
    {
        enum ShutdownOptions { Shutdown, Hibernate };


        public static readonly DependencyProperty HoursProperty;
        public static readonly DependencyProperty MinutesProperty;
        public static readonly DependencyProperty IsDecreasableProperty;

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly DispatcherTimer _countTimer;
        private static ShutdownOptions options = ShutdownOptions.Hibernate;

        static HibernateCore()
        {
            HoursProperty = DependencyProperty.Register("Hours", typeof(int), typeof(HibernateCore),
                new FrameworkPropertyMetadata(01, FrameworkPropertyMetadataOptions.AffectsRender, HoursChangedCallback));
            MinutesProperty = DependencyProperty.Register("Minutes", typeof(int), typeof(HibernateCore),
                new FrameworkPropertyMetadata(30, FrameworkPropertyMetadataOptions.AffectsRender, MinutesChangedCallback));
            IsDecreasableProperty = DependencyProperty.Register("IsDecreasable", typeof(bool), typeof(HibernateCore),
                new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.AffectsRender, IsDecreasableChangedCallback));
        }

        public HibernateCore()
        {
            _countTimer = new DispatcherTimer();
            _countTimer.Tick += On_Tick;
            _countTimer.Interval = new TimeSpan(0, 1, 0);
            _countTimer.Start();
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
            get => (int)GetValue(HoursProperty);
        }

        public int Minutes
        {
            set
            {
                SetValue(MinutesProperty, value);
                NotifyPropertyChanged("Minutes");
            }
            get => (int)GetValue(MinutesProperty);
        }

        public bool IsDecreasable
        {
            set
            {
                SetValue(IsDecreasableProperty, value);
                NotifyPropertyChanged("IsDecreasable");
            }
            get => (bool)GetValue(IsDecreasableProperty);
        }

        private void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }

        private static void HoursChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            HibernateCore obj = (HibernateCore)dependencyObject;
            obj.SetIsDecreasable();
        }

        private static void MinutesChangedCallback(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs args)
        {
            HibernateCore obj = (HibernateCore)dependencyObject;
            if (obj.Minutes >= 60)
            {
                obj.Hours += obj.Minutes / 60;
                obj.Minutes = obj.Minutes % 60;
            }

            if (obj.Minutes < 0)
            {
                if (obj.Hours > 0)
                {
                    obj.Hours--;
                    obj.Minutes += 60;
                }
            }

            obj.SetIsDecreasable();

            if (obj.Minutes == 0 && obj.Hours == 0)
            {
                obj._countTimer.Stop();
                if (options == ShutdownOptions.Shutdown)
                    ShutDown();
                else
                    Hibernate();
            }
        }

        private static void IsDecreasableChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs args)
        {
            HibernateCore obj = (HibernateCore)dependencyObject;
        }

        public static void ShutDown()
        {
            ManagementBaseObject outParameters = null;
            ManagementClass sysOS = new ManagementClass("Win32_OperatingSystem");
            sysOS.Get();
            // enables required security privilege.
            sysOS.Scope.Options.EnablePrivileges = true;
            // get our in parameters
            ManagementBaseObject inParameters = sysOS.GetMethodParameters("Win32Shutdown");
            // pass the flag of 0 = System Shutdown
            // Flag 1 means we want to shut down the system. Use "2" to reboot.

            inParameters["Flags"] = "2";
            inParameters["Reserved"] = "0";
            foreach (ManagementObject manObj in sysOS.GetInstances())
            {
                outParameters = manObj.InvokeMethod("Win32Shutdown", inParameters, null);
            }
        }
        public static void Hibernate()
        {
            System.Windows.Forms.Application.SetSuspendState(PowerState.Hibernate, false, false);
        }

        public void InvertShutdownStatus()
        {
            options = (options == ShutdownOptions.Shutdown) ? ShutdownOptions.Hibernate : ShutdownOptions.Shutdown;
        }
        private void SetIsDecreasable()
        {
            IsDecreasable = !(Hours == 0 && Minutes < 30);
        }
    }
}
