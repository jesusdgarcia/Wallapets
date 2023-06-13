using ENTITIES;
using Firebase.Auth;
using Firebase.Auth.Providers;

namespace DAL
{
    public class clsFirebase
    {
        private static FirebaseAuthClient client;

        /// <summary>
        /// Metodo que crea la configuracion de firebase
        /// </summary>
        /// <returns>config</returns>
        private static FirebaseAuthConfig config()
        {
            var config = new FirebaseAuthConfig
            {
                ApiKey = "AIzaSyCtGrUE-Gu4iifoOebjWgoon9TwXqT-PBo",
                AuthDomain = "wallapets-73a5a.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                {
                    // Add and configure individual providers
                    new GoogleProvider().AddScopes("email"),
                    new EmailProvider()
                }
            };
            return config;
        }

        /// <summary>
        /// Metodo que inicia sesion de un usuario pasado por parametros
        /// Devuelve el credencial de usuario iniciado correctamente o no
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public static Task<UserCredential> Login(clsUser usuario)
        {
            Task<UserCredential> credential = null;
            try
            {
                client = new FirebaseAuthClient(config());
                credential = client.SignInWithEmailAndPasswordAsync(usuario.Email, usuario.Password);
            }
            catch (Exception) 
            {
                
            }
            return credential;
        }

        /// <summary>
        /// Metodo que registra un usuario pasado por parametros
        /// Devuelve el credencial de usuario creado correctamente o no
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public static Task<UserCredential> Register(clsUser usuario)
        {
            Task<UserCredential> credential = null;
            try
            {
                client = new FirebaseAuthClient(config());
                credential = client.CreateUserWithEmailAndPasswordAsync(usuario.Email, usuario.Password, usuario.Name);
            } catch (Exception)
            {

            }
            return credential;
        }

        /// <summary>
        /// Metodo que te finaliza la conexion del usuario
        /// </summary>
        public static void Logout()
        {
            client.SignOut();
        }
    }
}
