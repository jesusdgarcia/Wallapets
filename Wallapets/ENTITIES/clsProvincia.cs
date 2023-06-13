using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES
{
    public class clsProvincia
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
        public clsProvincia() { }

        public clsProvincia(string name)
        {
            this.name = name;
        }
        #endregion
    }
}
