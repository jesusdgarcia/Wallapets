using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES
{
    public class clsTipo
    {
        #region Propiedades
        private string name;
        #endregion

        #region Atributos
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        #endregion

        #region Constructores
        public clsTipo() { }

        public clsTipo(string name)
        {
            this.name = name;
        }
        #endregion
    }
}
