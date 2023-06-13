using DAL;
using ENTITIES;
using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class clsFirebaseBL
    {
        /// <summary>
        /// Metodo que inicia sesion de un usuario pasado por parametros
        /// Devuelve el credencial de usuario iniciado correctamente o no
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public static Task<UserCredential> LoginBL(clsUser usuario) {
            return clsFirebase.Login(usuario);
        }

        /// <summary>
        /// Metodo que registra un usuario pasado por parametros
        /// Devuelve el credencial de usuario creado correctamente o no
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public static Task<UserCredential> RegisterBL(clsUser usuario)
        {
            return clsFirebase.Register(usuario);
        }

        /// <summary>
        /// Metodo que te finaliza la conexion del usuario
        /// </summary>
        public static void LogoutBL()
        {
            clsFirebase.Logout();
        }
    }
}
