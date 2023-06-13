using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENTITIES
{
    public class clsUser
    {
        #region Propiedades
        private string name;
        private string email;
        private string password;
        #endregion

        #region Atributos
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        #endregion

        #region Constructores
        public clsUser() { }
        public clsUser(string email, string password)
        {
            this.email = email;
            this.password = password;
        }

        public clsUser(string name, string email, string password)
        {
            this.name = name;
            this.email = email;
            this.password = password;
        }
        #endregion
    }
}
