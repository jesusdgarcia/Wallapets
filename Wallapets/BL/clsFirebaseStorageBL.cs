using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BL
{
    public class clsFirebaseStorageBL
    {
        /// <summary>
        /// Metodo que sube una imagen, el token es el uid del usuario, el nombre del archivo
        /// y el path de la imagen
        /// </summary>
        /// <param name="token"></param>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Task<String> UploadImageBL(string padre, string name, string path)
        {
            return clsFirebaseStorage.UploadImage(padre, name, path);
        }
    }
}
