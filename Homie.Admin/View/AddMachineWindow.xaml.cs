using System;
using System.Globalization;
using System.ServiceModel;
using System.Windows;
using Homie.Model;

namespace Homie.Admin.View
{
    /// <summary>
    /// Interaction logic for AddMachineWindow.xaml
    /// </summary>
    public partial class AddMachineWindow : Window
    {
        private Machine m_CurrentMachine;

        public AddMachineWindow()
        {
            InitializeComponent();
        }
    }
}
