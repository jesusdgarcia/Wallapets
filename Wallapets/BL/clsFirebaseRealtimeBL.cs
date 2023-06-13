using DAL;
using ENTITIES;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class clsFirebaseRealtimeBL
    {
        /// <summary>
        /// Metodo que crea o actualiza un animal pasado por parametro
        /// Devuelve un bool para definir que la trasaccion se ha efectuado correctamente
        /// </summary>
        /// <param name="pet"></param>
        /// <returns></returns>
        public static async Task<bool> UploadPetBL(clsMascota pet)
        {
            return await clsFirebaseRealtime.UploadPetAsync(pet);
        }

        /// <summary>
        /// Metodo que obtiene todas las mascotas de la dashboard que no contengan
        /// el uid de subida del logueado a la app
        /// (En construcción scroll infinito) apartir del key (id del objeto mascota en la bbdd)
        /// se obtienen los 16 siguientes animales (por si la base tiene 100 elementos que no 
        /// coja los 100 del tirón)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static async Task<List<clsMascota>> GetPetsBL(string key, string uid)
        {
            return await clsFirebaseRealtime.GetPets(key, uid);
        }

        /// <summary>
        /// Metodo que borra un animal segun su clave
        /// Devuelve un bool para definir que la trasaccion se ha efectuado correctamente
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<bool> DeletePetBL(string key)
        {
            return await clsFirebaseRealtime.DeletePet(key);
        }

        /// <summary>
        /// Metodo que obtiene todas las mascotas cuyo tipo y provincias sean los mismo que los pasado por parametros
        /// y el uid no
        /// </summary>
        /// <param name="provincia"></param>
        /// <param name="tipo"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static async Task<List<clsMascota>> GetPetsProvinciaTipoBL(string provincia, string tipo, string uid)
        {
            return await clsFirebaseRealtime.GetPetsProvinciaTipo(provincia, tipo, uid);
        }

        /// <summary>
        /// Metodo que obtiene todas las mascotas cuya provincia sean la misma que la pasada por parametros
        /// y el uid no
        /// </summary>
        /// <param name="provincia"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static async Task<List<clsMascota>> GetPetsProvinciaBL(string provincia, string uid)
        {
            return await clsFirebaseRealtime.GetPetsProvincia(provincia, uid);
        }

        /// <summary>
        /// Metodo que obtiene todas las mascotas cuyo tipo sean el mismo que el pasado por parametros
        /// y el uid no
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static async Task<List<clsMascota>> GetPetsTipoBL(string tipo, string uid)
        {
            return await clsFirebaseRealtime.GetPetsTipo(tipo, uid);
        }

        /// <summary>
        /// Metodo que obtiene todas las provincias de la base de datos
        /// </summary>
        /// <returns></returns>
        public static async Task<List<clsProvincia>> GetProvinciaBL()
        {
            return await clsFirebaseRealtime.GetProvincias();
        }

        /// <summary>
        /// Metodo que obtiene todas las expecies de la base de datos
        /// </summary>
        /// <returns></returns>
        public static async Task<List<clsTipo>> GetTipoBL()
        {
            return await clsFirebaseRealtime.GetTipos();
        }

        /// <summary>
        /// Metodo que obtiene todas las mascotas subidas por el logueado en la app
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static async Task<List<clsMascota>> GetUserPetsBL(string uid)
        {
            return await clsFirebaseRealtime.GetUserPets(uid);
        }

        /// <summary>
        /// Metodo que crea un chat pasado por parametros,
        /// comprueba si el chat no ha sido creado anteriormente
        /// </summary>
        /// <param name="chat"></param>
        public static void IniciarChat(clsChat chat)
        {
            clsFirebaseRealtime.IniciarChat(chat);
        }

        /// <summary>
        /// Metodo que obtiene todos los chat que dicho usuario tenga
        /// Se le pasa su identificacion por parametros
        /// Devuelve una lista de chats
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static async Task<List<clsChat>> GetChatUserBL(string uid)
        {
            return await clsFirebaseRealtime.GetChatUser(uid);
        }

        /// <summary>
        /// Metodo que obtiene los mensajes segun la clave de del chat y el ultimo mensaje enviado
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<List<clsMensaje>> GetMessagesBL(string uid, string ultimoMensaje)
        {
            return await clsFirebaseRealtime.GetMessages(uid, ultimoMensaje);
        }

        /// <summary>
        /// Metodo que envia un mensaje, se indica el key segun el identificador del chat y se le pasa por parametros
        /// el objeto chat
        /// </summary>
        /// <param name="key"></param>
        /// <param name="mensaje"></param>
        /// <returns></returns>
        public static async Task<bool> SendMessageBL(string key, clsMensaje mensaje)
        {
            return await clsFirebaseRealtime.SendMessage(key, mensaje);
        }
    }
}
