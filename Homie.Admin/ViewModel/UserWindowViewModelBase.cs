using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using MVVMLib;
using MVVMLib.ViewModel;
using MVVMLib.Dialog.Service;

using Homie.Common.Interfaces;
using Homie.Model;

namespace Homie.Admin.ViewModel
{
    public abstract class UserWindowViewModelBase : DialogViewModelBase
    {
        private readonly IDialogService dialogService;        
        private readonly UserDetailsViewModel userDetailsViewModel;

        protected readonly IUserControlService UserControlService;

        public IViewModel UserDetailsViewModel
        {
            get
            {
                return userDetailsViewModel;
            }
        }

        protected UserWindowViewModelBase(IUserControlService userControlService, User user)
        {
            dialogService = ServiceLocator.Resolve<IDialogService>();
            this.UserControlService = userControlService;
            userDetailsViewModel = new UserDetailsViewModel(user);
        }

        private RelayCommand saveCommand;

        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    return saveCommand = new RelayCommand(o => SaveUserInternal(userDetailsViewModel.User));
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

        protected abstract Task SaveUser(User user);

        private async void SaveUserInternal(User user)
        {
            try
            {
                await SaveUser(user);
                this.DialogResult = true;
            }
            catch (Exception exception)
            {
                dialogService.ShowMessageBox(this, exception.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
