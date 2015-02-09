using System.Threading.Tasks;

using Homie.Common.Interfaces;
using Homie.Model;

namespace Homie.Admin.ViewModel
{
    public class EditUserWindowViewModel : UserWindowViewModelBase
    {
        public EditUserWindowViewModel(IUserControlService userControlService, User user) : base(userControlService, user)
        {

        }

        protected override async Task SaveUser(User user)
        {
            await UserControlService.UpdateUserAsync(user);
        }
    }
}
