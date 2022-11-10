using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFMotorTax.Models;
using WPFMotorTax.Repositories;

namespace WPFMotorTax.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        //Fields
        private string _regNo;
        private SecureString _PIN;
        private string _errorMessage;
        private bool _isViewVisible = true;

        private IUserRepository userRepository;
        Regex pattern = new Regex("^[0-9]{2,3}[A-Z]{2}[0-9]{1,5}$");

        //Properties
        public string RegNo
        {
            get
            {
                return _regNo;
            }

            set
            {
                _regNo = value;
                OnPropertyChanged(nameof(RegNo));
            }
        }

        public SecureString PIN
        {
            get
            {
                return _PIN;
            }

            set
            {
                _PIN = value;
                OnPropertyChanged(nameof(PIN));
            }
        }

        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }

            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public bool IsViewVisible
        {
            get
            {
                return _isViewVisible;
            }

            set
            {
                _isViewVisible = value;
                OnPropertyChanged(nameof(IsViewVisible));
            }
        }

        //-> Commands
        public ICommand LoginCommand { get; }
        public ICommand RecoverPasswordCommand { get; }

        //Constructor
        public LoginViewModel()
        {
            userRepository = new UserRepository();
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
            RecoverPasswordCommand = new ViewModelCommand(p => ExecuteRecoverPassCommand("", ""));
        }

        private bool CanExecuteLoginCommand(object obj)
        {

            bool validData;
            if (string.IsNullOrWhiteSpace(RegNo) || pattern.IsMatch(RegNo) ||
                PIN == null || PIN.Length != 6)
                validData = false;
            else
                validData = true;
            return validData;
        }

        private void ExecuteLoginCommand(object obj)
        {
            var isValidUser = userRepository.AuthenticateUser(new NetworkCredential(RegNo, PIN));
            if (isValidUser)
            {
                Thread.CurrentPrincipal = new GenericPrincipal(
                    new GenericIdentity(RegNo), null);
                IsViewVisible = false;
            }
            else
            {
                ErrorMessage = "* Invalid Reg number or PIN";
            }
        }

        private void ExecuteRecoverPassCommand(string username, string email)
        {
            throw new NotImplementedException();
        }
    }
}
