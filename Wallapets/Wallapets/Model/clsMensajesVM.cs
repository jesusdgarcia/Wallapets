using BL;
using ENTITIES;
using System.Collections.Generic;
using System.Linq;
using Wallapets.Model.Utilities;

namespace Wallapets.Model
{
    [QueryProperty(nameof(Chat), "chat")]
    public class clsMensajesVM : clsBase
    {
        #region Atributos
        private List<clsMensaje> listadoMensajes;
        private List<clsMensaje> listadoMensajesServidor;
        private DelegateCommand enviarMensajeCommand;
        private clsChat chat;
        private string texto;
        private clsMensaje ultimoMensaje;
        public bool Res = false;
        #endregion

        #region Propiedades
        public List<clsMensaje> ListadoMensajes
        {
            get { return listadoMensajes; }
            set
            {
                listadoMensajes = value;
            }
        }

        public DelegateCommand EnviarMensajeCommand
        {
            get { return enviarMensajeCommand; }
        }

        public clsChat Chat
        {
            get { return chat; }
            set
            {
                chat = value;
                NotifyPropertyChanged(nameof(Chat));
            }
        }

        public string Texto
        {
            get { return texto; }
            set 
            {
                if (texto != value)
                {
                    texto = value;
                    enviarMensajeCommand.RaiseCanExecuteChanged();
                }
            }
        }
        #endregion

        public clsMensajesVM()
        {
            chat = new clsChat();
            ultimoMensaje = new clsMensaje();
            CargarMensajes();
            enviarMensajeCommand = new DelegateCommand(EnviarMensajeCommand_execute, EnviarMensajeCommand_canExecute);
        }
        /// <summary>
        /// Metodo que carga los mensajes
        /// </summary>
        public async void CargarMensajes()
        {
            try
            {
                //Comprueba que el chat no es nulo
                if (chat.Key != null)
                {
                    //Comprueba si el ultimo mensaje no es nulo
                    if (ultimoMensaje.Key != null)
                    {
                        //Obtiene los mensajes desde el ultimo mensaje
                        listadoMensajesServidor = await clsFirebaseRealtimeBL.GetMessagesBL(chat.Key, ultimoMensaje.Key);
                    }
                    else
                    {
                        //Obtiene todos los mensajes
                        listadoMensajesServidor = await clsFirebaseRealtimeBL.GetMessagesBL(chat.Key, null);
                    }
                    //Comprueba que la lista no viene vacia
                    if (listadoMensajesServidor.Count > 0 || listadoMensajesServidor == null)
                    {
                        //Comprueba que el ultimomensaje no sea nulo
                        if (ultimoMensaje.Key == null)
                        {
                            //Obtiene el ultimo mensaje de la lista
                            ultimoMensaje = listadoMensajesServidor.Last();
                            //Unen los listados
                            listadoMensajes = listadoMensajes.Union(listadoMensajesServidor).ToList();
                            //Los ordena por la hora
                            listadoMensajes = listadoMensajes.OrderBy(x => x.Hora).ToList();
                            //Res se vuelve positivo para que desde el code behind haga refresh
                            Res = true;
                            NotifyPropertyChanged(nameof(ListadoMensajes));
                        } 
                        //Comprueba que el ultimo mensaje no sea el ultimo mensaje de la lista
                        else if (ultimoMensaje.Key != listadoMensajesServidor.Last().Key)
                        {
                            //Borra el ultimo mensaje de la lista ya que obtiene desde el ultimo mensaje y hace diplicidad cuando lo muestra por pantalla
                            listadoMensajesServidor.RemoveAll(x => x.Key == ultimoMensaje.Key);
                            ultimoMensaje = listadoMensajesServidor.Last();
                            listadoMensajes = listadoMensajes.Union(listadoMensajesServidor).ToList();
                            listadoMensajes = listadoMensajes.OrderBy(x => x.Hora).ToList();
                            Res = true;
                            NotifyPropertyChanged(nameof(ListadoMensajes));
                        }
                        
                    }
                }
                else
                {
                    //Si no lo inicializa
                    listadoMensajes = new List<clsMensaje>();
                    listadoMensajesServidor = new List<clsMensaje>();
                }
                
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Error al cargar los mensajes", "Recargar");
                CargarMensajes();
            }
        }
        /// <summary>
        /// Metodo que envia un mensaje
        /// </summary>
        private async void EnviarMensajeCommand_execute()
        {
            try
            {
                clsMensaje mensaje = new clsMensaje(texto, DateTime.UtcNow, Preferences.Get("UserToken", ""));
                await clsFirebaseRealtimeBL.SendMessageBL(Chat.Key, mensaje);
                texto = "";
                NotifyPropertyChanged(nameof(Texto));
            }
            catch (Exception)
            {
                await Application.Current.MainPage.DisplayAlert("Alerta", "Error al enviar el mensaje", "OK");
            }
        }
        
        /// <summary>
        /// Metodo que habilita el boton de envio
        /// </summary>
        /// <returns></returns>
        private bool EnviarMensajeCommand_canExecute()
        {
            bool btnTexto = false;
            if (!string.IsNullOrEmpty(Texto))
            {
                btnTexto = true;
            }
            return btnTexto;
        }
    }
}
