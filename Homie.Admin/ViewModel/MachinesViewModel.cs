using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

using Homie.Common.Interfaces;
using Homie.Model;

using MVVMLib.ViewModel;
using MVVMLib.Dialog.Service;

namespace Homie.Admin.ViewModel
{
    public class MachinesViewModel : ViewModelBase
    {
        private readonly IDialogService dialogService;
        private readonly IMachineControlService machineControlService;

        private readonly ObservableCollection<Machine> machines = new ObservableCollection<Machine>();
       

        public ObservableCollection<Machine> Machines
        {
            get { return machines; }
        }

        private Machine selectedMachine;

        public Machine SelectedMachine
        {
            get
            {
                return selectedMachine;
            }
            set
            {
                selectedMachine = value;
                base.OnPropertyChanged();
            }
        }

        #region Commands

        public ICommand LoadMachinesCommand
        {
            get
            {
                return new RelayCommand(action => GetMachinesAsync());
            }
        }

        private RelayCommand addMachineCommand;

        public ICommand AddMachineCommand
        {
            get
            {
                if (addMachineCommand == null)
                {
                    addMachineCommand = new RelayCommand(o => ShowAddMachineDialog());
                }
                return addMachineCommand;
            }
        }

        private RelayCommand editMachineCommand;

        public ICommand EditMachineCommand
        {
            get
            {
                if (editMachineCommand == null)
                {
                    editMachineCommand = new RelayCommand(o => ShowEditMachineDialog(), o => selectedMachine != null);
                }
                return editMachineCommand;
            }
        }

        private RelayCommand removeMachineCommand;

        public ICommand RemoveMachineCommand
        {
            get
            {
                if (removeMachineCommand == null)
                {
                    removeMachineCommand = new RelayCommand(o => RemoveMachine(), o => selectedMachine != null);
                }
                return removeMachineCommand;
            }
        }

        #endregion

        public MachinesViewModel(IDialogService dialogService, IMachineControlService machineControlService)
        {
            this.dialogService = dialogService;
            this.machineControlService = machineControlService;
        }

        public async Task GetMachinesAsync()
        {
            this.machines.Clear();

            foreach (var machine in await machineControlService.GetMachinesAsync())
            {
                this.machines.Add(machine);
            }
        }

        private async void ShowAddMachineDialog()
        {
            // Show dialog.
            var addMachineWindowViewModel = new AddMachineWindowViewModel(machineControlService, new Machine());
            dialogService.ShowDialog(this, addMachineWindowViewModel);

            // Refresh list.
            await this.GetMachinesAsync();        
        }

        private void ShowEditMachineDialog()
        {
            // Show dialog.
            var editMachineWindowViewModel = new EditMachineWindowViewModel(machineControlService, selectedMachine);
            dialogService.ShowDialog(this, editMachineWindowViewModel);
        }

        private async void RemoveMachine()
        {
            // Remove machine.
            await machineControlService.RemoveMachineAsync(selectedMachine.MachineID);

            // Refresh list.
            await this.GetMachinesAsync();     
        }
    }
}
