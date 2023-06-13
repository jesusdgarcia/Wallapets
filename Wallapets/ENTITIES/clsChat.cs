using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES
{
    public class clsChat
    {
        #region Propiedades
        private string key;
        private string titulo;
        private string uidParticipantes;
        private string urlImagenMascota;
        #endregion

        #region Atributos
        public string Key
        {
            get { return key; }
            set { key = value; }
        }
        public string Titulo
        {
            get { return titulo; }
            set { titulo = value; }
        }
       
        public string UidParticipantes
        {
            get { return uidParticipantes; }
            set { uidParticipantes = value; }
        }

        public string UrlImagenMascota
        {
            get { return urlImagenMascota; }
            set {  urlImagenMascota = value; }
        }

        #endregion

        #region Constructores
        public clsChat()
        {

        }

        public clsChat(string titulo, string uidParticipantes, string urlImagenMascota)
        {
            Titulo = titulo;
            UidParticipantes = uidParticipantes;
            UrlImagenMascota = urlImagenMascota;
        }

        public clsChat(string titulo, string uidParticipantes, string urlImagenMascota, string key)
        {
            Titulo = titulo;
            UidParticipantes = uidParticipantes;
            UrlImagenMascota = urlImagenMascota;
            Key = key;
        }
        #endregion
    }
}
