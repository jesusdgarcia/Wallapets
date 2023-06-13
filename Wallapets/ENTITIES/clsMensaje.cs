using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES
{
    public class clsMensaje
    {
        #region Atributos
        private string texto;
        private DateTime hora;
        private string uid;
        private string key;
        #endregion

        #region Propiedades
        public string Texto
        {
            get { return texto; }
            set { texto = value; }
        }
        public DateTime Hora 
        { 
            get { return hora; } 
            set { hora = value; } 
        }
        public string Uid 
        { 
            get { return uid; } 
            set { uid = value; } 
        }
        public string Key
        {
            get { return key; }
            set { key = value; }
        }
        #endregion

        #region Constructores
        public clsMensaje() { }

        public clsMensaje(string texto, DateTime hora, string uid)
        {
            Texto = texto;
            Hora = hora;
            Uid = uid;
        }
        #endregion
    }
}
