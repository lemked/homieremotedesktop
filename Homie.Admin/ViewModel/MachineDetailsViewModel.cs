using Homie.Model;
using MVVMLib.ViewModel;

namespace Homie.Admin.ViewModel
{
    public class MachineDetailsViewModel : ViewModelBase
    {
        public Machine Machine { get; set; }

        public MachineDetailsViewModel()
        {

        }

        public MachineDetailsViewModel(Machine machine)
        {
            this.Machine = machine;
        }
    }
}
