using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES
{
    public class clsMascota
    {
        #region Propiedades
        private string key;
        private string titulo;
        private string descripcion;
        private string provincia;
        private string localidad;
        private string tipo;
        private string urlImagen;
        private string uidUser;
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
        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }
        public string Tipo
        {
            get
            {
                return tipo;
            }
            set
            { tipo = value; }
        }
        public string UrlImagen
        {
            get {
                return urlImagen;
            }
            set { urlImagen = value; }
        }
        public string UidUser
        {
            get { return uidUser; }
            set { uidUser = value; }
        }

        public string Localidad 
        {
            get { return localidad; }
            set { localidad = value; }
        }

        public string Provincia
        {
            get { return provincia; }
            set { provincia = value; }
        }

        #endregion

        #region Constructores
        public clsMascota()
        {

        }

        public clsMascota(string titulo, string descripcion, string provincia, string localidad, string tipo, string urlImagen, string uidUser)
        {
            Titulo = titulo;
            Descripcion = descripcion;
            Provincia = provincia;
            Localidad = localidad;
            Tipo = tipo;
            UrlImagen = urlImagen;
            UidUser = uidUser;
        }
        #endregion
    }
}
