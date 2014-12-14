using System.Threading.Tasks;

using Homie.Common.Interfaces;
using Homie.Model;

namespace Homie.Admin.ViewModel
{
    public class EditMachineWindowViewModel : MachineWindowViewModelBase
    {
        public EditMachineWindowViewModel(IMachineControlService machineControlService, Machine machine) : base(machineControlService, machine)
        {

        }

        protected override async Task SaveMachine(Machine machine)
        {
            await MachineControlService.UpdateMachineAsync(machine);
        }
    }
}
