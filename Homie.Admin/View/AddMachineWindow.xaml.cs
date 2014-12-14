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

//        public AddMachineWindow(Machine pMachine) : this()
//        {
//            DialogMode = DialogMode.Edit;
//
//            m_CurrentMachine = ServiceCall.GetMachineByID(pMachine.MachineID);
//
//            if (m_CurrentMachine == null)
//            {
//                // If machine wasn't found, do nothing else. This should not happen
//                // in any case because the we query for a machine that was already
//                // found and listed when the application started.
//                return;
//            }
//
//            MachineNameTextBox.Text = m_CurrentMachine.NameOrAddress;
//            MacAddressTextBox.Text = m_CurrentMachine.MacAddress;
//            PortTextBox.Text = m_CurrentMachine.Port.ToString(CultureInfo.InvariantCulture);
//
//            OkButton.Content = Common.Properties.Resources.Save;
//        }
//
//        private void OkButtonClick(object pSender, RoutedEventArgs pRoutedEventArgs)
//        {
//            var lDialogMessage = String.Empty;
//
//            if (DialogMode.Equals(DialogMode.Edit))
//            {
//                try
//                {
//                    // When in edit mode, remove the existing machine before adding the new one.
//                    if (!ServiceCall.RemoveMachine(m_CurrentMachine)) return;
//                    lDialogMessage = Common.Properties.Resources.MachineAppliedChanges;
//                }
//                catch (CommunicationException lCommunicationException)
//                {
//                    MessageBox.Show(lCommunicationException.Message);
//                }
//            }
//            else
//            {
//                lDialogMessage = Common.Properties.Resources.MachineAdded;
//            }
//
//            // Prepare the new machine.
//            Machine lMachine = new Machine(MachineNameTextBox.Text, MacAddressTextBox.Text);
//            if (!String.IsNullOrEmpty(PortTextBox.Text))
//            {
//                lMachine.Port = int.Parse(PortTextBox.Text);
//            }
//
//            try 
//	        {	        
//                // Add the new machine
//		        ServiceCall.AddMachine(lMachine);
//	            MessageBox.Show
//	            (
//	                String.Format(lDialogMessage, lMachine.NameOrAddress), Common.Properties.Resources.Info,
//	                MessageBoxButton.OK, MessageBoxImage.Information
//	            );
//	        }
//            catch (CommunicationException lCommunicationException)
//            {
//                MessageBox.Show(lCommunicationException.Message);
//            }
//
//            this.Close();
//        }
//
//        private void CancelButtonClick(object pSender, RoutedEventArgs pRoutedEventArgs)
//        {
//            this.Close();
//        }
    }
}
