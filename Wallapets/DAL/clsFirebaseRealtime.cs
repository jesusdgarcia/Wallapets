using ENTITIES;
using Firebase.Database;
using Firebase.Database.Query;

namespace DAL
{
    public class clsFirebaseRealtime
    {
        private static string firebaseUrl = "https://wallapets-73a5a-default-rtdb.firebaseio.com/";

        /// <summary>
        /// Metodo que crea o actualiza un animal pasado por parametro
        /// Devuelve un bool para definir que la trasaccion se ha efectuado correctamente
        /// </summary>
        /// <param name="pet"></param>
        /// <returns></returns>
        public static async Task<bool> UploadPetAsync(clsMascota pet)
        {
            bool salida = false;
            try
            {
                FirebaseClient cliente = new FirebaseClient(
                firebaseUrl,
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(pet.UidUser)
                });
                if (!string.IsNullOrWhiteSpace(pet.Key))
                {
                    await cliente
                        .Child("Pets")
                        .Child(pet.Key)
                        .PutAsync(pet);
                    salida = true;
                }
                else
                {
                    var response = cliente
                        .Child("Pets")
                        .PostAsync(pet);
                    if (response != null)
                    {
                        salida = true;
                    }
                }
            }
            catch (Exception)
            {

            }
            return salida;
        }

        /// <summary>
        /// Metodo que borra un animal segun su clave
        /// Devuelve un bool para definir que la trasaccion se ha efectuado correctamente
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<bool> DeletePet(string key)
        {
            bool salida = false;
            try
            {
                FirebaseClient cliente = new FirebaseClient(firebaseUrl);
                await cliente.Child("Pets").Child(key).DeleteAsync();
                salida = true;
            }
            catch (Exception)
            {

            }
            return salida;
        }

        /// <summary>
        /// Metodo que crea un chat pasado por parametros
        /// Comprueba previamente si el chat que se va a crear existe
        /// </summary>
        /// <param name="chat"></param>
        public static async void IniciarChat(clsChat chat)
        {
            try
            {
                FirebaseClient cliente = new FirebaseClient(firebaseUrl);
                var chats = await cliente
                    .Child("Chats")
                    .OrderByKey()
                    .OnceAsync<clsChat>();
                if (chats.FirstOrDefault(i => i.Object.UidParticipantes.Contains(chat.UidParticipantes)) == null)
                {
                    _ = cliente
                    .Child("Chats")
                    .PostAsync(chat);
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Metodo que obtiene todos los chat que dicho usuario tenga
        /// Se le pasa su identificacion por parametros
        /// Devuelve una lista de chats
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static async Task<List<clsChat>> GetChatUser(string uid)
        {
            List<clsChat> lChats = new List<clsChat>();
            try
            {
                FirebaseClient cliente = new FirebaseClient(firebaseUrl);
                var chats = await cliente
                    .Child("Chats")
                    .OrderByKey()
                    .OnceAsync<clsChat>();
                foreach (var chat in chats)
                {
                    if (chat.Object.UidParticipantes.Contains(uid))
                    {
                        lChats.Add(
                            new clsChat(chat.Object.Titulo, chat.Object.UidParticipantes, chat.Object.UrlImagenMascota, chat.Key)
                            );
                    }
                }
            }
            catch (Exception)
            {

            }
            return lChats;
        }

        /// <summary>
        /// Metodo que obtiene todas las provincias de la base de datos
        /// </summary>
        /// <returns></returns>
        public static async Task<List<clsProvincia>> GetProvincias()
        {
            List<clsProvincia> lProvincias = new List<clsProvincia>();
            try
            {
                FirebaseClient cliente = new FirebaseClient(firebaseUrl);
                var provincias = await cliente
                    .Child("Provincias")
                    .OrderByKey()
                    .OnceAsync<clsProvincia>();
                foreach (var provincia in provincias)
                {
                    lProvincias.Add(
                        new clsProvincia(provincia.Object.Name)
                        );
                }
            }
            catch (Exception)
            {

            }
            return lProvincias;
        }

        /// <summary>
        /// Metodo que obtiene todas las expecies de la base de datos
        /// </summary>
        /// <returns></returns>
        public static async Task<List<clsTipo>> GetTipos()
        {
            List<clsTipo> lTipos = new List<clsTipo>();
            try
            {
                FirebaseClient cliente = new FirebaseClient(firebaseUrl);
                var tipos = await cliente
                    .Child("Tipos")
                    .OrderByKey()
                    .OnceAsync<clsTipo>();
                foreach (var tipo in tipos)
                {
                    lTipos.Add(
                        new clsTipo(tipo.Object.Name)
                        );
                }
            }
            catch (Exception)
            {

            }
            return lTipos;
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
        public static async Task<List<clsMascota>> GetPets(string key, string uid)
        {
            List<clsMascota> mascotas = new List<clsMascota>();
            try
            {
                FirebaseClient cliente = new FirebaseClient(firebaseUrl);
                IReadOnlyCollection<FirebaseObject<clsMascota>>? pets;
                if (key == null)
                {
                    pets = await cliente
                    .Child("Pets")
                    .OrderByKey()
                    //.LimitToFirst(16)
                    .OnceAsync<clsMascota>();
                }
                else
                {
                    pets = await cliente
                    .Child("Pets")
                    .OrderByKey()
                    .StartAt(key)
                    //.LimitToFirst(16)
                    .OnceAsync<clsMascota>(); ;
                }
                foreach (var pet in pets)
                {
                    if (pet.Object.UidUser != uid)
                    {
                        mascotas.Add(
                            new clsMascota
                            {
                                Titulo = pet.Object.Titulo,
                                Descripcion = pet.Object.Descripcion,
                                Provincia = pet.Object.Provincia,
                                Localidad = pet.Object.Localidad,
                                Tipo = pet.Object.Tipo,
                                UrlImagen = pet.Object.UrlImagen,
                                UidUser = pet.Object.UidUser,
                                Key = pet.Key,
                            });
                    }
                }
            }
            catch (Exception)
            {

            }
            return mascotas;
        }

        /// <summary>
        /// Metodo que obtiene todas las mascotas subidas por el logueado en la app
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static async Task<List<clsMascota>> GetUserPets(string uid)
        {
            List<clsMascota> mascotas = new List<clsMascota>();
            try
            {
                FirebaseClient cliente = new FirebaseClient(firebaseUrl);
                var pets = await cliente
                    .Child("Pets")
                    .OrderByKey()
                    .OnceAsync<clsMascota>();
                foreach (var pet in pets)
                {
                    if (pet.Object.UidUser == uid)
                    {
                        mascotas.Add(
                            new clsMascota
                            {
                                Titulo = pet.Object.Titulo,
                                Descripcion = pet.Object.Descripcion,
                                Provincia = pet.Object.Provincia,
                                Localidad = pet.Object.Localidad,
                                Tipo = pet.Object.Tipo,
                                UrlImagen = pet.Object.UrlImagen,
                                UidUser = pet.Object.UidUser,
                                Key = pet.Key,
                            });
                    }
                }
            }
            catch (Exception)
            {

            }
            return mascotas;
        }

        /// <summary>
        /// Metodo que obtiene todas las mascotas cuyo tipo y provincias sean los mismo que los pasado por parametros
        /// y el uid no
        /// </summary>
        /// <param name="provincia"></param>
        /// <param name="tipo"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static async Task<List<clsMascota>> GetPetsProvinciaTipo(string provincia, string tipo, string uid)
        {
            List<clsMascota> mascotas = new List<clsMascota>();
            try
            {
                FirebaseClient cliente = new FirebaseClient(firebaseUrl);
                var pets = await cliente
                    .Child("Pets")
                    .OrderByKey()
                    .OnceAsync<clsMascota>();
                foreach (var pet in pets)
                {
                    if (pet.Object.Provincia == provincia && pet.Object.Tipo == tipo && pet.Object.UidUser != uid)
                    {
                        mascotas.Add(
                            new clsMascota
                            {
                                Titulo = pet.Object.Titulo,
                                Descripcion = pet.Object.Descripcion,
                                Provincia = pet.Object.Provincia,
                                Localidad = pet.Object.Localidad,
                                Tipo = pet.Object.Tipo,
                                UrlImagen = pet.Object.UrlImagen,
                                UidUser = pet.Object.UidUser,
                                Key = pet.Key,
                            });
                    }
                }
            }
            catch (Exception)
            {

            }
            return mascotas;
        }

        /// <summary>
        /// Metodo que obtiene todas las mascotas cuya provincia sean la misma que la pasada por parametros
        /// y el uid no
        /// </summary>
        /// <param name="provincia"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static async Task<List<clsMascota>> GetPetsProvincia(string provincia, string uid)
        {
            List<clsMascota> mascotas = new List<clsMascota>();
            try
            {
                FirebaseClient cliente = new FirebaseClient(firebaseUrl);
                var pets = await cliente
                    .Child("Pets")
                    .OrderByKey()
                    .OnceAsync<clsMascota>();
                foreach (var pet in pets)
                {
                    if (pet.Object.Provincia == provincia && pet.Object.UidUser != uid)
                    {
                        mascotas.Add(
                            new clsMascota
                            {
                                Titulo = pet.Object.Titulo,
                                Descripcion = pet.Object.Descripcion,
                                Provincia = pet.Object.Provincia,
                                Localidad = pet.Object.Localidad,
                                Tipo = pet.Object.Tipo,
                                UrlImagen = pet.Object.UrlImagen,
                                UidUser = pet.Object.UidUser,
                                Key = pet.Key,
                            });
                    }
                }
            }
            catch (Exception)
            {

            }
            return mascotas;
        }

        /// <summary>
        /// Metodo que obtiene todas las mascotas cuyo tipo sean el mismo que el pasado por parametros
        /// y el uid no
        /// </summary>
        /// <param name="tipo"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public static async Task<List<clsMascota>> GetPetsTipo(string tipo, string uid)
        {
            List<clsMascota> mascotas = new List<clsMascota>();
            try
            {
                FirebaseClient cliente = new FirebaseClient(firebaseUrl);
                var pets = await cliente
                    .Child("Pets")
                    .OrderByKey()
                    .OnceAsync<clsMascota>();
                foreach (var pet in pets)
                {
                    if (pet.Object.Tipo == tipo && pet.Object.UidUser != uid)
                    {
                        mascotas.Add(
                            new clsMascota
                            {
                                Titulo = pet.Object.Titulo,
                                Descripcion = pet.Object.Descripcion,
                                Provincia = pet.Object.Provincia,
                                Localidad = pet.Object.Localidad,
                                Tipo = pet.Object.Tipo,
                                UrlImagen = pet.Object.UrlImagen,
                                UidUser = pet.Object.UidUser,
                                Key = pet.Key,
                            });
                    }
                }
            }
            catch (Exception)
            {

            }
            return mascotas;
        }

        /// <summary>
        /// Metodo que envia un mensaje, se indica el key segun el identificador del chat y se le pasa por parametros
        /// el objeto chat
        /// </summary>
        /// <param name="key"></param>
        /// <param name="mensaje"></param>
        /// <returns></returns>
        public static async Task<bool> SendMessage(string key, clsMensaje mensaje)
        {
            bool salida = false;
            try
            {
                FirebaseClient cliente = new FirebaseClient(
                firebaseUrl,
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(mensaje.Uid)
                });
                var response = cliente
                        .Child("Chats")
                        .Child(key)
                        .Child("Mensajes")
                        .PostAsync(mensaje);
                if (response != null)
                {
                    salida = true;
                }
            }
            catch (Exception)
            {

            }
            return salida;
        }

        /// <summary>
        /// Metodo que obtiene los mensajes segun la clave de del chat
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<List<clsMensaje>> GetMessages(string key, string ultimoMensaje)
        {
            List<clsMensaje> lMensajes = new List<clsMensaje>();
            try
            {
                FirebaseClient cliente = new FirebaseClient(firebaseUrl);
                IReadOnlyCollection<FirebaseObject<clsMensaje>>? mensajes;
                if (ultimoMensaje == null)
                {
                    mensajes = await cliente
                                .Child("Chats")
                                .Child(key)
                                .Child("Mensajes")
                                .OrderByKey()
                                .OnceAsync<clsMensaje>();
                }
                else
                {
                    mensajes = await cliente
                                .Child("Chats")
                                .Child(key)
                                .Child("Mensajes")
                                .OrderByKey()
                                .StartAt(ultimoMensaje)
                                .OnceAsync<clsMensaje>();
                }

                foreach (var mensaje in mensajes)
                {
                    lMensajes.Add(
                        new clsMensaje
                        {
                            Texto = mensaje.Object.Texto,
                            Hora = mensaje.Object.Hora,
                            Uid = mensaje.Object.Uid,
                            Key = mensaje.Key
                        });
                }
            }
            catch (Exception)
            {

            }
            return lMensajes;
        }
    }
}
