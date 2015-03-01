using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

using Homie.Common.Interfaces;
using Homie.Model;

using MVVMLib.ViewModel;
using MVVMLib.Dialog.Service;

namespace Homie.Admin.ViewModel
{
    public class UsersViewModel : ViewModelBase
    {
        private readonly IDialogService dialogService;
        private readonly IUserControlService userControlService;

        private readonly ObservableCollection<User> users = new ObservableCollection<User>();
       

        public ObservableCollection<User> Users
        {
            get { return users; }
        }

        private User selectedUser;

        public User SelectedUser
        {
            get
            {
                return selectedUser;
            }
            set
            {
                selectedUser = value;
                base.OnPropertyChanged();
            }
        }

        #region Commands

        public ICommand LoadUsersCommand
        {
            get
            {
                return new RelayCommand(action => GetUsersAsync());
            }
        }

        private RelayCommand addUserCommand;

        public ICommand AddUserCommand
        {
            get
            {
                if (addUserCommand == null)
                {
                    addUserCommand = new RelayCommand(o => ShowAddUserDialog());
                }
                return addUserCommand;
            }
        }

        private RelayCommand editUserCommand;

        public ICommand EditUserCommand
        {
            get
            {
                if (editUserCommand == null)
                {
                    editUserCommand = new RelayCommand(o => ShowEditUserDialog(), o => selectedUser != null);
                }
                return editUserCommand;
            }
        }

        private RelayCommand removeUserCommand;

        public ICommand RemoveUserCommand
        {
            get
            {
                if (removeUserCommand == null)
                {
                    removeUserCommand = new RelayCommand(o => RemoveUser(), o => selectedUser != null);
                }
                return removeUserCommand;
            }
        }

        #endregion

        public UsersViewModel(IDialogService dialogService, IUserControlService userControlService)
        {
            this.dialogService = dialogService;
            this.userControlService = userControlService;
        }

        public async Task GetUsersAsync()
        {
            this.users.Clear();

            foreach (var user in await userControlService.GetUsersAsync())
            {
                this.users.Add(user);
            }
        }

        private async void ShowAddUserDialog()
        {
            // Show dialog.
            var addUserWindowViewModel = new AddUserWindowViewModel(userControlService, new User());
            dialogService.ShowDialog(this, addUserWindowViewModel);

            // Refresh list.
            await this.GetUsersAsync();        
        }

        private void ShowEditUserDialog()
        {
            // Show dialog.
            var editUserWindowViewModel = new EditUserWindowViewModel(userControlService, selectedUser);
            dialogService.ShowDialog(this, editUserWindowViewModel);
        }

        private async void RemoveUser()
        {
            // Remove user.
            await userControlService.RemoveUserAsync(selectedUser.ID);

            // Refresh list.
            await this.GetUsersAsync();     
        }
    }
}
