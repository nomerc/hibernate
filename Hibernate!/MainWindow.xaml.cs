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

namespace Hibernate_
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly HibernateCore _core;

        public MainWindow()
        {
            InitializeComponent();
            _core = new HibernateCore
            {
                Minutes = 30
            };

            DataContext = _core;
        }

        private void Inc_Time_Btn_Click(object sender, RoutedEventArgs e)
        {
            _core.Minutes += 30;
        }

        private void Dec_Time_Btn_Click(object sender, RoutedEventArgs e)
        {
            _core.Minutes -= 30;
        }

        private void On_Off_Btn_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void On_Off_Btn_Unchecked(object sender, RoutedEventArgs e)
        {

        }
    }
}
