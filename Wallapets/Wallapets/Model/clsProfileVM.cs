using BL;
using ENTITIES;
using System.Collections.ObjectModel;
using Wallapets.Model.Utilities;

namespace Wallapets.Model
{
    public class clsProfileVM : clsBase
    {
        #region Atributos
        private DelegateCommand logoutCommand;
        private ObservableCollection<clsMascota> listadoMascotasUser;
        private clsMascota mascotaSeleccionada;
        private string nombre;
        private string email;
        private string urlImagenProfile;
        #endregion

        #region Propiedades
        public DelegateCommand LogoutCommand
        {
            get { return logoutCommand; }
            set { logoutCommand = value; }
        }
        
        public ObservableCollection<clsMascota> ListadoMascotasUser
        {
            get => listadoMascotasUser;
            set
            {
                listadoMascotasUser = value;
                NotifyPropertyChanged(nameof(ListadoMascotasUser));
            }
        }

        public string Email
        {
            get => email;
        }

        public string Nombre
        {
            get => nombre;
        }

        public string UrlImagenProfile
        {
            get => urlImagenProfile;
            set
            {
                urlImagenProfile = value;
                NotifyPropertyChanged(nameof(UrlImagenProfile));
            }
        }

        public clsMascota MascotaSeleccionada
        {
            get { return mascotaSeleccionada; }
            set
            {
                if (mascotaSeleccionada != value)
                {
                    mascotaSeleccionada = value;
                    DetalleMascota();
                }
            }
        }

        #endregion

        public clsProfileVM()
        {
            logoutCommand = new DelegateCommand(logoutCommand_execute);
            urlImagenProfile = "userprofile.png";
            cargarMascotas();
            cargarUsuario();
            NotifyPropertyChanged(nameof(UrlImagenProfile));
        }

        #region Metodo
        /// <summary>
        /// Metodo que envia a una vista de edicion de dicha mascota
        /// </summary>
        private async void DetalleMascota()
        {
            var navParameter = new Dictionary<string, object>
            {
                { "mascota", mascotaSeleccionada}
            };
            await Shell.Current.GoToAsync("editpage", navParameter);
        }

        /// <summary>
        /// Metodo que cierra la sesion y envia a la pagina de inicio
        /// </summary>
        private static async void logoutCommand_execute()
        {
            clsFirebaseBL.LogoutBL();
            Preferences.Clear();
            await Shell.Current.GoToAsync(state: "//Login");
        }

        /// <summary>
        /// Metodo que carga las mascotas segun el uid del usuario
        /// </summary>
        public async void cargarMascotas()
        {
            try
            {
                listadoMascotasUser = new ObservableCollection<clsMascota>(await clsFirebaseRealtimeBL.GetUserPetsBL(Preferences.Get("UserToken", "")));
                NotifyPropertyChanged(nameof(ListadoMascotasUser));
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Error al cargar las mascotas", "Recargar");
                cargarMascotas();
            }
        }

        /// <summary>
        /// Metodo que carga los datos del usuario
        /// </summary>
        public async void cargarUsuario()
        {
            clsUser data = new clsUser(Preferences.Get("User", ""), Preferences.Get("Password", ""));
            try
            {
                var userCredential = clsFirebaseBL.LoginBL(data);
                nombre = userCredential.Result.User.Info.DisplayName;
                email = userCredential.Result.User.Info.Email;
                NotifyPropertyChanged(nameof(Nombre));
                NotifyPropertyChanged(nameof(Email));
            }
            catch(Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Error al cargar los datos del usuario", "Recargar");
                cargarMascotas();
            }
        }
        #endregion
    }
}
