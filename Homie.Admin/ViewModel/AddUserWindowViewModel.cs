using System.Threading.Tasks;

using Homie.Common.Interfaces;
using Homie.Model;

namespace Homie.Admin.ViewModel
{
    public class AddUserWindowViewModel : UserWindowViewModelBase
    {
        public AddUserWindowViewModel(IUserControlService userControlService, User user)
            : base(userControlService, user)
        {

        }

        protected override async Task SaveUser(User user)
        {
            await UserControlService.AddUserAsync(user);
        }
    }
}
