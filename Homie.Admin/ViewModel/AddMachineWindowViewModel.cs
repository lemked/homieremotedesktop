using System.Threading.Tasks;

using Homie.Common.Interfaces;
using Homie.Model;

namespace Homie.Admin.ViewModel
{
    public class AddMachineWindowViewModel : MachineWindowViewModelBase
    {
        public AddMachineWindowViewModel(IMachineControlService machineControlService, Machine machine) : base(machineControlService, machine)
        {

        }

        protected override async Task SaveMachine(Machine machine)
        {
            await MachineControlService.AddMachineAsync(machine);
        }
    }
}
