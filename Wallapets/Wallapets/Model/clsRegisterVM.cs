using BL;
using ENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallapets.Model.Utilities;

namespace Wallapets.Model
{
    public class clsRegisterVM : clsBase
    {
        #region Atributos
        private string name;
        private string email;
        private string password;
        private DelegateCommand registerCommand;
        private DelegateCommand backCommand;
        #endregion

        #region Propiedades
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                email = value;
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

        public DelegateCommand RegisterCommand
        {
            get { return registerCommand; }
        }

        public DelegateCommand BackCommand
        {
            get { return backCommand; }
        }
        #endregion

        #region Constructores
        public clsRegisterVM()
        {
            registerCommand = new DelegateCommand(RegisterCommand_execute);
            backCommand = new DelegateCommand(BackCommand_execute);
        }
        #endregion

        #region Metodos
        /// <summary>
        /// Metodo que registra el usuario en la bbdd
        /// </summary>
        private async void RegisterCommand_execute()
        {
            clsUser data = new clsUser(name, email, password);
            try
            {
                var userCredential = clsFirebaseBL.RegisterBL(data);
                if (userCredential != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Alerta", "Usuario registrado correctamente", "OK");
                    await Shell.Current.GoToAsync(state: "//Login");
                }
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Se ha producido un error, vuelve a intentarlo", "Ok");
                throw;
            }
        }

        /// <summary>
        /// Metodo que vuelve hacia atras
        /// </summary>
        private async void BackCommand_execute()
        {
            await Shell.Current.GoToAsync("//Login");
        }
        #endregion
    }
}
