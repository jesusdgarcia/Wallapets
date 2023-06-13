using BL;
using ENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallapets.Model.Utilities;
using Wallapets.Views;

namespace Wallapets.Model
{
    public class clsLoginVM : clsBase
    {
        #region Atributos
        private string user;
        private string password;
        private DelegateCommand loginCommand;
        private DelegateCommand registrerCommand;
        #endregion

        #region Propiedades

        public string User
        {
            get { return user; }
            set
            {
                user = value;
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                password = value;
            }
        }

        public DelegateCommand LoginCommand
        {
            get { return loginCommand; }
        }

        public DelegateCommand RegistrerCommand
        {
            get { return registrerCommand; }
        }
        #endregion

        #region Constructores
        public clsLoginVM()
        {
            loginCommand = new DelegateCommand(LoginCommand_execute);
            registrerCommand = new DelegateCommand(RegistrerCommand_execute);
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Metodo que inicia sesion en la aplicacion
        /// </summary>
        private async void LoginCommand_execute()
        {
            clsUser data = new clsUser(user,password);
            try
            {
                var userCredential = clsFirebaseBL.LoginBL(data);
                //Almacena en las preferencias que esta logueado en la app
                Preferences.Set("UserAlreadyLoggedIn", true);
                //Almacena en las preferencias el token (uid del user) del usuario logueado
                Preferences.Set("UserToken", userCredential.Result.User.Uid);
                //Almacena en las preferencias el usuario
                Preferences.Set("User", user);
                //Almacena en las preferencias el contraseña
                Preferences.Set("Password", password);
                user = "";
                password = "";
                NotifyPropertyChanged(nameof(User));
                NotifyPropertyChanged(nameof(Password));
                await Shell.Current.GoToAsync(state: "//Dashboard");
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Alerta","Se ha producido un error, vuelve a intentarlo", "Ok");
                Preferences.Clear();
            }
        }

        /// <summary>
        /// Metodo que envia a la vista de registro
        /// </summary>
        private async void RegistrerCommand_execute()
        {
            await Shell.Current.GoToAsync("registerpage");
        }
        #endregion
    }
}
