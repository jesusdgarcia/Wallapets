using Firebase.Storage;

namespace DAL
{
    public class clsFirebaseStorage
    {
        private static string firebaseUrl = "wallapets-73a5a.appspot.com";

        /// <summary>
        /// Metodo que sube una imagen, el token es el uid del usuario, el nombre del archivo
        /// y el path de la imagen
        /// </summary>
        /// <param name="token"></param>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static async Task<String> UploadImage(string token, string name, string path)
        {
            var stream = File.Open(path, FileMode.Open);
            var firebaseStorage = "";
            try
            {
                firebaseStorage = await new FirebaseStorage(
                    firebaseUrl,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(token),
                        ThrowOnCancel = true,
                    })
                    .Child(token)
                    .Child(name)
                    .PutAsync(stream);
            }catch (Exception)
            {

            }
            return firebaseStorage;
        }
    }
}
