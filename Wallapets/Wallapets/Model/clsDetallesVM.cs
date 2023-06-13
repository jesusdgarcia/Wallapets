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
    [QueryProperty(nameof(Mascota), "mascota")]
    public class clsDetallesVM : clsBase
    {
        #region Atributos
        private clsMascota mascota;
        private DelegateCommand enviarMensajeCommand;
        #endregion

        #region Propiedades
        public clsMascota Mascota
        {
            get { return mascota; }
            set
            {
                mascota = value;
                NotifyPropertyChanged(nameof(Mascota));
            }
        }

        public DelegateCommand EnviarMensajeCommand
        {
            get { return enviarMensajeCommand; }
        }
        #endregion

        public clsDetallesVM()
        {
            mascota = new clsMascota();
            enviarMensajeCommand = new DelegateCommand(EnviarMensajeCommand_execute);
        }

        #region Metodos
        /// <summary>
        /// Metodo que crea un chat o inicia un chat para dicha mascota
        /// </summary>
        private async void EnviarMensajeCommand_execute()
        {
            try
            {
                clsChat chat = new clsChat(mascota.Titulo, mascota.UidUser + Preferences.Get("UserToken", ""),mascota.UrlImagen);
                clsFirebaseRealtimeBL.IniciarChat(chat);
            } catch (Exception) 
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Imagen no se ha podido subir correctamente", "OK");
            }
            await Shell.Current.GoToAsync(state: "//Chat");
        }
        #endregion
    }
}
