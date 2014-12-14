using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using MVVMLib;
using MVVMLib.Service;
using MVVMLib.ViewModel;

using Homie.Common.Interfaces;
using Homie.Model;

namespace Homie.Admin.ViewModel
{
    public abstract class MachineWindowViewModelBase : DialogViewModelBase
    {
        private readonly IDialogService dialogService;        
        private readonly MachineDetailsViewModel machineDetailsViewModel;

        protected readonly IMachineControlService MachineControlService;

        public IViewModel MachineDetailsViewModel
        {
            get
            {
                return machineDetailsViewModel;
            }
        }

        protected MachineWindowViewModelBase(IMachineControlService machineControlService, Machine machine)
        {
            dialogService = ServiceLocator.Resolve<IDialogService>();
            this.MachineControlService = machineControlService;
            machineDetailsViewModel = new MachineDetailsViewModel(machine);
        }

        private RelayCommand saveCommand;

        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    return saveCommand = new RelayCommand(o => SaveMachineInternal(machineDetailsViewModel.Machine));
                }
                return saveCommand;
            }
        }
        
        public RelayCommand CancelCommand
        {
            get
            {
                return new RelayCommand(o => this.DialogResult = true);
            }
        }

        protected abstract Task SaveMachine(Machine machine);

        private async Task SaveMachineInternal(Machine machine)
        {
            try
            {
                await SaveMachine(machine);
                this.DialogResult = true;
            }
            catch (Exception exception)
            {
                dialogService.ShowMessageBox(this, exception.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
