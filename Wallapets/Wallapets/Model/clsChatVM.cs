using BL;
using ENTITIES;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wallapets.Model.Utilities;

namespace Wallapets.Model
{
    public class clsChatVM : clsBase
    {
        #region Atributos
        private ObservableCollection<clsChat> listadoChats;
        private clsChat chatSeleccionado;
        #endregion

        #region Propiedades
        public ObservableCollection<clsChat> ListadoChats
        {
            get { return listadoChats; }
            set
            {
                listadoChats = value;
                NotifyPropertyChanged(nameof(ListadoChats));
            }
        }

        public clsChat ChatSeleccionado
        {
            get { return chatSeleccionado; }
            set
            {
                if (chatSeleccionado != value)
                {
                    chatSeleccionado = value;
                    AbrirMensajes();
                }
                
            }
        }
        #endregion

        public clsChatVM()
        {
            CargarChats();
        }

        #region Metodos
        /// <summary>
        /// Metodo que llama a la base de datos y recoje los chat que tenga dicho usuario
        /// </summary>
        public async void CargarChats()
        {
            try
            {
                listadoChats = new ObservableCollection<clsChat>(await clsFirebaseRealtimeBL.GetChatUserBL(Preferences.Get("UserToken", "")));
                NotifyPropertyChanged(nameof(ListadoChats));
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Error al cargar los chats", "Recargar");
                CargarChats();
            }
        }

        /// <summary>
        /// Metodo que abre el chat para mostrar los mensajes que tiene
        /// </summary>
        private async void AbrirMensajes()
        {
            var navParameter = new Dictionary<string, object>
            {
                { "chat", chatSeleccionado}
            };
            await Shell.Current.GoToAsync("mensajespage", navParameter);
        }
        #endregion
    }
}
