using Homie.Model;
using MVVMLib.ViewModel;

namespace Homie.Admin.ViewModel
{
    public class UserDetailsViewModel : ViewModelBase
    {
        public User User { get; set; }

        public UserDetailsViewModel()
        {

        }

        public UserDetailsViewModel(User user)
        {
            this.User = user;
        }
    }
}
